"""Собирает основной отчёт Отчёт_ЛР1.html: картинки вшиты base64, файл один.

Такой HTML открывается в любом браузере (в том числе на Linux) без папки со
скриншотами и печатается в PDF через Ctrl+P.

    python tools/make_report_standalone.py          # только HTML (основной отчёт)
    python tools/make_report_standalone.py --md     + Отчёт_ЛР1_standalone.md
"""
import base64
import html
import io
import os
import re
import sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

SRC = os.path.join(ROOT, "Отчёт_ЛР1.md")
OUT_HTML = os.path.join(ROOT, "Отчёт_ЛР1.html")
OUT_MD = os.path.join(ROOT, "Отчёт_ЛР1_standalone.md")


def prepare_embedded(path):
    """Встраивает исходный снимок без изменения изображения."""
    src = os.path.join(ROOT, path)
    with open(src, "rb") as image_file:
        data = base64.b64encode(image_file.read()).decode("ascii")
    return f"data:image/png;base64,{data}"


def image_data(md_path):
    cache = {}

    def resolve(rel):
        if rel not in cache:
            cache[rel] = prepare_embedded(rel)
        return cache[rel]

    return resolve


def highlight_line(line):
    """Безопасно выводит строку кода в HTML."""
    return html.escape(line) or "&nbsp;"


def to_html(lines, data_uri):
    out = []
    i = 0
    while i < len(lines):
        line = lines[i]

        if line.startswith("```"):
            block = []
            i += 1
            while i < len(lines) and not lines[i].startswith("```"):
                block.append(lines[i])
                i += 1
            i += 1
            out.append('<pre class="code">' + "\n".join(highlight_line(b) for b in block) + "</pre>")
            continue

        m = re.match(r"^!\[([^\]]*)\]\(([^)]+)\)$", line.strip())
        if m:
            out.append(
                f'<figure><img src="{data_uri(m.group(2))}" alt="{html.escape(m.group(1))}">'
                f"<figcaption>{html.escape(m.group(1))}</figcaption></figure>"
            )
            i += 1
            continue

        if line.startswith("### "):
            out.append(f"<h3>{inline_html(line[4:])}</h3>")
        elif line.startswith("## "):
            out.append(f"<h2>{inline_html(line[3:])}</h2>")
        elif line.startswith("# "):
            out.append(f"<h1>{inline_html(line[2:])}</h1>")
        elif line.strip() == "---":
            out.append("<hr>")
        elif line.strip() == "":
            pass
        else:
            out.append(f"<p>{inline_html(line.strip())}</p>")
        i += 1

    return "\n".join(out)


def inline_html(text):
    escaped = html.escape(text)
    escaped = re.sub(r"\*\*(.+?)\*\*", r"<strong>\1</strong>", escaped)
    escaped = re.sub(r"`([^`]+)`", r"<code>\1</code>", escaped)
    return escaped


CSS = """
body { font-family: 'Times New Roman', serif; font-size: 13pt; max-width: 900px;
       margin: 2rem auto; padding: 0 1rem; line-height: 1.45; color: #111; }
h1 { font-size: 18pt; margin-top: 1.5rem; }
h2 { font-size: 15pt; margin-top: 1.4rem; }
h3 { font-size: 13.5pt; margin-top: 1.1rem; }
pre.code { font-family: Consolas, monospace; font-size: 10pt; background: #f4f6f8;
           border: 1px solid #d6dce3; border-left: 4px solid #3b6e8f;
           padding: .6rem .8rem; overflow-x: auto; white-space: pre; line-height: 1.35; }
code { font-family: Consolas, monospace; font-size: 11pt; background: #f2f2f2; padding: 0 .2rem; }
figure { margin: .8rem 0; text-align: center; page-break-inside: avoid; }
figure img { max-width: 100%; height: auto; border: 1px solid #ddd; }
figcaption { font-size: 10pt; color: #555; font-style: italic; margin-top: .25rem; }
hr { border: 0; border-top: 1px solid #bbb; margin: 1.6rem 0; }
@media print { body { max-width: none; } }
"""


def main():
    with_md = "--md" in sys.argv

    lines = io.open(SRC, encoding="utf-8").read().split("\n")
    resolve = image_data(SRC)

    body = to_html(lines, resolve)
    html_doc = (
        "<!DOCTYPE html>\n<html lang=\"ru\">\n<head>\n<meta charset=\"utf-8\">\n"
        "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n"
        "<title>Лабораторная работа №1</title>\n<style>" + CSS + "</style>\n</head>\n<body>\n"
        + body + "\n</body>\n</html>\n"
    )
    io.open(OUT_HTML, "w", encoding="utf-8").write(html_doc)
    print(f"{os.path.basename(OUT_HTML)}: "
          f"{os.path.getsize(OUT_HTML) / 1024 / 1024:.2f} МБ - основной отчёт")

    if with_md:
        md_out = []
        for line in lines:
            m = re.match(r"^!\[([^\]]*)\]\(([^)]+)\)$", line.strip())
            if m:
                md_out.append(f"![{m.group(1)}]({resolve(m.group(2))})")
            else:
                md_out.append(line)
        io.open(OUT_MD, "w", encoding="utf-8").write("\n".join(md_out))
        print(f"{os.path.basename(OUT_MD)}: {os.path.getsize(OUT_MD) / 1024 / 1024:.2f} МБ")

    print("картинок вшито:", sum(1 for line in lines if re.match(r"^!\[[^\]]*\]\([^)]+\)$", line.strip())))


if __name__ == "__main__":
    main()
