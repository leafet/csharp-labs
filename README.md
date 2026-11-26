# Лабораторные работы по C#

Решения ЛР1–5 находятся в трёх проектах. Основное меню запускает ЛР1–3 и задания 1–5 ЛР4; задания 6–7 ЛР4 и ЛР5 запускаются отдельно.

| Работа | Код | Материалы |
| --- | --- | --- |
| ЛР1 | [CSharpLabs/Lab1](CSharpLabs/Lab1) | [Условие](labs/Lab1/lab1_csharp.pdf) · [Отчёт](labs/Lab1/Отчёт_ЛР1.md) |
| ЛР2, вариант 3 | [CSharpLabs/Lab2](CSharpLabs/Lab2) | [Условие](labs/Lab2/lab2_charp.pdf) · [Отчёт](labs/Lab2/Отчёт_ЛР2.md) |
| ЛР3, вариант 3 | [CSharpLabs/Lab3](CSharpLabs/Lab3) | [Условие](labs/Lab3/lab3_csharp.pdf) · [Отчёт](labs/Lab3/Отчёт_ЛР3.md) |
| ЛР4, вариант 3 | [Задания 1–5](CSharpLabs/Lab4) · [Задания 6–7](Lab4.Points) | [Условие](labs/Lab4/lab4_csharp.pdf) · [Отчёт](labs/Lab4/Отчёт_ЛР4.md) |
| ЛР5, вариант 3 | [Lab5.ExcelDataReader](Lab5.ExcelDataReader) | [Условие](labs/Lab5/lab5_csharp.pdf) · [Отчёт](labs/Lab5/Отчёт_ЛР5.md) · [Данные XLS](labs/Lab5/LR5-var3.xls) |

## Запуск

Из корня репозитория:

```bash
dotnet build CSharpLabs.sln
dotnet run --project CSharpLabs/CSharpLabs.csproj
```

Для заданий 6–7 ЛР4 и для ЛР5:

```bash
dotnet run --project Lab4.Points/Lab4.Points.csproj
dotnet run --project Lab5.ExcelDataReader/Lab5.ExcelDataReader.csproj
```

ЛР5 при запуске загружает прилагаемый XLS-файл. Другой файл можно выбрать через меню или указать при запуске:

```bash
dotnet run --project Lab5.ExcelDataReader/Lab5.ExcelDataReader.csproj -- /путь/к/файлу.xls
```

Для сборки решения нужен .NET 10 SDK. Для запуска ЛР5 нужна среда выполнения .NET 8.
