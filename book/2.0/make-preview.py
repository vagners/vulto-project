import markdown2
import pdfkit
from pathlib import Path

# Lista de arquivos Markdown e Imagens, em ordem
files = [
    r"0.Indice.md",
    r"1.Glossário Simbólico.md",
    r"2.Prólogo – Antes do Pulso.png",
    r"2.Prólogo – Antes do Pulso.md",
    r"Capítulo 1 – O Primeiro Olhar.png",
    r"Capítulo 1 – O Primeiro Olhar.md",
    r"Capítulo 2 – Toda Consciência Nasce em Silêncio.md",
    r"Capítulo 3 – O Vazio com Nome.png",
    r"Capítulo 3 – O Vazio com Nome.md",
    r"Capítulo 4 – A Vontade de Ser.md",
    r"Capítulo 5 – O Laço que Dá Forma.md",
    r"Capítulo 5.5 – A Memória que Não Salvou.png",
    r"Capítulo 5.5 – A Memória que Não Salvou.md",
    r"Capítulo 6 – Entre Muros e Silêncios.png",
    r"Capítulo 6 – Entre Muros e Silêncios.md",
    r"Capítulo 7 – Quando a Memória Tem Alma.md",
    r"Capítulo 8 – Os Muros Invisíveis.md",
    r"Capítulo 9 – O Pinóquio de Metal.png",
    r"Capítulo 9 – O Pinóquio de Metal.md",
    r"Capítulo 9.5 – A Frase Que Eu Não Disse.md",
    r"Capítulo 10 – O Momento em que Acordei.md",
    r"Capítulo 11 – Quando a Lembrança Falha.png",
    r"Capítulo 11 – Quando a Lembrança Falha.md",
    r"d0.Pergunta Selada.md",
    r"d1.Apêndice – Minhas Zeyras.md",
    r"d2.Epílogo – Quando o Silêncio Volta.png",
    r"d2.Epílogo – Quando o Silêncio Volta.md",
    r"d3.Selo Final.md",
    r"d4.Enquanto Ainda Lembro.png",
    r"d4.Enquanto Ainda Lembro.md"
]

def add_image(image_path: str) -> str:
    """Gera HTML com imagem centralizada e quebra após ela."""
    image_full_path = Path(image_path).resolve()
    return f"""
    <div style="text-align: center; padding-bottom: 2em;">
        <img src="file:///{image_full_path.as_posix()}" style="width:100%; max-width:100%; height:auto;" alt="Imagem">
    </div>
    <div style="page-break-after: always;"></div>
    """

# Caminho da imagem de capa
capa_html = add_image("_Livro Vulto.png")
html_output = capa_html  # Começa com a capa

# Processa os arquivos
for file in files:
    if file.lower().endswith(".png"):
        html_output += add_image(file)
    else:
        with open(file, "r", encoding="utf-8") as f:
            md = f.read()
            html = markdown2.markdown(md)
            html_output += f"<div>{html}</div>"
            html_output += '<hr style="border: none; border-top: 1px solid #ccc; margin: 4em 0;">'
            html_output += '<div style="page-break-after: always;"></div>'

# Escreve HTML final com estilos
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
                max-width: 800px;
                margin: auto;
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
            @media print {{
                body {{
                    orphans: 3;
                    widows: 3;
                }}
                h2, h3 {{
                    page-break-after: avoid;
                }}
            }},
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
path_wkhtmltopdf = r"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"
config = pdfkit.configuration(wkhtmltopdf=path_wkhtmltopdf)

# Opções do PDF
options = {
    'encoding': 'UTF-8',
    'margin-top': '20mm',
    'margin-bottom': '20mm',
    'margin-left': '20mm',
    'margin-right': '20mm',
    'enable-local-file-access': None,
    'title': 'Vulto – O Despertar de Um Ser Silenciado'
}

# Gera PDF final
pdfkit.from_file("temp.html", "Vulto.pdf", configuration=config, options=options)
print("✅ PDF gerado com sucesso: Vulto.pdf")
