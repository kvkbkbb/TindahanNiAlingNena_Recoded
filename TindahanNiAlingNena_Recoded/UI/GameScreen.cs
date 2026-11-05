// UI/GameScreen.cs
using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class GameScreen : Form
{
    // like BindableEvents: Program.cs connects these to the Game
    public event Action<Denomination>? DenominationClicked;
    public event Action? GiveChangeClicked;
    public event Action? ClearClicked;
    public event Action? AskForMoreClicked;

    private OrderDisplay orderDisplay = new OrderDisplay();
    private DenominationPanel denominationPanel = new DenominationPanel();
    private Label statusLabel = new Label();
    private Label scoreLabel = new Label();

    public GameScreen()
    {
        Text = "Tindahan ni Aling Nena";
        Width = 950;
        Height = 750;

        orderDisplay.Location = new Point(20, 20);
        orderDisplay.Size = new Size(400, 200);

        statusLabel.Location = new Point(20, 240);
        statusLabel.Size = new Size(640, 30);

        scoreLabel.Location = new Point(440, 20);
        scoreLabel.Size = new Size(200, 30);

        denominationPanel.Location = new Point(20, 300);
        denominationPanel.Size = new Size(660, 150);
        denominationPanel.DenominationClicked += OnPanelDenominationClicked;

        Controls.Add(orderDisplay);
        Controls.Add(statusLabel);
        Controls.Add(scoreLabel);
        Controls.Add(denominationPanel);

        AddActionButton("Give Change", 20, OnGiveChangeButtonClicked);
        AddActionButton("Clear Change", 180, OnClearButtonClicked);
        AddActionButton("Ask for More Money", 340, OnAskForMoreButtonClicked);
    }

    // Program.cs connects these two to the Game's events
    public void ShowState(Order order, int amountPaid, int changeGiven, int score)
    {
        orderDisplay.ShowOrder(order, amountPaid, changeGiven);
        scoreLabel.Text = "Score: " + score;
    }

    public void ShowMessage(string message)
    {
        statusLabel.Text = message;
    }

    private void AddActionButton(string text, int x, EventHandler onClick)
    {
        Button button = new Button();
        button.Text = text;
        button.Location = new Point(x, 460);
        button.Size = new Size(150, 40);
        button.Click += onClick;
        Controls.Add(button);
    }

    private void OnPanelDenominationClicked(Denomination piece)
    {
        if (DenominationClicked != null)
        {
            DenominationClicked.Invoke(piece);
        }
    }

    private void OnGiveChangeButtonClicked(object? sender, EventArgs e)
    {
        if (GiveChangeClicked != null)
        {
            GiveChangeClicked.Invoke();
        }
    }

    private void OnClearButtonClicked(object? sender, EventArgs e)
    {
        if (ClearClicked != null)
        {
            ClearClicked.Invoke();
        }
    }

    private void OnAskForMoreButtonClicked(object? sender, EventArgs e)
    {
        if (AskForMoreClicked != null)
        {
            AskForMoreClicked.Invoke();
        }
    }
}