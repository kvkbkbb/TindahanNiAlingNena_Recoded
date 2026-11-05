// UI/GameScreen.cs
using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class GameScreen : Form
{
    public event Action<Denomination>? DenominationClicked;
    public event Action? GiveChangeClicked;
    public event Action? ClearClicked;
    public event Action? AskForMoreClicked;

    private TableLayoutPanel mainLayout = new TableLayoutPanel();
    private FlowLayoutPanel buttonPanel = new FlowLayoutPanel();
    private FlowLayoutPanel infoPanel = new FlowLayoutPanel();

    private CustomerDisplay customerDisplay = new CustomerDisplay();
    private OrderDisplay orderDisplay = new OrderDisplay();
    private DenominationPanel denominationPanel = new DenominationPanel();
    private Label statusLabel = new Label();
    private Label scoreLabel = new Label();

    public GameScreen()
    {
        Text = "Tindahan ni Aling Nena";
        Width = 950;
        Height = 750;

        SetupMainLayout();
        SetupCustomerDisplay();
        SetupOrderDisplay();
        SetupButtonPanel();
        SetupInfoPanel();
        SetupCashbox();

        // Controls.Add(control, column, row)
        mainLayout.Controls.Add(customerDisplay, 0, 0);
        mainLayout.SetRowSpan(customerDisplay, 3);          // covers rows 0 to 2 on the left side
        mainLayout.Controls.Add(orderDisplay, 1, 0);
        mainLayout.Controls.Add(buttonPanel, 1, 1);
        mainLayout.Controls.Add(infoPanel, 1, 2);
        mainLayout.Controls.Add(denominationPanel, 0, 3);
        mainLayout.SetColumnSpan(denominationPanel, 2);     // cashbox stretches across both columns

        Controls.Add(mainLayout);
    }

    // ---------- building the screen ----------

    private void SetupMainLayout()
    {
        mainLayout.Dock = DockStyle.Fill;
        mainLayout.ColumnCount = 2;
        mainLayout.RowCount = 4;

        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35f));   // order display
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));   // action buttons
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10f));   // score and status
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 43f));   // cashbox
    }

    private void SetupCustomerDisplay()
    {
        customerDisplay.Dock = DockStyle.Fill;
    }

    private void SetupOrderDisplay()
    {
        orderDisplay.Dock = DockStyle.Fill;
        orderDisplay.BackColor = Color.CadetBlue;
    }

    private void SetupButtonPanel()
    {
        buttonPanel.Dock = DockStyle.Fill;

        AddActionButton("Give Change", OnGiveChangeButtonClicked);
        AddActionButton("Clear Change", OnClearButtonClicked);
        AddActionButton("Ask for More Money", OnAskForMoreButtonClicked);
    }

    private void SetupInfoPanel()
    {
        infoPanel.Dock = DockStyle.Fill;
        infoPanel.FlowDirection = FlowDirection.TopDown;

        scoreLabel.AutoSize = true;
        statusLabel.AutoSize = true;

        infoPanel.Controls.Add(scoreLabel);
        infoPanel.Controls.Add(statusLabel);
    }

    private void SetupCashbox()
    {
        denominationPanel.Dock = DockStyle.Fill;
        denominationPanel.BackColor = Color.Gray;
        denominationPanel.DenominationClicked += OnPanelDenominationClicked;
    }

    private void AddActionButton(string text, EventHandler onClick)
    {
        Button button = new Button();
        button.Text = text;
        button.Size = new Size(150, 40);
        button.Click += onClick;
        buttonPanel.Controls.Add(button);
    }

    // ---------- Program.cs connects these to the Game's events ----------

    public void ShowState(Order order, int amountPaid, int changeGiven, int score)
    {
        orderDisplay.ShowOrder(order, amountPaid, changeGiven);
        scoreLabel.Text = "Score: " + score;
    }

    public void ShowMessage(string message)
    {
        statusLabel.Text = message;
    }

    // Shows a customer. Returns the customer number so the same one can be shown again later.
    public int ShowCustomer(CustomerState state, int? customerNumber = null)
    {
        return customerDisplay.DisplayCustomer(state, customerNumber);
    }

    // Empties the customer area so only the background shows.
    public void ClearCustomer()
    {
        customerDisplay.DisplayBackground();
    }

    // ---------- events going out ----------

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