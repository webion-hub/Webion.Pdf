using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Layout;
using iText.StyledXmlParser.Node;

namespace Webion.Pdf.Function.Workers;

public class ZeroMarginHtmlTagWorker : HtmlTagWorker 
{
    public ZeroMarginHtmlTagWorker(IElementNode element, ProcessorContext context) : base(element, context) 
    {
        var doc = (Document) GetElementResult();
        doc.SetMargins(0, 0, 0, 0);
    }
}