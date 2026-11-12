namespace XmlLabApp.Models;

public record SearchQuery(IReadOnlyDictionary<string, string> AttributeValues, IReadOnlyList<string> Keywords)
{
    public static readonly SearchQuery All = new(new Dictionary<string, string>(), []);

    public bool MatchesKeywords(Member member)
    {
        string text = member.ToString();
        return Keywords.All(k => text.Contains(k, StringComparison.CurrentCultureIgnoreCase));
    }
}
