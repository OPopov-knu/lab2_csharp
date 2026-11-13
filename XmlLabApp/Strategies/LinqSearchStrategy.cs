using System.Xml.Linq;
using XmlLabApp.Models;

namespace XmlLabApp.Strategies;

public class LinqSearchStrategy : IXmlSearchStrategy
{
    public string Name => "LINQ to XML";

    public IReadOnlyList<Member> Search(string xmlPath, SearchQuery query)
    {
        var elements = XDocument.Load(xmlPath).Descendants(Member.ElementName);

        foreach (var (name, value) in query.AttributeValues)
            elements = elements.Where(e => (string?)e.Attribute(name) == value);

        return elements
            .Select(ToMember)
            .Where(query.MatchesKeywords)
            .ToList();
    }

    private static Member ToMember(XElement element)
    {
        var member = new Member
        {
            Attributes = element.Attributes().ToDictionary(a => a.Name.LocalName, a => a.Value)
        };
        member.Events.AddRange(element.Elements(Member.EventElementName).Select(e => new ParliamentEvent(
            (string?)e.Attribute("type") ?? "",
            (string?)e.Attribute("startDate") ?? "",
            (string?)e.Attribute("endDate") ?? "")));
        return member;
    }
}
