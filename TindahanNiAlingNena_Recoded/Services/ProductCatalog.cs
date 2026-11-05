using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.Services;

public class ProductCatalog
{
    private readonly Random _random = new();

    private readonly List<Product> _products = new()
    {
        new Product("Ligo Sardines", 25),
        new Product("Coke Sakto", 15),
        new Product("Instant Noodles", 12),
        new Product("Sachet Shampoo", 6),
        new Product("Candy", 1),
        new Product("Pandesal", 2)
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