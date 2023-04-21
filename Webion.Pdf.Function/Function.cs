using Google.Cloud.Functions.Framework;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Webion.Pdf.Function.Workers;

namespace Webion.Pdf.Function;

public class Function : IHttpFunction
{
    public async Task HandleAsync(HttpContext context)
    {
        using var pdfDest = new MemoryStream();
        using var writer = new PdfWriter(pdfDest);
        writer.SetCloseStream(false);

        using var bodyReader = new StreamReader(context.Request.Body);
        var html = JsonConvert.DeserializeObject<string>(await bodyReader.ReadToEndAsync());

        var converterProperties = new ConverterProperties();
        converterProperties.SetTagWorkerFactory(new CustomTagWorkerFactory());
        HtmlConverter.ConvertToPdf(html, writer, converterProperties);

        using var pdfStream = new MemoryStream(pdfDest.ToArray());
        using var reader = new StreamReader(pdfStream);
        
        var base64 = Convert.ToBase64String(pdfDest.GetBuffer());
        context.Response.StatusCode = StatusCodes.Status200OK;
        
        await context.Response.WriteAsync(base64, context.RequestAborted);
    }
}