using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Html;
using iText.StyledXmlParser.Node;

namespace Webion.Pdf.Function.Workers;

public class CustomTagWorkerFactory : DefaultTagWorkerFactory 
{
    public override ITagWorker? GetCustomTagWorker(IElementNode tag, ProcessorContext context) 
    {
        return tag.Name() == TagConstants.HTML 
            ? new ZeroMarginHtmlTagWorker(tag, context) 
            : null;
    }
}