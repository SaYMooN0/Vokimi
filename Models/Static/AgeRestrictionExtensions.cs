using Vokimi.Models;

public static class AgeRestrictionExtensions
{
    public static string ToReadableString(this AgeRestriction ageRestriction)
    {
        switch (ageRestriction)
        {
            case AgeRestriction.AllAges:
                return "Not limited";
            case AgeRestriction.Ages12Plus:
                return "12+";
            case AgeRestriction.Ages16Plus:
                return "16+";
            case AgeRestriction.Ages18Plus:
                return "18+";
            case AgeRestriction.Ages21Plus:
                return "21+";
            default:
                throw new ArgumentOutOfRangeException(nameof(ageRestriction), ageRestriction, null);
        }
    }
}
