namespace TindahanNiAlingNena_Recoded.Models.Money;

// Sealed == Inheritance Restricted
public sealed class Coin : Denomination
{

    // Private Constructor
    private Coin(int value, string imagePath)
    {
        Value = value;
        ImagePath = imagePath;
    }

    public static Dictionary<int, Coin> Denominations { get; } = new()
    {
        [1]  = new Coin(1,  Path.Combine("Assets", "Coins", "PHP_1.png")),
        [5]  = new Coin(5,  Path.Combine("Assets", "Coins", "PHP_5.png")),
        [10] = new Coin(10, Path.Combine("Assets", "Coins", "PHP_10.png"))
    };

}