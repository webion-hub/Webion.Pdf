using Google.Cloud.Functions.Framework;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Html2pdf.Html;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.StyledXmlParser.Node;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
        
        var base64 = System.Convert.ToBase64String(pdfDest.GetBuffer());
        context.Response.StatusCode = StatusCodes.Status200OK;
        
        await context.Response.WriteAsync(base64, context.RequestAborted);

        return;
    }
}

public class CustomTagWorkerFactory : DefaultTagWorkerFactory {
    public ITagWorker getCustomTagWorker(IElementNode tag, ProcessorContext context) {
        if (tag.Name() == TagConstants.HTML) {
            return new ZeroMarginHtmlTagWorker(tag, context);
        }
        return null!;
    }
}



public class ZeroMarginHtmlTagWorker : HtmlTagWorker {
    public ZeroMarginHtmlTagWorker(IElementNode element, ProcessorContext context) : base(element, context) {
        Document doc = (Document) GetElementResult();
        doc.SetMargins(0, 0, 0, 0);
    }
}