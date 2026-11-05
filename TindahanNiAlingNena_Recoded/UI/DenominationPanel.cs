// UI/DenominationPanel.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationPanel : FlowLayoutPanel
{
    // GameScreen listens to this, like a BindableEvent in Roblox
    public event Action<Denomination>? DenominationClicked;

    public DenominationPanel()
    {
        foreach (Coin coin in Coin.Denominations.Values)
        {
            AddButton(coin);
        }

        foreach (Bill bill in Bill.Denominations.Values)
        {
            AddButton(bill);
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
        DenominationButton clicked = (DenominationButton)sender!;

        if (DenominationClicked != null)
        {
            DenominationClicked.Invoke(clicked.GetDenomination());
        }
    }
}