// Models/Order.cs
namespace TindahanNiAlingNena_Recoded.Models;

public class Order
{
    private List<Product> products = new List<Product>();
    private int totalCost = 0;

    public void AddProduct(Product product)
    {
        products.Add(product);
        totalCost += product.GetPrice();
    }

    public List<Product> GetProducts()
    {
        return products;
    }

    public int GetProductCount()
    {
        return products.Count;
    }

    public int GetTotalCost()
    {
        return totalCost;
    }
}