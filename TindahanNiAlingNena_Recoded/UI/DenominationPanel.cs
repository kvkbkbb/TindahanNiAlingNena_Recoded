// UI/DenominationPanel.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationPanel : TableLayoutPanel
{
    public event Action<Denomination>? DenominationClicked;

    private TableLayoutPanel billArea = new TableLayoutPanel();
    private TableLayoutPanel coinArea = new TableLayoutPanel();
    private readonly Label heading = new();
    private readonly Label instruction = new();
    private readonly TableLayoutPanel drawer = new();
    private readonly Dictionary<int, Label> countLabels = new();

    public DenominationPanel()
    {
        BackColor = Color.FromArgb(39, 82, 61);
        Padding = new Padding(14, 8, 14, 12);
        Margin = new Padding(6);
        ColumnCount = 1;
        RowCount = 2;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        RowStyles.Add(new RowStyle(SizeType.Absolute, 45f));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        SetupBillArea();
        SetupCoinArea();

        heading.Text = "CASH DRAWER";
        heading.Dock = DockStyle.Left;
        heading.AutoSize = true;
        heading.ForeColor = Color.White;
        heading.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        heading.TextAlign = ContentAlignment.MiddleLeft;

        instruction.Text = "Double-click a note or coin to add it to the change.";
        instruction.Dock = DockStyle.Right;
        instruction.AutoSize = true;
        instruction.ForeColor = Color.FromArgb(218, 232, 218);
        instruction.Font = new Font("Segoe UI", 9F);
        instruction.TextAlign = ContentAlignment.MiddleRight;

        Panel header = new() { Dock = DockStyle.Fill, BackColor = Color.Transparent };
        header.Controls.Add(heading);
        header.Controls.Add(instruction);

        drawer.Dock = DockStyle.Fill;
        drawer.BackColor = Color.FromArgb(242, 236, 222);
        drawer.Padding = new Padding(7);
        drawer.ColumnCount = 2;
        drawer.RowCount = 1;
        drawer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76f));
        drawer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24f));
        drawer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        drawer.Controls.Add(billArea, 0, 0);
        drawer.Controls.Add(coinArea, 1, 0);

        Controls.Add(header, 0, 0);
        Controls.Add(drawer, 0, 1);
    }

    private void SetupBillArea()
    {
        billArea.Dock = DockStyle.Fill;
        billArea.ColumnCount = 6;
        billArea.RowCount = 1;
        billArea.BackColor = Color.Transparent;
        billArea.Padding = new Padding(0, 2, 0, 2);

        for (int i = 0; i < billArea.ColumnCount; i++)
        {
            billArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 6));
        }

        billArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        // largest bill first
        List<Bill> bills = new List<Bill>(Bill.Denominations.Values);
        bills.Reverse();

        foreach (Bill bill in bills)
        {
            DenominationButton billButton = new DenominationButton(bill);
            billButton.Anchor = AnchorStyles.None;   // centered in its slot, keeps its own size

            billArea.Controls.Add(CreateSlot(billButton));
        }
    }

    private void SetupCoinArea()
    {
        coinArea.Dock = DockStyle.Fill;
        coinArea.ColumnCount = 1;
        coinArea.RowCount = 3;
        coinArea.BackColor = Color.Transparent;

        coinArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        for (int i = 0; i < coinArea.RowCount; i++)
        {
            coinArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 3));
        }

        foreach (Coin coin in Coin.Denominations.Values)
        {
            DenominationButton coinButton = new DenominationButton(coin);
            coinButton.Anchor = AnchorStyles.None;            // centered in its slot

            coinArea.Controls.Add(CreateSlot(coinButton));
        }
    }

    // Puts a button inside its own box, like a compartment in a cash drawer.
    private Panel CreateSlot(DenominationButton button)
    {
        Panel slot = new()
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(5, 4, 5, 4),
            BackColor = Color.FromArgb(255, 253, 247),
            BorderStyle = BorderStyle.FixedSingle
        };

        Label countLabel = new()
        {
            AutoSize = true,
            BackColor = Color.FromArgb(242, 236, 222),
            ForeColor = Color.FromArgb(44, 54, 45),
            Font = new Font("Segoe UI", 8F, FontStyle.Bold),
            Margin = Padding.Empty,
            Padding = new Padding(3, 1, 3, 1),
            Text = "× 0"
        };
        countLabels[button.GetDenomination().Value] = countLabel;

        button.DoubleClick += OnDenominationButtonClicked;
        slot.Controls.Add(button);
        slot.Controls.Add(countLabel);

        void PositionSlotControls(object? sender, EventArgs e)
        {
            const int imageInset = 4;
            int imageWidth = Math.Max(0, slot.ClientSize.Width - imageInset * 2);
            int imageHeight = Math.Max(0, slot.ClientSize.Height - imageInset * 2);
            button.Size = new Size(imageWidth, imageHeight);
            button.Location = new Point(imageInset, imageInset);
            countLabel.Location = new Point(
                Math.Max(0, slot.ClientSize.Width - countLabel.Width - 3),
                Math.Max(0, slot.ClientSize.Height - countLabel.Height - 3));
        }

        slot.SizeChanged += PositionSlotControls;
        countLabel.SizeChanged += PositionSlotControls;
        countLabel.BringToFront();
        PositionSlotControls(slot, EventArgs.Empty);

        return slot;
    }

    public void ShowPieceCounts(IReadOnlyDictionary<int, int> pieceCounts)
    {
        foreach ((int value, Label countLabel) in countLabels)
        {
            int count = pieceCounts.TryGetValue(value, out int available) ? available : 0;
            countLabel.Text = $"× {count}";
            countLabel.AccessibleName = $"{count} pieces of {value} pesos";
        }
    }

    private void OnDenominationButtonClicked(object? sender, EventArgs e)
    {
        DenominationButton clickedButton = (DenominationButton)sender!;
        Denomination clickedDenomination = clickedButton.GetDenomination();

        if (DenominationClicked == null)
        {
            return;
        }

        DenominationClicked.Invoke(clickedDenomination);
    }
}