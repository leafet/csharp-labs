"""Run checks, capture their logs with Chromium, and build the Lab 2 reports."""

import base64
import html
import os
from pathlib import Path
import re
import shutil
import subprocess
import tempfile
import textwrap


ROOT = Path(__file__).resolve().parents[3]
LAB = ROOT / "labs/Lab2"
SHOTS = LAB / "screenshots/live"
LOGS = LAB / "screenshots/logs"
PROJECT = ROOT / "CSharpLabs/CSharpLabs.csproj"
CHROME = os.environ.get("REPORT_CHROMIUM") or shutil.which("chromium")

CHECKS = r'''
using System;
using System.IO;
using CSharpLabs;

internal static class ReportChecks
{
    private static int _checks;

    private static void Check(bool condition, string description)
    {
        if (!condition)
        {
            throw new Exception("FAIL: " + description);
        }

        _checks++;
        Console.WriteLine("PASS: " + description);
    }

    public static void Main(string[] args)
    {
        switch (args[0])
        {
            case "names":
                PersonFullName shortName = new PersonFullName("  Анна  ");
                PersonFullName fullName = new PersonFullName("Анна", "Иванова", "Ивановна");
                PersonFullName optionalName = new PersonFullName("Анна", "  ", null);
                Console.WriteLine(shortName);
                Console.WriteLine(fullName);
                Console.WriteLine(optionalName);
                Check(shortName.ToString() == "Анна", "Пробелы по краям имени удалены");
                Check(fullName.ToString() == "Иванова Анна Ивановна", "Фамилия, имя, отчество выведены в нужном порядке");
                Check(optionalName.ToString() == "Анна", "Необязательные части не дают лишних пробелов");
                bool emptyRejected = false;
                try { new PersonFullName("   "); }
                catch (ArgumentException) { emptyRejected = true; }
                Check(emptyRejected, "Пустое обязательное имя отклонено");
                break;

            case "people":
                Person father = new Person(new PersonFullName("Иван", "Чудов"), 175);
                Person son = new Person("Пётр", 180, father);
                Person independent = new Person(new PersonFullName("Борис", "Иванов", "Сергеевич"), 170, father);
                Console.WriteLine(father);
                Console.WriteLine(son);
                Console.WriteLine(independent);
                Check(son.ToString() == "Чудов Пётр Иванович, рост: 180", "Фамилия и отчество дополнены от непосредственного отца");
                Check(independent.ToString() == "Иванов Борис Сергеевич, рост: 170", "Заданные фамилия и отчество сохранены");
                Check(father.Father is null, "Отсутствие отца допустимо");
                bool selfRejected = false;
                try { son.Father = son; }
                catch (ArgumentException) { selfRejected = true; }
                Check(selfRejected && ReferenceEquals(son.Father, father), "Назначение себя отцом отклонено, прежняя ссылка сохранена");
                bool heightRejected = false;
                try { son.Height = 0; }
                catch (ArgumentOutOfRangeException) { heightRejected = true; }
                Check(heightRejected && son.Height == 180, "Нулевой рост отклонён, прежнее значение сохранено");
                break;

            case "cities":
                City a = new City("A");
                City b = new City("B");
                a.AddRoute(b, 5);
                Console.WriteLine(a);
                b.Name = "C";
                Console.WriteLine(a);
                Check(a.ToString() == "A [C:5]", "Путь хранит ссылку на город: переименование видно в выводе");
                Check(b.ToString() == "C []", "Город без исходящих путей выводится корректно");
                bool selfRouteRejected = false;
                try { a.AddRoute(a, 1); }
                catch (ArgumentException) { selfRouteRejected = true; }
                Check(selfRouteRejected, "Путь в тот же город отклонён");
                bool duplicateRejected = false;
                try { a.AddRoute(b, 2); }
                catch (ArgumentException) { duplicateRejected = true; }
                Check(duplicateRejected, "Повторный путь отклонён");
                bool costRejected = false;
                try { a.AddRoute(new City("D"), 0); }
                catch (ArgumentOutOfRangeException) { costRejected = true; }
                Check(costRejected && a.Routes.Count == 1, "Нулевая стоимость отклонена, список путей не изменился");
                break;

            case "constructors":
                Person lev = new Person("Лев", 170);
                PersonFullName sergeyName = new PersonFullName("Сергей", "Пушкин");
                Person sergey = new Person(sergeyName, 168, lev);
                Person alexander = new Person("Александр", 167, sergey);
                Person noFather = new Person(new PersonFullName("Клеопатра"), 152);
                Console.WriteLine(lev);
                Console.WriteLine(sergey);
                Console.WriteLine(alexander);
                Console.WriteLine(noFather);
                Check(lev.FullName.Name == "Лев" && lev.Father is null, "Конструктор: строковое имя и рост");
                Check(ReferenceEquals(sergey.FullName, sergeyName) && ReferenceEquals(sergey.Father, lev), "Конструктор: объект ФИО, рост и отец сохраняет ссылки");
                Check(alexander.ToString() == "Пушкин Александр Сергеевич, рост: 167", "Конструктор: строковое имя, рост и отец");
                Check(noFather.ToString() == "Клеопатра, рост: 152", "Конструктор: объект ФИО и рост");
                break;

            case "cat":
                Cat cat = new Cat("Мурзик");
                Console.WriteLine(cat);
                TextWriter original = Console.Out;
                StringWriter capture = new StringWriter();
                try
                {
                    Console.SetOut(capture);
                    cat.Meow();
                    cat.Meow(3);
                }
                finally
                {
                    Console.SetOut(original);
                }
                string output = capture.ToString();
                Console.Write(output);
                Check(output == "Мурзик: мяу!" + Environment.NewLine + "Мурзик: мяу-мяу-мяу!" + Environment.NewLine, "Перегрузки выводят одно и три мяуканья");
                bool zeroRejected = false;
                try { cat.Meow(0); }
                catch (ArgumentOutOfRangeException) { zeroRejected = true; }
                Check(zeroRejected, "Нулевое число мяуканий отклонено");
                bool limitRejected = false;
                try { cat.Meow(101); }
                catch (ArgumentOutOfRangeException) { limitRejected = true; }
                Check(limitRejected, "Число мяуканий выше ограничения 100 отклонено");
                break;
            default:
                throw new ArgumentException("Неизвестная группа проверок.");
        }

        Console.WriteLine($"Успешно: {_checks}; ошибок: 0.");
    }
}
'''


def run(command, *, input_text=None, cwd=ROOT):
    result = subprocess.run(command, cwd=cwd, input=input_text, text=True,
                            capture_output=True, timeout=90)
    if result.returncode:
        raise RuntimeError(result.stdout + result.stderr)
    return result.stdout + result.stderr


def capture(browser, temp, key, title, output, input_text=None):
    """Screenshot an HTML view of unmodified output; preserve the raw log too."""
    (LOGS / f"{key}.txt").write_text(output, encoding="utf-8")
    metadata = "Ввод: " + repr(input_text) if input_text is not None else "Проверка выполнена без интерактивного ввода"
    lines = sum(max(1, (len(line) + 95) // 96) for line in output.splitlines())
    height = max(360, 155 + lines * 23)
    page = temp / f"{key}.html"
    page.write_text('''<!doctype html><html lang="ru"><meta charset="utf-8">
<style>body{margin:0;padding:28px;background:#111827;color:#e5e7eb;font:16px monospace}
h1{font:600 21px sans-serif;margin:0 0 12px}p{color:#a5b4fc;font:14px monospace;margin:0 0 22px}
pre{font:16px/23px monospace;white-space:pre-wrap;overflow-wrap:anywhere;margin:0}
</style><h1>''' + html.escape(title) + '</h1><p>Журнал фактического запуска · ' +
                    html.escape(metadata) + '</p><pre>' + html.escape(output) + '</pre>', encoding="utf-8")
    command = [browser, "--headless", "--no-sandbox", "--disable-gpu", "--disable-dev-shm-usage",
               "--no-first-run", "--no-default-browser-check", "--hide-scrollbars",
               f"--user-data-dir={temp / 'chrome-profile'}", f"--window-size=1120,{height}",
               f"--screenshot={SHOTS / (key + '.png')}", page.as_uri()]
    result = subprocess.run(command, capture_output=True, text=True, timeout=45)
    if result.returncode or not (SHOTS / f"{key}.png").exists():
        raise RuntimeError(result.stderr)
    print(f"Снимок: {key}", flush=True)


def source(relative):
    return (ROOT / "CSharpLabs/Lab2" / relative).read_text(encoding="utf-8")


def method(relative, signature):
    code = source(relative)
    start = code.index(signature)
    start = code.rfind("\n", 0, start) + 1
    opening = code.index("{", start)
    depth = 1
    end = opening + 1
    while depth:
        depth += (code[end] == "{") - (code[end] == "}")
        end += 1
    return textwrap.dedent(code[start:end])


def write_reports():
    md, body = [], []

    def heading(level, text):
        md.append("#" * level + " " + text)
        body.append(f"<h{level}>{html.escape(text)}</h{level}>")

    def paragraph(text):
        md.append(text)
        body.append("<p>" + html.escape(text) + "</p>")

    def code(text):
        md.append("```csharp\n" + text.strip() + "\n```")
        body.append('<pre class="code">' + html.escape(text.strip()) + "</pre>")

    def shot(key, caption):
        md.append(f"![{caption}](screenshots/live/{key}.png)")
        data = base64.b64encode((SHOTS / f"{key}.png").read_bytes()).decode()
        body.append(f'<figure><img alt="{html.escape(caption)}" src="data:image/png;base64,{data}">'
                    f'<figcaption>{html.escape(caption)}</figcaption></figure>')

    def task(title, condition, algorithm, snippets, images):
        heading(2, title)
        heading(3, "Текст задачи")
        paragraph(condition)
        heading(3, "Алгоритм решения")
        paragraph(algorithm)
        for snippet in snippets:
            code(snippet)
        heading(3, "Тестирование")
        for key, caption in images:
            shot(key, caption)

    heading(1, "Братилов Захар, группа ИТ-1,2. Лабораторная работа №2")
    paragraph("Вариант 3. Выполнены задачи 1.3, 2.2, 2.3, 3.3, 4.5, 4.6 и 5.2. "
              "Номера 1–5 в меню объединяют задачи по разделам. Решение использует один проект .NET 10 и одну точку входа Program.Main.")
    paragraph("Класс, представляющий ФИО, назван PersonFullName. Личное имя обязательно; фамилия и отчество могут отсутствовать. "
              "Общие классы повторно используются в разных заданиях. Части Lab2 разнесены по папкам при помощи partial. "
              "Автосвойства, LINQ и лямбда-операторы не используются.")
    paragraph("Снимки показывают журналы реальных запусков программы и дополнительных проверок. Вывод сохранён без редактирования. "
              "Дополнительные проверки выполняются во временном проекте, который использует классы основного приложения. "
              "Примеры из меню запускаются с перенаправленным вводом, поэтому очистка экрана и ожидание Enter отключены.")

    heading(1, "Задание 1. Создание сущностей")
    task("Задача 3. Имена",
         "Создать сущность Имя с фамилией, личным именем и отчеством. Строковое представление должно содержать заданные части "
         "в порядке «Фамилия Имя Отчество», без лишних пробелов. Создать имена: Клеопатра; Пушкин Александр Сергеевич; Маяковский Владимир.",
         "PersonFullName хранит три части ФИО в отдельных полях с явно реализованными свойствами. OptionalText удаляет пробелы "
         "по краям и заменяет пустую строку на null; RequireText отклоняет пустое личное имя. ToString добавляет только заданные части.",
         [method("Task1_Names/PersonFullName.cs", "public override string ToString"),
          method("Task1_Names/PersonFullName.cs", "private static string RequireText"),
          method("Task1_Names/PersonFullName.cs", "private static string? OptionalText")],
         [("task1", "Пример из условия: три варианта ФИО"), ("check_names", "Дополнительные проверки: пробелы, отсутствующие части и пустое имя")])

    heading(1, "Задание 2. Взаимодействие объектов")
    task("Задача 2. Человек с именем",
         "Объединить сущности Человек и Имя так, чтобы имя человека задавалось объектом. Создать Клеопатру ростом 152, "
         "Пушкина Александра Сергеевича ростом 167 и Маяковского Владимира ростом 189.",
         "Person хранит ссылку на PersonFullName в свойстве FullName и положительный целочисленный рост в Height. "
         "При выводе вызывается ToString объекта ФИО. Конструктор отклоняет null вместо объекта имени, а свойство Height — неположительный рост.",
         [method("Task2_People/Person.cs", "public Person(PersonFullName fullName, int height, Person? father)"),
          method("Task2_People/Person.cs", "public override string ToString")],
         [("task2", "Примеры из условий задач 2.2 и 2.3"), ("check_people", "Дополнительные проверки человека и родственных связей")])
    task("Задача 3. Человек с родителем",
         "Добавить отца, который также является объектом Человек. При приведении к строке дополнить отсутствующую фамилию "
         "фамилией отца, а отчество — на основании его имени. Создать Чудова Ивана, Чудова Петра и Бориса; назначить Ивана отцом Петра, "
         "а Петра отцом Бориса; вывести всех троих.",
         "Свойство Father хранит ссылку на непосредственного отца. CompleteNameFromFather вызывается при приведении человека к строке. "
         "Если отец отсутствует, метод завершается. Заполняются только отсутствующие фамилия и отчество. "
         "Сеттер Father запрещает назначить человека собственным отцом. Цепочка предков не обходится. "
         "Для примеров отдельно обработаны Лев, Пётр и имена на «й»; для остальных добавляется «ович». Это не полный набор правил русского языка. "
         "Рост этих трёх людей в условии не задан, поэтому выбраны 175, 180 и 170.",
         [method("Task2_People/Person.cs", "private void CompleteNameFromFather"),
          method("Task2_People/Person.cs", "private static string MakePatronymic")],
         [("task2", "Иван, Пётр и Борис: формирование фамилии и отчества"),
          ("check_people", "Сохранение заданного ФИО, отсутствие отца и запрет ссылки на себя")])

    heading(1, "Задание 3. Наборы объектов")
    task("Задача 3. Города",
         "Создать сущность Город с названием и набором путей. Каждый путь содержит другой город и стоимость поездки. "
         "Строковая форма должна показывать название города и пары «название:стоимость». Построить схему с рисунка 2 исходного задания.",
         "City хранит название и список Route. Каждый Route содержит ссылку на город назначения и положительную стоимость. "
         "AddRoute проверяет отсутствие null, пути в себя и повторного назначения. Для двусторонних связей добавляются два пути. "
         "Схема содержит шесть городов и двенадцать направленных путей: A→B:5, A→F:1, A→D:6, B→A:5, B→C:3, "
         "C→B:3, C→D:4, D→A:6, D→C:4, D→E:2, F→B:1, F→E:2. У города E исходящих путей нет.",
         [method("Task3_Cities/City.cs", "public void AddRoute"), method("Task3_Cities/CitiesTask.cs", "private static City[] CreateCities")],
         [("task3", "Схема из условия: шесть городов и двенадцать направлений"),
          ("check_cities", "Переименование связанного города и отклонение недопустимых путей")])

    heading(1, "Задание 4. Конструкторы")
    name_source = source("Task1_Names/PersonFullName.cs")
    name_constructors = textwrap.dedent(name_source[name_source.index("    public PersonFullName("):name_source.index("    public override")])
    person_source = source("Task2_People/Person.cs")
    person_constructors = textwrap.dedent(person_source[person_source.index("    public Person(string"):person_source.index("    public override")])
    task("Задача 5. Создаем имена",
         "Предусмотреть создание имени по личному имени, по личному имени и фамилии, либо по всем трём частям. "
         "Создать имена Клеопатры, Александра Сергеевича Пушкина, Владимира Маяковского и Христофора Бонифатьевича. "
         "У Христофора слово «Бонифатьевич» является фамилией.",
         "Три перегруженных конструктора PersonFullName делегируют инициализацию конструктору с тремя параметрами. "
         "Пропущенные части передаются как null. Порядок строкового представления сохраняется: фамилия, личное имя, отчество; "
         "поэтому последний пример выводится как «Бонифатьевич Христофор».",
         [name_constructors],
         [("task4", "Все варианты конструкторов из заданий 4.5 и 4.6"),
          ("check_names", "Проверка полного и сокращённого ФИО")])
    task("Задача 6. Создаем человека",
         "Предусмотреть четыре способа создания человека: строковое имя и рост; строковое имя, рост и отец; объект имени и рост; "
         "объект имени, рост и отец. Фамилия и отчество должны дополняться по отцу, если не заданы. Присвоения сосредоточить в одном "
         "конструкторе. Создать Льва ростом 170, Сергея Пушкина ростом 168 с отцом Львом и Александра ростом 167 с отцом Сергеем.",
         "Строковое имя преобразуется в PersonFullName, после чего все перегрузки вызывают основной конструктор. Он сохраняет ФИО, "
         "рост и ссылку на отца. Объект ФИО сохраняется по ссылке. При вызове ToString недостающие части имени дополняются от отца; "
         "для Льва получается «Львович», для Сергея — «Сергеевич». Общий формат вывода Person: «ФИО, рост: значение».",
         [person_constructors],
         [("task4", "Лев, Сергей Пушкин и Александр"), ("check_constructors", "Проверены все четыре перегрузки и сохранение ссылок")])

    heading(1, "Задание 5. Методы объектов")
    task("Задача 2. Кот мяукает",
         "Создать кота с обязательным именем. Строковая форма: «кот: Имя». Метод без параметров выводит «Имя: мяу!», "
         "метод с параметром N — N мяуканий через дефис. Создать Барсика и вызвать одно и три мяуканья.",
         "Cat проверяет имя при создании. Meow() вызывает Meow(1). Перегрузка с числом повторений строит строку через StringBuilder, "
         "добавляя дефис перед каждым повторением, кроме первого. В реализации принято ограничение от 1 до 100 мяуканий; "
         "недопустимое значение вызывает ArgumentOutOfRangeException.",
         [method("Task5_Cat/Cat.cs", "public void Meow()"), method("Task5_Cat/Cat.cs", "public void Meow(int count)")],
         [("task5", "Барсик мяукает один и три раза"), ("check_cat", "Дополнительный пример и проверка границ количества мяуканий")])

    heading(1, "Проверка интерфейса и сборки")
    paragraph("В условии проверка ввода требуется, если данные вводятся с клавиатуры. Объекты из примеров создаются в коде. "
              "Пользователь вводит номера меню, для которых проверяются формат целого числа и допустимый диапазон. "
              "Завершение потока ввода обрабатывается в Program.Main. В интерактивном терминале результат остаётся до Enter; "
              "при возвращении в меню курсор перемещается в начало экрана и экран очищается.")
    shot("menu_main", "Главное меню: буквы и номер вне диапазона отклонены")
    shot("menu_lab2", "Меню ЛР2: недопустимые номера отклонены")
    shot("eof", "Завершение потока ввода обработано без аварии")
    shot("build", "Сборка проекта с предупреждениями как ошибками")
    heading(1, "Вывод")
    paragraph("Реализованы семь задач варианта 3, объединённых в пять пунктов меню. Использованы составные объекты, ссылки между "
              "объектами, списки путей, перегруженные конструкторы и методы, явные свойства и ToString. "
              "Проверены примеры из условия, дополнительные случаи и отклонение некорректных значений. "
              "При сборке ошибок и предупреждений не обнаружено.")
    paragraph("Запуск: dotnet run --project CSharpLabs/CSharpLabs.csproj. В главном меню выбрать 2, затем номер задания.")
    md.append("Исходное задание: [lab2_charp.pdf](lab2_charp.pdf). [Журналы и воспроизведение проверок](screenshots/README.md).")
    body.append('<p>Исходное задание: lab2_charp.pdf. Журналы и инструкции воспроизведения находятся в репозитории: labs/Lab2/screenshots.</p>')
    (LAB / "Отчёт_ЛР2.md").write_text("\n\n".join(md) + "\n", encoding="utf-8")
    template = (ROOT / "labs/Lab1/Отчёт_ЛР1.html").read_text(encoding="utf-8")
    style = re.search(r"<style>(.*?)</style>", template, re.S).group(1)
    style += "\npre.code { white-space: pre-wrap; overflow-wrap: anywhere; }\nh1, h2, h3 { break-after: avoid; }\n"
    document = '<!DOCTYPE html>\n<html lang="ru"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1">'
    document += '<title>Лабораторная работа №2 — вариант 3</title><style>' + style + '</style></head><body>\n'
    document += "\n".join(body) + "\n</body></html>\n"
    (LAB / "Отчёт_ЛР2.html").write_text(document, encoding="utf-8")


def main():
    if not CHROME:
        raise RuntimeError("Установите Chromium или задайте REPORT_CHROMIUM.")
    SHOTS.mkdir(parents=True, exist_ok=True)
    LOGS.mkdir(parents=True, exist_ok=True)
    with tempfile.TemporaryDirectory(prefix="lab2-report-") as directory:
        temp = Path(directory)
        build = run(["dotnet", "build", "CSharpLabs.sln", "-warnaserror"])
        capture(CHROME, temp, "build", "Сборка: dotnet build CSharpLabs.sln -warnaserror", build)
        cases = [
            ("task1", "Задание 1.3 — имена", "2\n1\n0\n0\n", ["Пушкин Александр Сергеевич", "Маяковский Владимир"]),
            ("task2", "Задания 2.2 и 2.3 — люди", "2\n2\n0\n0\n", ["Чудов Борис Петрович, рост: 170"]),
            ("task3", "Задание 3.3 — города", "2\n3\n0\n0\n", ["A [B:5, F:1, D:6]", "B [A:5, C:3]", "E []"]),
            ("task4", "Задания 4.5 и 4.6 — конструкторы", "2\n4\n0\n0\n", ["Бонифатьевич Христофор", "Пушкин Александр Сергеевич, рост: 167"]),
            ("task5", "Задание 5.2 — кот", "2\n5\n0\n0\n", ["кот: Барсик", "Барсик: мяу-мяу-мяу!"]),
            ("menu_main", "Проверка главного меню", "abc\n3\n0\n", ["Введите целое число от 0 до 2."]),
            ("menu_lab2", "Проверка меню ЛР2", "2\n6\n-1\n0\n0\n", ["Введите целое число от 0 до 5."]),
            ("eof", "Завершение потока ввода", "2\n", ["Ввод завершён."]),
        ]
        for key, title, input_text, expected in cases:
            output = run(["dotnet", "run", "--no-build", "--project", str(PROJECT)], input_text=input_text)
            for marker in expected:
                if marker not in output:
                    raise AssertionError(f"{key}: нет ожидаемого вывода {marker}")
            capture(CHROME, temp, key, title, output, input_text)
        harness = temp / "checks"
        harness.mkdir()
        (harness / "Checks.csproj").write_text('<Project Sdk="Microsoft.NET.Sdk"><PropertyGroup><OutputType>Exe</OutputType>'
            '<TargetFramework>net10.0</TargetFramework></PropertyGroup><ItemGroup><ProjectReference Include="' +
            html.escape(str(PROJECT), quote=True) + '" /></ItemGroup></Project>', encoding="utf-8")
        (harness / "Program.cs").write_text(textwrap.dedent(CHECKS), encoding="utf-8")
        run(["dotnet", "build", str(harness / "Checks.csproj"), "-warnaserror"])
        for group, count in [("names", 4), ("people", 5), ("cities", 5), ("constructors", 4), ("cat", 3)]:
            output = run(["dotnet", "run", "--no-build", "--project", str(harness / "Checks.csproj"), "--", group])
            if f"Успешно: {count}; ошибок: 0." not in output:
                raise AssertionError(output)
            capture(CHROME, temp, "check_" + group, "Дополнительные проверки: " + group, output)
    write_reports()
    print("Готовы 14 снимков, журналы, Markdown и самостоятельный HTML.")


if __name__ == "__main__":
    main()
