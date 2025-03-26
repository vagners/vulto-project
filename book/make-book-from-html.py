import pdfkit

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

pdfkit.from_file("Carta_de_Alma_FINAL.html", "Livro_Carta_de_Alma_Final.pdf", configuration=config, options=options)
