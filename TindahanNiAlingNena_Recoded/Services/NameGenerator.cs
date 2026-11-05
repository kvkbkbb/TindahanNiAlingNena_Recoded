using TindahanNiAlingNena_Recoded.Models.Customers;

namespace TindahanNiAlingNena_Recoded.Services;

public static class NameGenerator
{
    private static readonly Random random = new Random();

    private static readonly string[] maleTitles = { "Pareng", "Kuya", "Mang", "Tatay", "Tito" };
    private static readonly string[] femaleTitles = { "Mareng", "Ate", "Aleng", "Nanay", "Tita" };

    private static readonly string[] maleNames =
    {
        "Juan", "Jose", "Pedro", "Mario", "Ramon",
        "Eduardo", "Rodel", "Jun", "Totoy", "Berto"
    };

    private static readonly string[] femaleNames =
    {
        "Maria", "Rosa", "Luz", "Carmen", "Gloria",
        "Teresita", "Nena", "Lourdes", "Remedios", "Baby"
    };

    public static string GenerateName(Gender gender)
    {
        string title;
        string firstName;

        if (gender == Gender.Male)
        {
            title = maleTitles[random.Next(maleTitles.Length)];
            firstName = maleNames[random.Next(maleNames.Length)];
        }
        else
        {
            title = femaleTitles[random.Next(femaleTitles.Length)];
            firstName = femaleNames[random.Next(femaleNames.Length)];
        }

        return title + " " + firstName;
    }
}