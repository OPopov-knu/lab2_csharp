using System.Xml.Xsl;

namespace XmlLabApp.Services;

public static class XslHtmlTransformer
{
    public static MemoryStream Transform(string xmlPath, string xslPath)
    {
        var transform = new XslCompiledTransform();
        transform.Load(xslPath);

        var html = new MemoryStream();
        transform.Transform(xmlPath, null, html);
        html.Position = 0;
        return html;
    }
}
