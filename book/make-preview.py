import markdown2
import pdfkit
from pathlib import Path

# Pega todos os .md
base_path = Path("./")
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
    r"d1.Apêndice – Minhas Zeyras.md",
    r"d2.Epílogo – Quando o Silêncio Volta.md",
    r"d3.Comentarios do autor.md",
    r"d4.Enquanto Ainda Lembro.md"
]

# Concatena todos os HTMLs
html_output = ""
for md_file in md_files:
    with open(md_file, "r", encoding="utf-8") as f:
        md = f.read()
        html = markdown2.markdown(md)
        html_output += html + '<div style="page-break-after: always;"></div>'



# Salva HTML temporário
with open("temp.html", "w", encoding="utf-8") as f:
    f.write(f"""
    <html>
    <head>
    <meta charset="utf-8">
    <style>
        body {{ font-family: Arial, sans-serif; padding: 2em; }}
        h1 {{ color: #333; margin-top: 40px; }}
        pre {{ background-color: #f4f4f4; padding: 1em; overflow-x: auto; }}
    </style>
    </head>
    <body>{html_output}</body>
    </html>
    """)

# Caminho para o wkhtmltopdf no Windows
path_wkhtmltopdf = r'C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe'
config = pdfkit.configuration(wkhtmltopdf=path_wkhtmltopdf)

# Converte HTML → PDF (usando a configuração)
pdfkit.from_file("temp.html", "Livro_Carta_de_Alma.pdf", configuration=config)
