using Lab5.ExcelLibrary.Models;

namespace Lab5.ExcelLibrary;

public abstract class FileDbContext
{
    private readonly Dictionary<Type, object> _sets = new();

    public string FilePath { get; protected set; }

    protected FileDbContext(string path)
    {
        FilePath = Path.GetFullPath(path);
    }

    protected void RegisterSet<T>(List<T> items, Action<T>? validateWrite = null,
        Action<T>? validateRemove = null) where T : Entity
    {
        _sets.Add(typeof(T), new EntitySet<T>(items, validateWrite, validateRemove));
    }

    public IReadOnlyList<T> Set<T>() where T : Entity
    {
        return GetSet<T>().View;
    }

    public T? Find<T>(int id) where T : Entity
    {
        return GetSet<T>().Items.Find(item => item.Id == id);
    }

    public void Add<T>(T item) where T : Entity
    {
        ArgumentNullException.ThrowIfNull(item);
        EntitySet<T> set = GetSet<T>();

        if (set.Items.Exists(existing => existing.Id == item.Id))
        {
            throw new ArgumentException("Запись с таким ID уже существует.", nameof(item));
        }

        set.ValidateWrite?.Invoke(item);
        set.Items.Add(item);
    }

    public bool Update<T>(T replacement) where T : Entity
    {
        ArgumentNullException.ThrowIfNull(replacement);
        EntitySet<T> set = GetSet<T>();
        int index = set.Items.FindIndex(item => item.Id == replacement.Id);

        if (index < 0)
        {
            return false;
        }

        set.ValidateWrite?.Invoke(replacement);
        set.Items[index] = replacement;
        return true;
    }

    public bool Remove<T>(int id) where T : Entity
    {
        EntitySet<T> set = GetSet<T>();
        int index = set.Items.FindIndex(item => item.Id == id);

        if (index < 0)
        {
            return false;
        }

        set.ValidateRemove?.Invoke(set.Items[index]);
        set.Items.RemoveAt(index);
        return true;
    }

    public abstract void SaveChanges(string? path = null);

    private EntitySet<T> GetSet<T>() where T : Entity
    {
        if (!_sets.TryGetValue(typeof(T), out object? set))
        {
            throw new InvalidOperationException($"Набор {typeof(T).Name} не зарегистрирован.");
        }

        return (EntitySet<T>)set;
    }

    private sealed class EntitySet<T> where T : Entity
    {
        public List<T> Items { get; }
        public IReadOnlyList<T> View { get; }
        public Action<T>? ValidateWrite { get; }
        public Action<T>? ValidateRemove { get; }

        public EntitySet(List<T> items, Action<T>? validateWrite, Action<T>? validateRemove)
        {
            Items = items;
            View = items.AsReadOnly();
            ValidateWrite = validateWrite;
            ValidateRemove = validateRemove;
        }
    }
}
