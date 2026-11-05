// UI/DenominationPanel.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationPanel : TableLayoutPanel
{
    public event Action<Denomination>? DenominationClicked;

    private const int CoinSize = 60;

    private TableLayoutPanel billArea = new TableLayoutPanel();
    private TableLayoutPanel coinArea = new TableLayoutPanel();

    public DenominationPanel()
    {
        BackColor = Color.FromArgb(40, 40, 40);
        Padding = new Padding(10);

        // two sides: bills on the left, coins on the right
        ColumnCount = 2;
        RowCount = 1;
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75f));
        ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
        RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        SetupBillArea();
        SetupCoinArea();

        Controls.Add(billArea, 0, 0);
        Controls.Add(coinArea, 1, 0);
    }

    private void SetupBillArea()
    {
        billArea.Dock = DockStyle.Fill;
        billArea.ColumnCount = 6;
        billArea.RowCount = 1;

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

        coinArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

        for (int i = 0; i < coinArea.RowCount; i++)
        {
            coinArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / 3));
        }

        foreach (Coin coin in Coin.Denominations.Values)
        {
            DenominationButton coinButton = new DenominationButton(coin);
            coinButton.Size = new Size(CoinSize, CoinSize);   // small, so bills look bigger
            coinButton.Anchor = AnchorStyles.None;            // centered in its slot

            coinArea.Controls.Add(CreateSlot(coinButton));
        }
    }

    // Puts a button inside its own box, like a compartment in a cash drawer.
    private TableLayoutPanel CreateSlot(DenominationButton button)
    {
        TableLayoutPanel slot = new TableLayoutPanel();
        slot.Dock = DockStyle.Fill;
        slot.Margin = new Padding(4);
        slot.BackColor = Color.FromArgb(75, 75, 75);
        slot.BorderStyle = BorderStyle.Fixed3D;

        slot.ColumnCount = 1;
        slot.RowCount = 1;
        slot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        slot.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

        button.DoubleClick += OnDenominationButtonClicked;   // use Click for single click
        slot.Controls.Add(button);

        return slot;
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