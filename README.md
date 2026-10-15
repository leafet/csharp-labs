# Лабораторные работы по C#

В репозитории один проект .NET 10 — [CSharpLabs.csproj](CSharpLabs/CSharpLabs.csproj) — и одна точка входа, [Program.Main](CSharpLabs/Program.cs). Главное меню позволяет выбрать лабораторную работу. Код каждой работы находится в своей папке внутри проекта, а задания, отчёты и снимки запусков — в соответствующей папке `labs`.

```text
CSharpLabs.sln
CSharpLabs/
  CSharpLabs.csproj
  Program.cs
  ConsoleInput.cs
  Lab1/
    Lab1.cs
  Lab2/
    Lab2.cs
    Task1_Names/         # имена
    Task2_People/        # люди и родственные связи
    Task3_Cities/        # города
    Task4_Constructors/  # конструкторы имён и людей
    Task5_Cat/           # кот
labs/
  Lab1/
    lab1_csharp.pdf
    Отчёт_ЛР1.md
    Отчёт_ЛР1.html
    screenshots/
  Lab2/
    lab2_charp.pdf
    Отчёт_ЛР2.md
    Отчёт_ЛР2.html
    screenshots/
```

| Работа | Код | Материалы |
| --- | --- | --- |
| № 1 | [CSharpLabs/Lab1](CSharpLabs/Lab1) | [Условие](labs/Lab1/lab1_csharp.pdf), [отчёт Markdown](labs/Lab1/Отчёт_ЛР1.md), [отчёт HTML](labs/Lab1/Отчёт_ЛР1.html) |
| № 2, вариант 3 | [CSharpLabs/Lab2](CSharpLabs/Lab2) | [Условие](labs/Lab2/lab2_charp.pdf), [отчёт Markdown](labs/Lab2/Отчёт_ЛР2.md), [отчёт HTML](labs/Lab2/Отчёт_ЛР2.html), [снимки проверок](labs/Lab2/screenshots/live/) |

В ЛР2 папки `Task1_Names`–`Task5_Cat` соответствуют пяти пунктам меню. Общие для нескольких заданий классы `PersonFullName` и `Person` находятся там, где впервые вводятся: в `Task1_Names` и `Task2_People`. Задание `Task4_Constructors` использует их конструкторы без дублирования классов. Статический класс `ConsoleInput` рядом с `Program` проверяет ввод для главного меню и ЛР2.

## Запуск

Из корня репозитория:

```bash
dotnet build CSharpLabs.sln
dotnet run --project CSharpLabs/CSharpLabs.csproj
```

Нужен .NET 10 SDK. В главном меню введите `1` или `2`; `0` завершает программу. Для новой работы добавьте классы в `CSharpLabs/LabN`, материалы в `labs/LabN` и пункт выбора в `Program.Main`. Новый проект и новая точка входа не нужны.

В меню ЛР2 пункты 1–5 выводят примеры из условия, созданные из объектов. В обычном терминале экран очищается при переходе между меню; результат задания остаётся на экране до нажатия Enter. При перенаправлении ввода или вывода очистка и пауза отключены. Ввод номеров меню проверяется. Автосвойства, LINQ и лямбда-операторы не используются.
