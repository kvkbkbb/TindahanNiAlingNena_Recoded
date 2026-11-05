using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.UI;

public class GameScreen : Form
{
    private static readonly Color CanvasColor = Color.FromArgb(246, 242, 232);
    private static readonly Color InkColor = Color.FromArgb(44, 54, 45);
    private static readonly Color GreenColor = Color.FromArgb(48, 105, 75);
    private const int HeaderHeight = 76;
    private const int DrawerHeight = 248;
    private const float CustomerAreaWidthRatio = 0.54F;

    public event Action<Denomination>? DenominationClicked;
    public event Action? GiveChangeClicked;
    public event Action? ClearClicked;
    public event Action? AskForMoreClicked;

    private readonly TableLayoutPanel buttonPanel = new();
    private readonly CustomerDisplay customerDisplay = new();
    private readonly OrderDisplay orderDisplay = new();
    private readonly DenominationPanel denominationPanel = new();
    private readonly Label scoreLabel = new();
    private readonly TableLayoutPanel orderPanel = new();
    private Control header = null!;

    public GameScreen()
    {
        Text = "Tindahan ni Aling Nena";
        ClientSize = new Size(1280, 850);
        MinimumSize = new Size(1000, 700);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = CanvasColor;
        ForeColor = InkColor;
        Font = new Font("Segoe UI", 10F);

        SetupCustomerDisplay();
        SetupOrderDisplay();
        SetupButtonPanel();
        SetupCashbox();
        SetupOrderPanel();

        header = CreateHeader();
        Controls.Add(customerDisplay);
        Controls.Add(header);
        Controls.Add(orderPanel);
        Controls.Add(denominationPanel);
        customerDisplay.SendToBack();
        Resize += OnScreenResized;
        LayoutOverlayControls();
    }

    private Control CreateHeader()
    {
        Panel header = new()
        {
            Dock = DockStyle.None,
            BackColor = Color.FromArgb(39, 82, 61),
            Padding = new Padding(20, 10, 20, 10),
            Margin = Padding.Empty
        };

        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            BackColor = Color.Transparent
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72F));
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        Panel titlePanel = new() { Dock = DockStyle.Fill, BackColor = Color.Transparent };
        Label title = new()
        {
            Text = "Tindahan ni Aling Nena",
            Dock = DockStyle.Fill,
            AutoSize = false,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 20F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        titlePanel.Controls.Add(title);

        Panel scoreCard = new()
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(57, 110, 78),
            Margin = new Padding(12, 4, 0, 4)
        };
        scoreLabel.Dock = DockStyle.Fill;
        scoreLabel.Text = "SCORE  0";
        scoreLabel.TextAlign = ContentAlignment.MiddleCenter;
        scoreLabel.ForeColor = Color.White;
        scoreLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        scoreCard.Controls.Add(scoreLabel);

        content.Controls.Add(titlePanel, 0, 0);
        content.Controls.Add(scoreCard, 1, 0);
        header.Controls.Add(content);
        return header;
    }

    private void SetupCustomerDisplay()
    {
        customerDisplay.Dock = DockStyle.None;
    }

    private void SetupOrderDisplay()
    {
        orderDisplay.Dock = DockStyle.Fill;
        orderDisplay.BackColor = Color.White;
        orderDisplay.ForeColor = InkColor;
        orderDisplay.Font = new Font("Segoe UI", 10F);
        orderDisplay.Padding = new Padding(36, 24, 36, 16);
        orderDisplay.TextAlign = ContentAlignment.TopCenter;
    }

    private void SetupButtonPanel()
    {
        buttonPanel.Dock = DockStyle.Fill;
        buttonPanel.ColumnCount = 3;
        buttonPanel.RowCount = 1;
        buttonPanel.Padding = new Padding(0, 8, 0, 4);
        buttonPanel.BackColor = Color.FromArgb(246, 242, 232);
        buttonPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        for (int i = 0; i < buttonPanel.ColumnCount; i++)
        {
            buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / buttonPanel.ColumnCount));
        }

        AddActionButton("Give change", GreenColor, Color.White, OnGiveChangeButtonClicked);
        AddActionButton("Clear", Color.White, InkColor, OnClearButtonClicked, true);
        AddActionButton("Ask for more", Color.FromArgb(222, 164, 75), Color.FromArgb(53, 46, 34), OnAskForMoreButtonClicked);
    }

    private void SetupCashbox()
    {
        denominationPanel.Dock = DockStyle.None;
        denominationPanel.DenominationClicked += OnPanelDenominationClicked;
    }

    private void SetupOrderPanel()
    {
        orderPanel.BackColor = CanvasColor;
        orderPanel.Padding = new Padding(8);
        orderPanel.ColumnCount = 1;
        orderPanel.RowCount = 2;
        orderPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        orderPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        orderPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
        orderDisplay.Dock = DockStyle.Fill;
        orderDisplay.Margin = new Padding(0);
        buttonPanel.Margin = new Padding(0);
        orderPanel.Controls.Add(orderDisplay, 0, 0);
        orderPanel.Controls.Add(buttonPanel, 0, 1);
    }

    private void OnScreenResized(object? sender, EventArgs e)
    {
        LayoutOverlayControls();
    }

    private void LayoutOverlayControls()
    {
        int horizontalMargin = 14;
        int customerAreaWidth = (int)(ClientSize.Width * CustomerAreaWidthRatio);
        int rightAreaLeft = customerAreaWidth + horizontalMargin;
        int rightAreaWidth = ClientSize.Width - rightAreaLeft - horizontalMargin;
        int drawerTop = ClientSize.Height - DrawerHeight - horizontalMargin;
        int orderTop = HeaderHeight + 24;

        customerDisplay.Bounds = new Rectangle(
            0,
            0,
            customerAreaWidth,
            ClientSize.Height);
        header.Bounds = new Rectangle(
            rightAreaLeft,
            12,
            rightAreaWidth,
            HeaderHeight);
        denominationPanel.Bounds = new Rectangle(
            horizontalMargin,
            drawerTop,
            ClientSize.Width - horizontalMargin * 2,
            DrawerHeight);
        orderPanel.Bounds = new Rectangle(
            rightAreaLeft,
            orderTop,
            rightAreaWidth,
            Math.Max(200, drawerTop - orderTop - 14));
    }

    private void AddActionButton(string text, Color backColor, Color foreColor, EventHandler onClick, bool outlined = false)
    {
        Button button = new()
        {
            Text = text,
            Dock = DockStyle.Fill,
            Margin = new Padding(4, 5, 4, 5),
            BackColor = backColor,
            ForeColor = foreColor,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = outlined ? 1 : 0;
        button.FlatAppearance.BorderColor = Color.FromArgb(214, 208, 194);
        button.FlatAppearance.MouseOverBackColor = outlined
            ? Color.FromArgb(246, 243, 235)
            : ControlPaint.Light(backColor, 0.12F);
        button.Click += (_, _) => AudioPlayer.PlaySound("button.wav");
        button.Click += onClick;
        buttonPanel.Controls.Add(button, buttonPanel.Controls.Count, 0);
    }

    public void ShowState(Order order, int amountPaid, int changeGiven, int score)
    {
        orderDisplay.ShowOrder(order, amountPaid, changeGiven);
        scoreLabel.Text = $"SCORE  {score}";
    }

    public void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "Tindahan ni Aling Nena",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public int ShowCustomer(CustomerState state, int? customerNumber = null)
    {
        return customerDisplay.DisplayCustomer(state, customerNumber);
    }

    public void ClearCustomer()
    {
        customerDisplay.DisplayBackground();
    }

    private void OnPanelDenominationClicked(Denomination piece)
    {
        DenominationClicked?.Invoke(piece);
    }

    private void OnGiveChangeButtonClicked(object? sender, EventArgs e)
    {
        GiveChangeClicked?.Invoke();
    }

    private void OnClearButtonClicked(object? sender, EventArgs e)
    {
        ClearClicked?.Invoke();
    }

    private void OnAskForMoreButtonClicked(object? sender, EventArgs e)
    {
        AskForMoreClicked?.Invoke();
    }
}
