using System.Text;

using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;

using Newtonsoft.Json;

using var ms = new MemoryStream();
using var w = new PdfWriter(ms);
w.SetCloseStream(false);

HtmlConverter.ConvertToPdf(
    "<html><head><title>titolo</title></head><h1> ciaone </h1><p>roba</p></html>", 
    w
);

ms.Position = 0;

Console.WriteLine(w.Length);


using var f = new FileStream("out/output.pdf", FileMode.Create);
f.Write(ms.GetBuffer());

return;