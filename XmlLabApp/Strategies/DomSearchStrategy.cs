using System.Xml;
using XmlLabApp.Models;

namespace XmlLabApp.Strategies;

public class DomSearchStrategy : IXmlSearchStrategy
{
    public string Name => "DOM API";

    public IReadOnlyList<Member> Search(string xmlPath, SearchQuery query)
    {
        var document = new XmlDocument();
        document.Load(xmlPath);

        return document.SelectNodes(BuildXPath(query))!
            .Cast<XmlElement>()
            .Select(ToMember)
            .Where(query.MatchesKeywords)
            .ToList();
    }

    private static string BuildXPath(SearchQuery query)
    {
        var conditions = query.AttributeValues.Select(c => $"@{c.Key}={ToXPathLiteral(c.Value)}").ToList();
        return conditions.Count == 0
            ? $"//{Member.ElementName}"
            : $"//{Member.ElementName}[{string.Join(" and ", conditions)}]";
    }

    // Значення може містити апостроф (напр. «комп'ютерних»), тому лапки добираються під нього.
    private static string ToXPathLiteral(string value)
    {
        if (!value.Contains('\'')) return $"'{value}'";
        if (!value.Contains('"')) return $"\"{value}\"";
        return "concat('" + value.Replace("'", "',\"'\",'") + "')";
    }

    private static Member ToMember(XmlElement element)
    {
        var member = new Member
        {
            Attributes = element.Attributes.Cast<XmlAttribute>().ToDictionary(a => a.Name, a => a.Value)
        };
        foreach (XmlElement e in element.GetElementsByTagName(Member.EventElementName))
            member.Events.Add(new ParliamentEvent(
                e.GetAttribute("type"), e.GetAttribute("startDate"), e.GetAttribute("endDate")));
        return member;
    }
}
