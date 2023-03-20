using iText.Html2pdf;

using var htmlSource = File.Open("in/input.html", FileMode.Open);
using var pdfDest = File.Open("out/output.pdf", FileMode.Create);

ConverterProperties converterProperties = new ConverterProperties();
HtmlConverter.ConvertToPdf(htmlSource, pdfDest, converterProperties);
