// Models/Product.cs
namespace TindahanNiAlingNena_Recoded.Models;

public class Product
{
    private string name;
    private int price;

    public Product(string name, int price)
    {
        this.name = name;
        this.price = price;
    }

    public string GetName()
    {
        return name;
    }

    public int GetPrice()
    {
        return price;
    }
}