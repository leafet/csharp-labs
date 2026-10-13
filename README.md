# Лабораторные работы по C#

В репозитории один проект .NET 10 — [CSharpLabs.csproj](CSharpLabs/CSharpLabs.csproj) — и одна точка входа, [Program.Main](CSharpLabs/Program.cs). Главное меню позволяет выбрать лабораторную работу. Код каждой работы находится в своей папке внутри проекта, а задания, отчёты и снимки запусков — в соответствующей папке `labs`.

```text
CSharpLabs.sln
CSharpLabs/
  CSharpLabs.csproj
  Program.cs
  Lab1/
    Lab1.cs
  Lab2/
    Lab2.cs
    Task1/  # имена
    Task2/  # люди и родственные связи
    Task3/  # города
    Task4/  # конструкторы имён и людей
    Task5/  # кот
labs/
  Lab1/
    lab1_csharp.pdf
    Отчёт_ЛР1.md
    Отчёт_ЛР1.html
    screenshots/
  Lab2/
    lab2_charp.pdf
    Отчёт_ЛР2.md
```

| Работа | Код | Материалы |
| --- | --- | --- |
| № 1 | [CSharpLabs/Lab1](CSharpLabs/Lab1) | [Условие](labs/Lab1/lab1_csharp.pdf), [отчёт Markdown](labs/Lab1/Отчёт_ЛР1.md), [отчёт HTML](labs/Lab1/Отчёт_ЛР1.html) |
| № 2, вариант 3 | [CSharpLabs/Lab2](CSharpLabs/Lab2) | [Условие](labs/Lab2/lab2_charp.pdf), [отчёт](labs/Lab2/Отчёт_ЛР2.md) |

В ЛР2 папки `Task1`–`Task5` соответствуют пяти пунктам меню. Общие для нескольких заданий классы `PersonName` и `Person` находятся там, где впервые вводятся: в `Task1` и `Task2`. Задание `Task4` использует их конструкторы без дублирования классов.

## Запуск

Из корня репозитория:

```bash
dotnet build CSharpLabs.sln
dotnet run --project CSharpLabs/CSharpLabs.csproj
```

Нужен .NET 10 SDK. В главном меню введите `1` или `2`; `0` завершает программу. Для новой работы добавьте классы в `CSharpLabs/LabN`, материалы в `labs/LabN` и пункт выбора в `Program.Main`. Новый проект и новая точка входа не нужны.

Ввод в обеих работах проверяется. Автосвойства, LINQ и лямбда-операторы не используются.
