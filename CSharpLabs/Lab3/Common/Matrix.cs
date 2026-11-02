using System.Globalization;
using System.Text;
using CSharpLabs.Common;

namespace CSharpLabs.Lab3.Common;

public class Matrix
{
    private readonly long[,] _values;

    /// <summary>Вводит прямоугольную матрицу по столбцам сверху вниз.</summary>
    public Matrix(int rows, int columns)
    {
        ValidateDimensions(rows, columns);
        _values = new long[rows, columns];

        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                _values[row, column] = ConsoleInput.ReadInt(
                    $"Элемент [{row + 1}, {column + 1}]: ", int.MinValue, int.MaxValue);
            }
        }
    }

    public Matrix(int size, Random random)
    {
        ValidateDimensions(size, size);
        ArgumentNullException.ThrowIfNull(random);
        _values = new long[size, size];

        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                _values[row, column] = row + column < size - 1
                    ? random.Next(-12, 4566)
                    : random.Next(-1024, 1025);
            }
        }
    }

    /// <summary>Заполняет нижний правый треугольник по диагоналям от правого нижнего угла.</summary>
    public Matrix(int size)
    {
        ValidateDimensions(size, size);
        _values = new long[size, size];
        long value = 1;

        for (int diagonal = 0; diagonal < size; diagonal++)
        {
            for (int offset = 0; offset <= diagonal; offset++)
            {
                _values[size - 1 - offset, size - 1 - diagonal + offset] = value;
                value++;
            }
        }
    }

    /// <summary>Создаёт независимую копию переданного массива.</summary>
    public Matrix(long[,] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        ValidateDimensions(values.GetLength(0), values.GetLength(1));
        _values = (long[,])values.Clone();
    }

    public int Rows
    {
        get
        {
            return _values.GetLength(0);
        }
    }

    public int Columns
    {
        get
        {
            return _values.GetLength(1);
        }
    }

    public long this[int row, int column]
    {
        get
        {
            return _values[row, column];
        }
    }

    /// <summary>Находит первый при обходе по строкам блок 3×3 с максимальной суммой.</summary>
    public Matrix FindMaximumBlock(out int top, out int left, out long sum)
    {
        if (Rows < 3 || Columns < 3)
        {
            throw new InvalidOperationException("Для блока 3×3 нужны минимум три строки и три столбца.");
        }

        top = 0;
        left = 0;
        sum = long.MinValue;

        for (int row = 0; row <= Rows - 3; row++)
        {
            for (int column = 0; column <= Columns - 3; column++)
            {
                long current = 0;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        current = checked(current + _values[row + i, column + j]);
                    }
                }

                if (current > sum)
                {
                    sum = current;
                    top = row;
                    left = column;
                }
            }
        }

        long[,] block = new long[3, 3];

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                block[row, column] = _values[top + row, left + column];
            }
        }

        return new Matrix(block);
    }

    public Matrix Transpose()
    {
        long[,] result = new long[Columns, Rows];

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                result[column, row] = _values[row, column];
            }
        }

        return new Matrix(result);
    }

    public static Matrix operator +(Matrix first, Matrix second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.Rows != second.Rows || first.Columns != second.Columns)
        {
            throw new ArgumentException("Размеры слагаемых матриц должны совпадать.");
        }

        long[,] result = new long[first.Rows, first.Columns];

        for (int row = 0; row < first.Rows; row++)
        {
            for (int column = 0; column < first.Columns; column++)
            {
                result[row, column] = checked(first[row, column] + second[row, column]);
            }
        }

        return new Matrix(result);
    }

    public static Matrix operator *(long factor, Matrix matrix)
    {
        ArgumentNullException.ThrowIfNull(matrix);
        long[,] result = new long[matrix.Rows, matrix.Columns];

        for (int row = 0; row < matrix.Rows; row++)
        {
            for (int column = 0; column < matrix.Columns; column++)
            {
                result[row, column] = checked(factor * matrix[row, column]);
            }
        }

        return new Matrix(result);
    }

    public static Matrix operator *(Matrix matrix, long factor)
    {
        return factor * matrix;
    }

    public override string ToString()
    {
        int width = 1;

        foreach (long value in _values)
        {
            width = Math.Max(width, value.ToString(CultureInfo.InvariantCulture).Length);
        }

        StringBuilder result = new StringBuilder();

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (column > 0)
                {
                    result.Append(' ');
                }

                string value = _values[row, column].ToString(CultureInfo.InvariantCulture);
                result.Append(value.PadLeft(width));
            }

            if (row < Rows - 1)
            {
                result.AppendLine();
            }
        }

        return result.ToString();
    }

    private static void ValidateDimensions(int rows, int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rows);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(columns);
    }
}
