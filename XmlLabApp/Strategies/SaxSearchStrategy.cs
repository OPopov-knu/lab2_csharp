using System.Xml;
using XmlLabApp.Models;

namespace XmlLabApp.Strategies;

// У .NET немає класичного SAX; його потоковим аналогом є XmlReader.
public class SaxSearchStrategy : IXmlSearchStrategy
{
    public string Name => "SAX API";

    public IReadOnlyList<Member> Search(string xmlPath, SearchQuery query)
    {
        var found = new List<Member>();
        Member? current = null;

        using var reader = XmlReader.Create(xmlPath);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == Member.ElementName)
            {
                bool isEmpty = reader.IsEmptyElement;
                var attributes = ReadAttributes(reader);
                current = query.AttributeValues.All(c => attributes.GetValueOrDefault(c.Key) == c.Value)
                    ? new Member { Attributes = attributes }
                    : null;
                if (isEmpty) Complete();
            }
            else if (reader.NodeType == XmlNodeType.Element && reader.Name == Member.EventElementName)
            {
                current?.Events.Add(new ParliamentEvent(
                    reader.GetAttribute("type") ?? "",
                    reader.GetAttribute("startDate") ?? "",
                    reader.GetAttribute("endDate") ?? ""));
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == Member.ElementName)
            {
                Complete();
            }
        }
        return found;

        void Complete()
        {
            if (current != null && query.MatchesKeywords(current)) found.Add(current);
            current = null;
        }
    }

    private static Dictionary<string, string> ReadAttributes(XmlReader reader)
    {
        var attributes = new Dictionary<string, string>();
        while (reader.MoveToNextAttribute())
            attributes[reader.Name] = reader.Value;
        reader.MoveToElement();
        return attributes;
    }
}
