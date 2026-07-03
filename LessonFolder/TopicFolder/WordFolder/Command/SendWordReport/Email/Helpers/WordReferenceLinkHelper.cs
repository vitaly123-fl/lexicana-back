using lexicana.Common.Enums;

namespace lexicana.LessonFolder.TopicFolder.WordFolder.Command.SendWordReport.Email.Helpers;

public static class WordReferenceLinkHelper
{
    private static readonly Dictionary<Language, string> LanguageCodes = new()
    {
        { Language.Spanish, "es" },
        { Language.Italian, "it" },
        { Language.French, "fr" },
        { Language.German, "de" }
    };

    public static string BuildUrl(Language language, string word)
    {
        var code = LanguageCodes.TryGetValue(language, out var value) ? value : "en";
        return $"https://www.wordreference.com/{code}en/{Uri.EscapeDataString(word)}";
    }
}