using Google.Cloud.Functions.Framework;
using Google.Cloud.Functions.Hosting;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Webion.Pdf.Function;

[FunctionsStartup(typeof(Startup))]
public class Function : IHttpFunction
{
    public async Task HandleAsync(HttpContext context)
    {
        using var pdfDest = new MemoryStream();
        using var sw = new PdfWriter(pdfDest);
        sw.SetCloseStream(false);

        using var bodyReader = new StreamReader(context.Request.Body);
        var html = JsonConvert.DeserializeObject<string>(await bodyReader.ReadToEndAsync());

        ConverterProperties converterProperties = new ConverterProperties();
        HtmlConverter.ConvertToPdf(html, sw, converterProperties);
        
        context.Response.ContentType = "application/pdf"; 
        context.Response.StatusCode = StatusCodes.Status200OK;

        using var pdfStream = new MemoryStream(pdfDest.ToArray());
        pdfStream.Position = 0;
        using var reader = new StreamReader(pdfStream);
        
        await context.Response.BodyWriter.WriteAsync(pdfDest.GetBuffer());

        return;
    }
}
