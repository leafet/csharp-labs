namespace CSharpLabs;

public class Lab1
{
    private readonly string[] _days =
    {
        "понедельник", "вторник", "среда", "четверг", "пятница", "суббота", "воскресенье"
    };

    /// <summary>
    /// Запуск дружественного интерфейса (меню) и вызов всех методов класса.
    /// Все данные, необходимые задачам, вводятся с клавиатуры с обязательной проверкой ввода.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("1 - Сумма знаков. Сумма двух последних цифр числа.\n" +
                              "2 - Есть ли позитив. Проверяет, положительное ли число.\n" +
                              "3 - Большая буква. Проверяет, заглавная ли это латинская буква.\n" +
                              "4 - Делитель. Проверяет, делится ли одно число на другое.\n" +
                              "5 - Многократный вызов. Складывает последние цифры пяти чисел по очереди.\n" +
                              "6 - Безопасное деление. При делении на ноль возвращает 0.\n" +
                              "7 - Строка сравнения. Показывает знак сравнения двух чисел.\n" +
                              "8 - Тройная сумма. Проверяет, дают ли два числа в сумме третье.\n" +
                              "9 - Возраст. Подбирает слово «год», «года» или «лет».\n" +
                              "10 - Вывод дней недели. Выводит дни от выбранного до воскресенья.\n" +
                              "11 - Числа наоборот. Выводит числа от заданного до нуля.\n" +
                              "12 - Степень числа. Возводит число в заданную степень.\n" +
                              "13 - Одинаковость. Проверяет, одинаковы ли все цифры числа.\n" +
                              "14 - Левый треугольник. Рисует треугольник из звёздочек.\n" +
                              "15 - Угадайка. Нужно угадать число от 0 до 9.\n" +
                              "16 - Поиск последнего значения. Находит последнее вхождение числа в массив.\n" +
                              "17 - Добавление в массив. Вставляет число в выбранную позицию.\n" +
                              "18 - Реверс. Меняет порядок элементов массива на обратный.\n" +
                              "19 - Объединение. Соединяет два массива в один.\n" +
                              "20 - Удалить негатив. Удаляет отрицательные числа из массива.\n" +
                              "0 - Выход.\n");

            int selection = ReadInt("Введите номер задания: ", 0, 20);

            switch (selection)
            {
                case 0:
                    return;

                // Задание 1. Методы
                case 1:
                    Console.WriteLine(sumLastNums(ReadTwoDigitInt("Введите число x: ")));
                    break;
                case 2:
                    Console.WriteLine(isPositive(ReadInt("Введите число x: ")));
                    break;
                case 3:
                    Console.WriteLine(isUpperCase(ReadChar("Введите символ: ")));
                    break;
                case 4:
                    Console.WriteLine(isDivisor(ReadInt("Введите число a: "), ReadInt("Введите число b: ")));
                    break;
                case 5:
                    Console.WriteLine("Последовательное сложение пяти чисел (сумма цифр разряда единиц).");
                    int result = ReadInt("Введите 1-е число: ");
                    for (int i = 2; i <= 5; i++)
                    {
                        int next = ReadInt($"Введите {i}-е число: ");
                        Console.WriteLine($"{result}+{next} это {lastNumSum(result, next)}");
                        result = lastNumSum(result, next);
                    }

                    Console.WriteLine($"Итого {result}");
                    break;

                // Задание 2. Условия
                case 6:
                    Console.WriteLine(safeDiv(ReadInt("Введите делимое x: "), ReadInt("Введите делитель y: ")));
                    break;
                case 7:
                    Console.WriteLine(makeDecision(ReadInt("Введите число x: "), ReadInt("Введите число y: ")));
                    break;
                case 8:
                    Console.WriteLine(sum3(ReadInt("Введите число x: "), ReadInt("Введите число y: "),
                        ReadInt("Введите число z: ")));
                    break;
                case 9:
                    Console.WriteLine(age(ReadInt("Введите возраст x: ", 0, int.MaxValue)));
                    break;
                case 10:
                    Console.Write("Введите название дня недели: ");
                    printDays(Console.ReadLine() ?? throw new EndOfStreamException());
                    break;

                // Задание 3. Циклы
                case 11:
                    Console.WriteLine(reverseListNums(ReadInt("Введите число x: ", 0, 1000)));
                    break;
                case 12:
                    while (true)
                    {
                        int number = ReadInt("Введите основание x: ");
                        int exponent = ReadInt("Введите показатель y: ", 0, 1000);

                        try
                        {
                            Console.WriteLine(pow(number, exponent));
                            break;
                        }
                        catch (OverflowException)
                        {
                            Console.WriteLine("Результат выходит за диапазон int. Введите другие значения.");
                        }
                    }

                    break;
                case 13:
                    Console.WriteLine(equalNum(ReadInt("Введите число x: ")));
                    break;
                case 14:
                    leftTriangle(ReadInt("Введите высоту треугольника x: ", 1, 50));
                    break;
                case 15:
                    guessGame();
                    break;

                // Задание 4. Массивы
                case 16:
                    PrintArrayInfo(findLast(ReadArray("Введите элементы массива через пробел: "),
                        ReadInt("Введите искомое число x: ")));
                    break;
                case 17:
                    int[] source = ReadArray("Введите элементы массива через пробел: ");
                    int inserted = ReadInt("Введите вставляемое значение x: ");
                    int position = ReadInt("Введите позицию pos: ", 0, source.Length);
                    Console.WriteLine(ArrayToString(add(source, inserted, position)));
                    break;
                case 18:
                    int[] arr = ReadArray("Введите элементы массива через пробел: ");
                    reverse(arr);
                    Console.WriteLine($"Изменённый массив: {ArrayToString(arr)}");
                    break;
                case 19:
                    Console.WriteLine(ArrayToString(concat(ReadArray("Введите элементы первого массива через пробел: "),
                        ReadArray("Введите элементы второго массива через пробел: "))));
                    break;
                case 20:
                    Console.WriteLine(ArrayToString(deleteNegative(ReadArray("Введите элементы массива через пробел: "))));
                    break;

                default:
                    Console.WriteLine("Такого задания не существует");
                    break;
            }

            Console.WriteLine();
        }
    }

    // ---------------------------------------------------------------------------------------
    // Задание 1. Методы
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Сумма знаков. Возвращает сумму двух последних цифр числа
    /// (предполагается, что цифр в числе не менее двух).
    /// </summary>
    /// <param name="x">Исходное число.</param>
    /// <returns>Сумма двух последних цифр числа x.</returns>
    /// <example>Пример: x=4568, результат: 14</example>
    public int sumLastNums(int x)
    {
        int y = 0;

        y += OnesDigit(x);
        x /= 10;

        y += OnesDigit(x);

        return y;
    }

    /// <summary>
    /// Есть ли позитив. Возвращает true, если число положительное.
    /// </summary>
    /// <param name="x">Проверяемое число.</param>
    /// <returns>true, если x &gt; 0, иначе false.</returns>
    /// <example>Пример 1: x=3, результат: true. Пример 2: x=-5, результат: false</example>
    public bool isPositive(int x)
    {
        return x > 0;
    }

    /// <summary>
    /// Большая буква. Возвращает true, если символ является заглавной буквой
    /// латинского алфавита (диапазон кодов от 'A' до 'Z').
    /// </summary>
    /// <param name="x">Проверяемый символ.</param>
    /// <returns>true, если символ входит в диапазон 'A'-'Z', иначе false.</returns>
    /// <example>Пример 1: x='D', результат: true. Пример 2: x='q', результат: false</example>
    public bool isUpperCase(char x)
    {
        return x >= 'A' && x <= 'Z';
    }

    /// <summary>
    /// Делитель. Возвращает true, если любое из двух принятых чисел делит другое нацело.
    /// </summary>
    /// <param name="a">Первое число.</param>
    /// <param name="b">Второе число.</param>
    /// <returns>true, если a делится на b или b делится на a без остатка, иначе false.</returns>
    /// <example>Пример 1: a=3, b=6, результат: true. Пример 2: a=2, b=15, результат: false</example>
    public bool isDivisor(int a, int b)
    {
        if (b != 0 && (long)a % b == 0)
        {
            return true;
        }

        if (a != 0 && (long)b % a == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Многократный вызов. Возвращает сумму цифр разряда единиц двух чисел.
    /// </summary>
    /// <param name="a">Первое число.</param>
    /// <param name="b">Второе число.</param>
    /// <returns>Сумма последних цифр чисел a и b.</returns>
    /// <example>Пример: 5+11 это 6; 6+123 это 9; 9+14 это 13; 13+1 это 4. Итого: 4</example>
    public int lastNumSum(int a, int b)
    {
        return OnesDigit(a) + OnesDigit(b);
    }

    // ---------------------------------------------------------------------------------------
    // Задание 2. Условия
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Безопасное деление. Возвращает частное x/y, гарантированно не допуская
    /// деления на ноль: при y = 0 возвращается 0.
    /// </summary>
    /// <param name="x">Делимое.</param>
    /// <param name="y">Делитель.</param>
    /// <returns>Результат деления x на y либо 0, если y = 0.</returns>
    /// <example>Пример 1: x=5, y=0, результат: 0. Пример 2: x=8, y=2, результат: 4</example>
    public double safeDiv(int x, int y)
    {
        if (y == 0)
        {
            return 0;
        }

        return (double)x / y;
    }

    /// <summary>
    /// Строка сравнения. Возвращает строку из двух чисел и корректного знака
    /// сравнения (больше, меньше или равно).
    /// </summary>
    /// <param name="x">Левое число.</param>
    /// <param name="y">Правое число.</param>
    /// <returns>Строка вида "x &lt; y", "x &gt; y" или "x == y".</returns>
    /// <example>Пример 1: x=5, y=7, результат: "5 &lt; 7". Пример 2: x=8, y=-1, результат: "8 &gt; -1".
    /// Пример 3: x=4, y=4, результат: "4 == 4"</example>
    public string makeDecision(int x, int y)
    {
        if (x < y)
        {
            return $"{x} < {y}";
        }

        if (x > y)
        {
            return $"{x} > {y}";
        }

        return $"{x} == {y}";
    }

    /// <summary>
    /// Тройная сумма. Возвращает true, если два любых из трёх принятых чисел
    /// в сумме дают третье число.
    /// </summary>
    /// <param name="x">Первое число.</param>
    /// <param name="y">Второе число.</param>
    /// <param name="z">Третье число.</param>
    /// <returns>true, если сумма каких-либо двух чисел равна третьему, иначе false.</returns>
    /// <example>Пример 1: x=5, y=7, z=2, результат: true. Пример 2: x=8, y=-1, z=4, результат: false</example>
    public bool sum3(int x, int y, int z)
    {
        return (long)x + y == z || (long)x + z == y || (long)y + z == x;
    }

    /// <summary>
    /// Возраст. Возвращает строку с числом и правильно подобранным словом:
    /// "год" - если число заканчивается на 1 (кроме 11),
    /// "года" - если заканчивается на 2, 3 или 4 (кроме 12, 13, 14),
    /// "лет" - во всех остальных случаях.
    /// </summary>
    /// <param name="x">Возраст.</param>
    /// <returns>Строка вида "5 лет", "31 год", "44 года".</returns>
    /// <example>Пример 1: x=5, результат: "5 лет". Пример 2: x=31, результат: "31 год".
    /// Пример 3: x=44, результат: "44 года"</example>
    public string age(int x)
    {
        long n = Math.Abs((long)x);
        long lastDigit = n % 10;
        long lastTwoDigits = n % 100;

        if (lastTwoDigits >= 11 && lastTwoDigits <= 14)
        {
            return $"{x} лет";
        }

        if (lastDigit == 1)
        {
            return $"{x} год";
        }

        if (lastDigit == 2 || lastDigit == 3 || lastDigit == 4)
        {
            return $"{x} года";
        }

        return $"{x} лет";
    }

    /// <summary>
    /// Вывод дней недели. Выводит на экран название переданного дня
    /// и все последующие дни до конца недели (первый день - понедельник, последний - воскресенье).
    /// Вместо if используется switch.
    /// </summary>
    /// <param name="x">Название дня недели.</param>
    /// <example>Пример 1: x="четверг", результат: четверг, пятница, суббота, воскресенье.
    /// Пример 2: x="чг", результат: "это не день недели"</example>
    public void printDays(string x)
    {
        int start = -1;

        switch (x.ToLower())
        {
            case "понедельник":
                start = 0;
                break;
            case "вторник":
                start = 1;
                break;
            case "среда":
                start = 2;
                break;
            case "четверг":
                start = 3;
                break;
            case "пятница":
                start = 4;
                break;
            case "суббота":
                start = 5;
                break;
            case "воскресенье":
                start = 6;
                break;
            default:
                Console.WriteLine("это не день недели");
                return;
        }

        for (int i = start; i < _days.Length; i++)
        {
            Console.WriteLine(_days[i]);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Задание 3. Циклы
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Числа наоборот. Возвращает строку со всеми числами от x до 0 включительно.
    /// </summary>
    /// <param name="x">Верхняя граница диапазона.</param>
    /// <returns>Строка вида "x ... 2 1 0".</returns>
    /// <example>Пример: x=5, результат: "5 4 3 2 1 0"</example>
    public string reverseListNums(int x)
    {
        string result = "";

        for (int i = x; i >= 0; i--)
        {
            result += i;

            if (i > 0)
            {
                result += " ";
            }
        }

        return result;
    }

    /// <summary>
    /// Степень числа. Возвращает результат возведения x в степень y:
    /// единица умножается на x ровно y раз.
    /// </summary>
    /// <param name="x">Основание степени.</param>
    /// <param name="y">Показатель степени (не отрицательный).</param>
    /// <returns>Значение x в степени y.</returns>
    /// <example>Пример: x=2, y=5, результат: 32</example>
    public int pow(int x, int y)
    {
        if (y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        int result = 1;

        for (int i = 0; i < y; i++)
        {
            result = checked(result * x);
        }

        return result;
    }

    /// <summary>
    /// Одинаковость. Возвращает true, если все цифры числа одинаковы.
    /// </summary>
    /// <param name="x">Проверяемое число.</param>
    /// <returns>true, если все цифры числа совпадают, иначе false.</returns>
    /// <example>Пример 1: x=1111, результат: true. Пример 2: x=1211, результат: false</example>
    public bool equalNum(int x)
    {
        long n = Math.Abs((long)x);
        long lastDigit = n % 10;

        while (n > 0)
        {
            if (n % 10 != lastDigit)
            {
                return false;
            }

            n /= 10;
        }

        return true;
    }

    /// <summary>
    /// Левый треугольник. Выводит на экран треугольник из символов '*':
    /// x строк в высоту, количество символов в ряду совпадает с номером строки.
    /// </summary>
    /// <param name="x">Высота треугольника.</param>
    /// <example>Пример: x=4, результат:
    /// *
    /// **
    /// ***
    /// ****</example>
    public void leftTriangle(int x)
    {
        for (int i = 1; i <= x; i++)
        {
            for (int j = 0; j < i; j++)
            {
                Console.Write("*");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Угадайка. Генерирует случайное число от 0 до 9 и считывает варианты с консоли,
    /// пока пользователь не угадает число. После победы выводит количество попыток.
    /// Метод работает в цикле и проверяет корректность ввода.
    /// </summary>
    /// <example>Пример:
    /// Введите число от 0 до 9: 5
    /// Вы не угадали, введите число от 0 до 9: 9
    /// Вы угадали! Вы отгадали число за 2 попытки</example>
    public void guessGame()
    {
        Random random = new Random();
        int secret = random.Next(0, 10);
        int attempts = 0;

        while (true)
        {
            int answer;

            if (attempts == 0)
            {
                answer = ReadInt("Введите число от 0 до 9: ", 0, 9);
            }
            else
            {
                answer = ReadInt("Вы не угадали, введите число от 0 до 9: ", 0, 9);
            }

            attempts++;

            if (answer == secret)
            {
                break;
            }
        }

        Console.WriteLine("Вы угадали!");

        int lastDigit = attempts % 10;
        int lastTwoDigits = attempts % 100;
        string word = "попыток";

        if (lastTwoDigits < 11 || lastTwoDigits > 14)
        {
            if (lastDigit == 1)
            {
                word = "попытка";
            }
            else if (lastDigit >= 2 && lastDigit <= 4)
            {
                word = "попытки";
            }
        }

        Console.WriteLine($"Вы отгадали число за {attempts} {word}");
    }

    // ---------------------------------------------------------------------------------------
    // Задание 4. Массивы
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Поиск последнего значения. Возвращает индекс последнего вхождения числа x в массив arr.
    /// </summary>
    /// <param name="arr">Массив для поиска.</param>
    /// <param name="x">Искомое число.</param>
    /// <returns>Индекс последнего вхождения x или -1, если числа в массиве нет.</returns>
    /// <example>Пример: arr=[1,2,3,4,2,2,5], x=2, результат: 5</example>
    public int findLast(int[] arr, int x)
    {
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            if (arr[i] == x)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Добавление в массив. Возвращает новый массив, в который в позицию pos
    /// вставлено значение x (остальные элементы сохраняют порядок).
    /// </summary>
    /// <param name="arr">Исходный массив.</param>
    /// <param name="x">Вставляемое значение.</param>
    /// <param name="pos">Позиция вставки от 0 до длины массива включительно.</param>
    /// <returns>Новый массив длиной arr.Length + 1 со вставленным значением.</returns>
    /// <example>Пример: arr=[1,2,3,4,5], x=9, pos=3, результат: [1,2,3,9,4,5]</example>
    public int[] add(int[] arr, int x, int pos)
    {
        if (pos < 0 || pos > arr.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(pos));
        }

        int[] result = new int[arr.Length + 1];

        for (int i = 0; i < pos; i++)
        {
            result[i] = arr[i];
        }

        result[pos] = x;

        for (int i = pos; i < arr.Length; i++)
        {
            result[i + 1] = arr[i];
        }

        return result;
    }

    /// <summary>
    /// Реверс. Изменяет переданный массив на месте, записывая его элементы задом наперёд.
    /// </summary>
    /// <param name="arr">Массив, который нужно перевернуть.</param>
    /// <example>Пример: arr=[1,2,3,4,5], результат: arr=[5,4,3,2,1]</example>
    public void reverse(int[] arr)
    {
        int left = 0;
        int right = arr.Length - 1;

        while (left < right)
        {
            int temp = arr[left];
            arr[left] = arr[right];
            arr[right] = temp;

            left++;
            right--;
        }
    }

    /// <summary>
    /// Объединение. Возвращает новый массив, в котором сначала идут элементы arr1,
    /// а затем элементы arr2.
    /// </summary>
    /// <param name="arr1">Первый массив.</param>
    /// <param name="arr2">Второй массив.</param>
    /// <returns>Новый массив длиной arr1.Length + arr2.Length.</returns>
    /// <example>Пример: arr1=[1,2,3], arr2=[7,8,9], результат: [1,2,3,7,8,9]</example>
    public int[] concat(int[] arr1, int[] arr2)
    {
        int[] result = new int[arr1.Length + arr2.Length];

        for (int i = 0; i < arr1.Length; i++)
        {
            result[i] = arr1[i];
        }

        for (int i = 0; i < arr2.Length; i++)
        {
            result[arr1.Length + i] = arr2[i];
        }

        return result;
    }

    /// <summary>
    /// Удалить негатив. Возвращает новый массив со всеми элементами arr, кроме отрицательных.
    /// </summary>
    /// <param name="arr">Исходный массив.</param>
    /// <returns>Новый массив, содержащий только неотрицательные элементы.</returns>
    /// <example>Пример: arr=[1,2,-3,4,-2,2,-5], результат: [1,2,4,2]</example>
    public int[] deleteNegative(int[] arr)
    {
        int count = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0)
            {
                count++;
            }
        }

        int[] result = new int[count];
        int index = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] >= 0)
            {
                result[index] = arr[i];
                index++;
            }
        }

        return result;
    }

    // ---------------------------------------------------------------------------------------
    // Вспомогательные методы (private)
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Возвращает цифру разряда единиц числа без учёта знака.
    /// </summary>
    /// <param name="x">Исходное число.</param>
    /// <returns>Последняя цифра числа (0-9).</returns>
    private int OnesDigit(int x)
    {
        int digit = x % 10;

        if (digit < 0)
        {
            digit = -digit;
        }

        return digit;
    }

    /// <summary>
    /// Проверка ввода целого числа: при некорректном вводе просит повторить.
    /// </summary>
    /// <param name="prompt">Текст приглашения ко вводу.</param>
    /// <param name="min">Минимальное допустимое значение.</param>
    /// <param name="max">Максимальное допустимое значение.</param>
    /// <returns>Введённое целое число.</returns>
    private int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();

            if (line == null)
            {
                throw new EndOfStreamException();
            }

            if (int.TryParse(line, out int value))
            {
                if (value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Ошибка ввода: допустимый диапазон от {min} до {max}.");
                continue;
            }

            Console.WriteLine("Ошибка ввода: ожидалось целое число, попробуйте ещё раз.");
        }
    }

    /// <summary>
    /// Читает целое число, имеющее не менее двух цифр без учёта знака.
    /// </summary>
    private int ReadTwoDigitInt(string prompt)
    {
        while (true)
        {
            int value = ReadInt(prompt);
            if (Math.Abs((long)value) >= 10)
            {
                return value;
            }

            Console.WriteLine("Ошибка ввода: число должно содержать не менее двух цифр.");
        }
    }

    /// <summary>
    /// Проверка ввода символа: при пустом вводе просит повторить.
    /// </summary>
    /// <param name="prompt">Текст приглашения ко вводу.</param>
    /// <returns>Первый введённый символ.</returns>
    private char ReadChar(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();

            if (line == null)
            {
                throw new EndOfStreamException();
            }

            if (line.Length == 1)
            {
                return line[0];
            }

            Console.WriteLine("Ошибка ввода: ожидался символ, попробуйте ещё раз.");
        }
    }

    /// <summary>
    /// Проверка ввода массива целых чисел (элементы через пробел или запятую).
    /// </summary>
    /// <param name="prompt">Текст приглашения ко вводу.</param>
    /// <returns>Массив введённых чисел.</returns>
    private int[] ReadArray(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? line = Console.ReadLine();

            if (line == null)
            {
                throw new EndOfStreamException();
            }

            if (line.Trim() == "")
            {
                Console.WriteLine("Ошибка ввода: массив пуст, попробуйте ещё раз.");
                continue;
            }

            string[] parts = line.Replace(",", " ").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int[] arr = new int[parts.Length];
            bool correct = true;

            for (int i = 0; i < parts.Length; i++)
            {
                if (!int.TryParse(parts[i], out arr[i]))
                {
                    correct = false;
                    break;
                }
            }

            if (correct)
            {
                return arr;
            }

            Console.WriteLine("Ошибка ввода: все элементы массива должны быть целыми числами.");
        }
    }

    /// <summary>
    /// Возвращает строковое представление массива в виде "[a,b,c]".
    /// </summary>
    /// <param name="arr">Массив для вывода.</param>
    /// <returns>Строка с элементами массива, заключённая в квадратные скобки.</returns>
    private string ArrayToString(int[] arr)
    {
        string result = "[";

        for (int i = 0; i < arr.Length; i++)
        {
            result += arr[i];

            if (i < arr.Length - 1)
            {
                result += ", ";
            }
        }

        return result + "]";
    }

    /// <summary>
    /// Дружественный вывод результата поиска индекса в массиве.
    /// </summary>
    /// <param name="index">Найденный индекс или -1.</param>
    private void PrintArrayInfo(int index)
    {
        if (index == -1)
        {
            Console.WriteLine("Число не входит в массив.");
        }
        else
        {
            Console.WriteLine($"Индекс последнего вхождения: {index}");
        }
    }
}
