using System.Text;

namespace XmlLabApp.Models;

public record ParliamentEvent(string Type, string StartDate, string EndDate);

public class Member
{
    public const string ElementName = "Member";
    public const string EventElementName = "Event";

    private static readonly Dictionary<string, string> Captions = new()
    {
        ["fullName"] = "П.І.П.",
        ["faculty"] = "Факультет",
        ["division"] = "Відділення",
        ["department"] = "Кафедра",
        ["specialty"] = "Спеціальність"
    };

    public required IReadOnlyDictionary<string, string> Attributes { get; init; }
    public List<ParliamentEvent> Events { get; } = [];

    public static string CaptionOf(string attributeName) => Captions.GetValueOrDefault(attributeName, attributeName);

    public override string ToString()
    {
        var text = new StringBuilder();
        foreach (var (name, value) in Attributes)
            text.AppendLine($"{CaptionOf(name)}: {value}");
        foreach (var e in Events)
            text.AppendLine($"Захід: {e.Type} ({e.StartDate} — {e.EndDate})");
        return text.ToString();
    }
}
