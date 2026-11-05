using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.Services;

public class OrderGenerator
{
    private ProductCatalog catalog;
    private Random random = new Random();

    public OrderGenerator(ProductCatalog catalog)
    {
        this.catalog = catalog;
    }

    public Order GenerateOrder(int minProducts, int maxProducts)
    {
        int productCount = random.Next(minProducts, maxProducts + 1);
        Order order = new Order();

        for (int i = 0; i < productCount; i++)
        {
            Product randomProduct = catalog.GetRandomProduct();
            order.AddProduct(randomProduct);
        }

        return order;
    }
}