// UI/DenominationPanel.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationPanel : FlowLayoutPanel
{
    // GameScreen listens to this, like a BindableEvent in Roblox
    public event Action<Denomination>? DenominationClicked;

    public DenominationPanel()
    {
        // So the for loop is reversed.
        List<Bill> bills = new List<Bill>(Bill.Denominations.Values);
        for (int i = bills.Count - 1; i >= 0; i--)
        {
            AddButton(bills[i]);
        }

        foreach (Coin coin in Coin.Denominations.Values)
        {
            AddButton(coin);
        }
    }

    private void AddButton(Denomination denomination)
    {
        DenominationButton button = new DenominationButton(denomination);
        button.Click += OnButtonClicked;
        Controls.Add(button);
    }

    private void OnButtonClicked(object? sender, EventArgs e)
    {
        DenominationButton clickedButton = (DenominationButton)sender!;
        Denomination clickedDenomination = clickedButton.GetDenomination();

        // DenominationClicked is an event, and an event with no subscribers is null. Kaya may check before invoking.
        if (DenominationClicked == null) {return;}

        DenominationClicked.Invoke(clickedDenomination);
    }
}