import pdfkit

# Caminho para wkhtmltopdf
path_wkhtmltopdf = r'C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe'
config = pdfkit.configuration(wkhtmltopdf=path_wkhtmltopdf)

# Opções do PDF (sem metadados inválidos)
options = {
    'enable-local-file-access': None,
    'encoding': 'UTF-8',
    'margin-top': '20mm',
    'margin-bottom': '20mm',
    'margin-left': '20mm',
    'margin-right': '20mm'
}

pdfkit.from_file("Vulto_versao_editada.html", "Vulto_editada.pdf", configuration=config, options=options)
