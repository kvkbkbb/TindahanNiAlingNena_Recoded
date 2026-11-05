// UI/OrderDisplay.cs
using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.UI;

public class OrderDisplay : Label
{
    public void ShowOrder(Order order, int amountPaid, int changeGiven)
    {
        string text = "Order:\n";

        foreach (Product product in order.GetProducts())
        {
            text += product.GetName() + " - P" + product.GetPrice() + "\n";
        }

        text += "Total: P" + order.GetTotalCost() + "\n";
        text += "Customer paid: P" + amountPaid + "\n";
        text += "Change you are giving: P" + changeGiven;

        Text = text;
    }
}