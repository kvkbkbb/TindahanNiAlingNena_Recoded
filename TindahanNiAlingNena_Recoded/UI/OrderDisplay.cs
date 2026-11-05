using TindahanNiAlingNena_Recoded.Models;

namespace TindahanNiAlingNena_Recoded.UI;

public class OrderDisplay : Panel
{
    private readonly Label receiptContent = new();

    public OrderDisplay()
    {
        AutoScroll = true;
        AutoScrollMargin = new Size(0, 12);
        BackColor = Color.White;
        BackgroundImageLayout = ImageLayout.Stretch;

        string receiptPath = Path.Combine("Assets", "Receipt.png");
        if (File.Exists(receiptPath))
        {
            BackgroundImage = Image.FromFile(receiptPath);
        }

        Padding = new Padding(36, 72, 36, 16);
        receiptContent.AutoSize = true;
        receiptContent.BackColor = Color.Transparent;
        receiptContent.Dock = DockStyle.Top;
        receiptContent.ForeColor = Color.FromArgb(44, 54, 45);
        receiptContent.TextAlign = ContentAlignment.TopCenter;
        Controls.Add(receiptContent);

        SizeChanged += (_, _) => UpdateReceiptContentWidth();
        UpdateReceiptContentWidth();
    }

    public void ShowOrder(Order order, string customerName, int amountPaid)
    {
        List<string> lines = new()
        {
            $"{customerName}'s Order",
            string.Empty
        };

        foreach (Product product in order.GetProducts())
        {
            lines.Add($"{product.GetName()}     ₱{product.GetPrice()}");
        }

        lines.Add(string.Empty);
        lines.Add($"TOTAL                 ₱{order.GetTotalCost()}");
        lines.Add($"Customer paid      ₱{amountPaid}");
        receiptContent.Text = string.Join(Environment.NewLine, lines);
    }

    private void UpdateReceiptContentWidth()
    {
        int contentWidth = Math.Max(
            1,
            ClientSize.Width - Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);
        receiptContent.MaximumSize = new Size(contentWidth, 0);
        receiptContent.Location = new Point(Padding.Left, Padding.Top);
    }
}
