using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.Models.Money;

// Sealed == Inheritance Restricted
public sealed class Bill : Denomination
{
    
    // Private Constructor
    private Bill(int value, string imagePath)
    {
        Value = value;
        ImagePath = imagePath;
    }

    public override void PlaySound()
    {
        AudioPlayer.PlaySound("paper.wav");
    }

    public static Dictionary<int, Bill> Denominations { get; } = new()
    {
        [20]   = new Bill(20,   Path.Combine("Assets", "Bills", "PHP_20.jpg")),
        [50]   = new Bill(50,   Path.Combine("Assets", "Bills", "PHP_50.jpg")),
        [100]  = new Bill(100,  Path.Combine("Assets", "Bills", "PHP_100.jpg")),
        [200]  = new Bill(200,  Path.Combine("Assets", "Bills", "PHP_200.jpg")),
        [500]  = new Bill(500,  Path.Combine("Assets", "Bills", "PHP_500.jpg")),
        [1000] = new Bill(1000, Path.Combine("Assets", "Bills", "PHP_1000.jpg"))
    };
}