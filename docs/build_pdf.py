"""Build the Tindahan ni Aling Nena documentation PDF.

Pipeline:
  1. Render the HTML with headless Microsoft Edge (A4, background graphics on).
  2. Read the rendered PDF, locate every numbered heading, and write the
     physical page number into the table of contents.
  3. Re-render and repeat until the TOC numbers are stable.
  4. Stamp a footer with page numbers, add PDF metadata and bookmarks.
  5. Render a contact sheet for visual QA.

Run:  python docs/build_pdf.py
"""

from __future__ import annotations

import os
import re
import shutil
import subprocess
import sys
from pathlib import Path

import pymupdf
from PIL import Image

HERE = Path(__file__).resolve().parent
HTML = HERE / "TindahanNiAlingNena_Documentation.html"
PDF = HERE / "TindahanNiAlingNena_Documentation.pdf"
TMP = Path(os.environ.get("TEMP", str(HERE))) / "tindahan_doc_build"

EDGE_CANDIDATES = [
    Path(r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
    Path(r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"),
]


def find_edge() -> Path:
    for candidate in EDGE_CANDIDATES:
        if candidate.exists():
            return candidate
    found = shutil.which("msedge")
    if found:
        return Path(found)
    raise SystemExit("Microsoft Edge was not found; install it or adjust EDGE_CANDIDATES.")


def render(html_text: str, destination: Path, label: str) -> None:
    TMP.mkdir(parents=True, exist_ok=True)
    source = TMP / f"document_{label}.html"
    source.write_text(html_text, encoding="utf-8")
    profile = TMP / "edge_profile"
    command = [
        str(find_edge()),
        "--headless=new",
        "--disable-gpu",
        "--no-pdf-header-footer",
        "--run-all-compositor-stages-before-draw",
        "--virtual-time-budget=8000",
        f"--user-data-dir={profile}",
        f"--print-to-pdf={destination}",
        source.resolve().as_uri(),
    ]
    result = subprocess.run(command, capture_output=True, text=True, timeout=180)
    if not destination.exists() or destination.stat().st_size < 10_000:
        sys.stderr.write(result.stdout + "\n" + result.stderr)
        raise SystemExit(f"Edge did not produce a usable PDF at {destination}")
    print(f"  rendered {label}: {destination.stat().st_size / 1024:.0f} KiB")


def normalise(text: str) -> str:
    return re.sub(r"\s+", " ", text).strip()


def heading_entries(html_text: str) -> list[tuple[str, str]]:
    pattern = re.compile(
        r'<span class="toc-p" data-heading="([^"]+)">([^<]*)</span>'
    )
    entries = pattern.findall(html_text)
    if not entries:
        raise SystemExit("No TOC entries found; check the HTML template.")
    return entries


def page_texts(pdf_path: Path) -> list[str]:
    with pymupdf.open(pdf_path) as document:
        return [normalise(page.get_text("text")) for page in document]


def locate_headings(pdf_path: Path, headings: list[str]) -> tuple[dict[str, int], int]:
    pages = page_texts(pdf_path)
    try:
        toc_start = next(
            i for i, text in enumerate(pages) if "Table of Contents" in text
        )
    except StopIteration:
        raise SystemExit("Could not find the table of contents in the rendered PDF.")

    toc_candidates = [
        i
        for i in range(toc_start, min(toc_start + 4, len(pages)))
        if "Appendix D: References" in pages[i]
    ]
    toc_end = max(toc_candidates) if toc_candidates else toc_start
    content_start = toc_end + 1

    mapping: dict[str, int] = {}
    missing: list[str] = []
    for heading in headings:
        needle = normalise(heading)
        page_number = None
        for index in range(content_start, len(pages)):
            if needle in pages[index]:
                page_number = index + 1
                break
        if page_number is None:
            missing.append(heading)
        else:
            mapping[heading] = page_number

    if missing:
        raise SystemExit("Headings not found in the PDF: " + ", ".join(missing))

    return mapping, content_start


def inject_pages(html_text: str, mapping: dict[str, int]) -> str:
    for heading, page in mapping.items():
        pattern = re.compile(
            r'(<span class="toc-p" data-heading="'
            + re.escape(heading)
            + r'">)[^<]*(</span>)'
        )
        html_text, count = pattern.subn(
            lambda match, page=page: f"{match.group(1)}{page}{match.group(2)}",
            html_text,
        )
        if count != 1:
            raise SystemExit(f"Expected one TOC placeholder for {heading!r}, found {count}.")
    return html_text


def add_footer_and_metadata(source: Path, destination: Path, mapping: dict[str, int]) -> int:
    with pymupdf.open(source) as document:
        total = document.page_count
        for index, page in enumerate(document):
            if index == 0:
                continue
            width, height = page.rect.width, page.rect.height
            page.draw_line(
                pymupdf.Point(48, height - 40),
                pymupdf.Point(width - 48, height - 40),
                color=(0.82, 0.86, 0.83),
                width=0.6,
            )
            page.insert_text(
                (48, height - 27),
                "Tindahan ni Aling Nena - Technical Documentation",
                fontsize=7.4,
                fontname="helv",
                color=(0.42, 0.46, 0.43),
            )
            label = f"Page {index + 1} of {total}"
            label_width = pymupdf.get_text_length(label, fontname="helv", fontsize=7.4)
            page.insert_text(
                (width - 48 - label_width, height - 27),
                label,
                fontsize=7.4,
                fontname="helv",
                color=(0.42, 0.46, 0.43),
            )

        document.set_metadata(
            {
                "title": "Tindahan ni Aling Nena - Technical Documentation",
                "author": "Project Documentation",
                "subject": "System architecture, game design, and object-oriented analysis",
                "keywords": "C#, .NET 8, Windows Forms, OOP, architecture, documentation",
                "creator": "Headless Edge + PyMuPDF build pipeline",
                "producer": "PyMuPDF",
            }
        )

        bookmarks = [
            [1, "Cover", 1],
            [1, "Document Information", 2],
            [1, "Table of Contents", 3],
        ]
        for heading, page in mapping.items():
            level = 1 if re.match(r"^(\d+\.\s|Appendix\s)", heading) else 2
            bookmarks.append([level, heading, page])
        document.set_toc(bookmarks)

        document.save(destination, garbage=4, deflate=True)

    return total


def contact_sheet(pdf_path: Path, output: Path) -> None:
    columns = 4
    thumb_width = 330
    margin = 16
    with pymupdf.open(pdf_path) as document:
        thumbs: list[Image.Image] = []
        for page in document:
            pixmap = page.get_pixmap(dpi=72)
            image = Image.frombytes("RGB", (pixmap.width, pixmap.height), pixmap.samples)
            ratio = thumb_width / image.width
            thumbs.append(image.resize((thumb_width, int(image.height * ratio)), Image.LANCZOS))

    rows = (len(thumbs) + columns - 1) // columns
    cell_height = max(image.height for image in thumbs) + margin
    sheet = Image.new(
        "RGB",
        (columns * (thumb_width + margin) + margin, rows * cell_height + margin),
        (232, 235, 232),
    )
    for index, image in enumerate(thumbs):
        column = index % columns
        row = index // columns
        x = margin + column * (thumb_width + margin)
        y = margin + row * cell_height
        sheet.paste(image, (x, y))
    sheet.save(output)
    print(f"  contact sheet: {output} ({len(thumbs)} pages)")


def main() -> None:
    html_template = HTML.read_text(encoding="utf-8")
    entries = heading_entries(html_template)
    headings = [heading for heading, _ in entries]

    TMP.mkdir(parents=True, exist_ok=True)
    first_pass = TMP / "pass1.pdf"
    print("Rendering first pass ...")
    render(html_template, first_pass, "pass1")

    mapping, content_start = locate_headings(first_pass, headings)
    print(f"  content starts on PDF page {content_start + 1}; found {len(mapping)} headings")

    working_pdf = TMP / "pass_final.pdf"
    for attempt in range(1, 4):
        numbered_html = inject_pages(html_template, mapping)
        render(numbered_html, working_pdf, f"pass{attempt + 1}")
        new_mapping, _ = locate_headings(working_pdf, headings)
        if new_mapping == mapping:
            print(f"  TOC page numbers stable after pass {attempt + 1}")
            break
        changed = {
            key: (mapping.get(key), new_mapping.get(key))
            for key in new_mapping
            if mapping.get(key) != new_mapping.get(key)
        }
        print(f"  page numbers shifted, retrying: {changed}")
        mapping = new_mapping
    else:
        raise SystemExit("Page numbering did not converge after four passes.")

    total = add_footer_and_metadata(working_pdf, PDF, mapping)
    print(f"  wrote {PDF} ({PDF.stat().st_size / 1024:.0f} KiB, {total} pages)")

    preview = TMP / "contact_sheet.png"
    contact_sheet(PDF, preview)

    # Persist the numbered HTML for reference, without altering the template.
    (HERE / "TindahanNiAlingNena_Documentation_numbered.html").write_text(
        inject_pages(html_template, mapping), encoding="utf-8"
    )
    print("Done.")


if __name__ == "__main__":
    main()
