using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.UI;

public class OrderDisplay : Label
{
    private readonly Label changeTotalLabel = new();

    public OrderDisplay()
    {
        string receiptPath = Path.Combine("Assets", "Receipt.png");
        if (File.Exists(receiptPath))
        {
            BackgroundImage = Image.FromFile(receiptPath);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        changeTotalLabel.Dock = DockStyle.Bottom;
        changeTotalLabel.Height = 34;
        changeTotalLabel.Margin = Padding.Empty;
        changeTotalLabel.BackColor = Color.Transparent;
        changeTotalLabel.ForeColor = ForeColor;
        changeTotalLabel.Font = new Font(Font, FontStyle.Bold);
        changeTotalLabel.TextAlign = ContentAlignment.MiddleCenter;
        Controls.Add(changeTotalLabel);
    }

    public void ShowOrder(Order order, int amountPaid, int changeGiven)
    {
        List<string> lines = new()
        {
            "ORDER SUMMARY",
            string.Empty
        };

        foreach (Product product in order.GetProducts())
        {
            lines.Add($"{product.GetName()}     ₱{product.GetPrice()}");
        }

        lines.Add(string.Empty);
        lines.Add($"TOTAL                 ₱{order.GetTotalCost()}");
        lines.Add($"Customer paid      ₱{amountPaid}");
        Text = string.Join(Environment.NewLine, lines);
        changeTotalLabel.Text = $"CHANGE SELECTED     ₱{changeGiven}";
    }
}