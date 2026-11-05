using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.Services;

public class ProductCatalog
{
    private readonly Random _random = new();

    private readonly List<Product> _products = new()
    {
        new Product("Canned Sardines", 25),
        new Product("Canned Corned Beef", 38),
        new Product("Instant Noodles", 12),
        new Product("Soft Drink Bottle", 15),
        new Product("Bottled Water", 10),
        new Product("Instant Coffee Sachet", 5),
        new Product("Shampoo Sachet", 6),
        new Product("Laundry Soap Bar", 20),
        new Product("Toothpaste Sachet", 8),
        new Product("Cooking Oil Sachet", 18),
        new Product("Soy Sauce Sachet", 7),
        new Product("Vinegar Sachet", 7),
        new Product("Salt Pack", 5),
        new Product("Sugar Pack", 10),
        new Product("Egg", 9),
        new Product("Rice Cup", 14),
        new Product("Biscuit Pack", 6),
        new Product("Chips Pack", 12),
        new Product("Candy", 1),
        new Product("Chewing Gum", 2),
        new Product("Cigarette Stick", 8),
        new Product("Matchbox", 3),
        new Product("Candle", 5),
        new Product("Bath Soap", 22),

        // 60+ pesos
        new Product("1 Liter Soft Drink", 65),
        new Product("1 Kilo Rice", 60),
        new Product("Cooking Oil Bottle", 75),
        new Product("Laundry Powder Pack", 68),
        new Product("Dishwashing Liquid Bottle", 62),
        new Product("Canned Meat Loaf Large", 70),
        new Product("Coffee Jar", 95),
        new Product("Dozen Eggs", 110)
    };

    public Product GetRandomProduct()
    {
        int randomIndex = _random.Next(_products.Count);
        return _products[randomIndex];
    }

    public List<Product> GetRandomProducts(int count)
    {
        var randomProducts = new List<Product>();

        for (int i = 0; i < count; i++)
        {
            randomProducts.Add(GetRandomProduct());
        }

        return randomProducts;
    }
}