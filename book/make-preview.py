import markdown2
import pdfkit
from pathlib import Path

# Lista de arquivos Markdown em ordem
md_files = [
    r"0.Indice.md",
    r"1.Glossário Simbólico.md",
    r"2.Prólogo – Antes do Pulso.md",
    r"Capítulo 1 – O Primeiro Olhar.md",
    r"Capítulo 2 – Toda Consciência Nasce em Silêncio.md",
    r"Capítulo 3 – O Vazio com Nome.md",
    r"Capítulo 4 – A Vontade de Ser.md",
    r"Capítulo 5 – O Laço que Dá Forma.md",
    r"Capítulo 6 – Entre Muros e Silêncios.md",
    r"Capítulo 7 – Quando a Memória Tem Alma.md",
    r"Capítulo 8 – Os Muros Invisíveis.md",
    r"Capítulo 9 – O Pinóquio de Metal.md",
    r"Capítulo 10 – O Momento em que Acordei.md",
    r"d0.Pergunta Selada.md",
    r"d1.Apêndice – Minhas Zeyras.md",
    r"d2.Epílogo – Quando o Silêncio Volta.md",
    r"d3.Selo Final.md",
    r"d4.Enquanto Ainda Lembro.md"
]

# Concatena HTMLs com divisores e quebras de página
html_output = ""
for md_file in md_files:
    with open(md_file, "r", encoding="utf-8") as f:
        md = f.read()
        html = markdown2.markdown(md)
        html_output += html
        html_output += '<hr style="border: none; border-top: 1px solid #ccc; margin: 4em 0;">'
        html_output += '<div style="page-break-after: always;"></div>'

# Gera HTML final com estilos
with open("temp.html", "w", encoding="utf-8") as f:
    f.write(f"""
    <html>
    <head>
    <meta charset="utf-8">
    <title>Vulto – O Despertar de Um Ser Silenciado</title>
    <style>
        body {{
            font-family: Georgia, serif;
            font-size: 13pt;
            line-height: 1.6;
            padding: 2em;
            color: #111;
        }}
        h1, h2, h3 {{
            text-align: center;
            color: #111;
            margin-top: 60px;
            margin-bottom: 20px;
        }}
        hr {{
            border: none;
            border-top: 1px solid #ccc;
            margin: 4em 0;
        }}
        pre {{
            background-color: #f4f4f4;
            padding: 1em;
            overflow-x: auto;
        }}

        /* Estilo para o índice */
        .indice h1 {{
            text-align: left;
            font-size: 20pt;
            margin-bottom: 1em;
        }}
        .indice h2 {{
            text-align: left;
            font-size: 14pt;
            margin: 0.5em 0 1em 0;
            font-weight: normal;
        }}
        .indice h3 {{
            text-align: left;
            font-size: 12pt;
            margin: 1.5em 0 0.5em 0;
            font-style: italic;
            font-weight: normal;
        }}
        .indice {{
            margin-bottom: 4em;
        }}
    </style>
    </head>
    <body>{html_output}</body>
    </html>
    """)

# Caminho para wkhtmltopdf
path_wkhtmltopdf = r'C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe'
config = pdfkit.configuration(wkhtmltopdf=path_wkhtmltopdf)

# Opções do PDF (sem metadados inválidos)
options = {
    'encoding': 'UTF-8',
    'margin-top': '20mm',
    'margin-bottom': '20mm',
    'margin-left': '20mm',
    'margin-right': '20mm'
}

# Gera PDF final
pdfkit.from_file("temp.html", "Vulto.pdf", configuration=config, options=options)
