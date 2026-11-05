// Services/Game.cs
using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Customers;
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.Services;

public class GameSession
{
    // Game builds its own parts
    private ProductCatalog catalog = new ProductCatalog();
    private OrderGenerator orderGenerator;
    private PaymentGenerator paymentGenerator = new PaymentGenerator();
    private CashDrawer cashDrawer = new CashDrawer();
    private ChangeChecker changeChecker = new ChangeChecker();
    private Random random = new Random();

    private Customer? currentCustomer;
    private List<Denomination> currentPayment = new List<Denomination>();
    private List<Denomination> changeGiven = new List<Denomination>();
    private int score = 0;

    // order, amount paid, change being given, score
    public event Action<Order, int, int, int>? StateChanged;
    public event Action<string>? MessageSent;
    public event Action? CustomerStarted;

    public GameSession()
    {
        // OrderGenerator needs the catalog, so it is created here
        orderGenerator = new OrderGenerator(catalog);
    }

    public void Start()
    {
        StartNextCustomer();
    }

    public void AddChangePiece(Denomination piece)
    {
        int alreadyChosen = changeChecker.CountPieces(changeGiven, piece.Value);

        if (alreadyChosen >= cashDrawer.GetPieceCount(piece.Value))
        {
            SendMessage("No more P" + piece.Value + " left in the drawer!");
            return;
        }

        piece.PlaySound();
        changeGiven.Add(piece);
        SendState();
    }

    public void ClearChange()
    {
        changeGiven.Clear();
        SendState();
    }

    public void GiveChange()
    {
        if (currentCustomer == null)
        {
            return;
        }

        int orderTotal = currentCustomer.GetOrder().GetTotalCost();
        int amountPaid = changeChecker.GetTotal(currentPayment);

        if (amountPaid < orderTotal)
        {
            SendFailure("The customer didn't pay enough! Ask for more money.");
            return;
        }

        if (!changeChecker.IsChangeCorrect(orderTotal, amountPaid, changeGiven))
        {
            changeGiven.Clear();
            SendFailure("Wrong change! Try again.");
            SendState();
            return;
        }

        foreach (Denomination piece in changeGiven)
        {
            cashDrawer.RemovePiece(piece.Value);
        }

        score++;
        AudioPlayer.PlaySound("success.wav");
        StartNextCustomer();
    }

    public void AskForMore()
    {
        if (currentCustomer == null)
        {
            return;
        }

        int orderTotal = currentCustomer.GetOrder().GetTotalCost();
        int amountPaid = changeChecker.GetTotal(currentPayment);

        if (amountPaid >= orderTotal)
        {
            SendFailure("The customer already paid enough!");
            return;
        }

        List<Denomination> additionalPayment =
            paymentGenerator.GenerateExactPayment(orderTotal - amountPaid);
        currentPayment.AddRange(additionalPayment);
        cashDrawer.AddPieces(additionalPayment);
        SendState();
    }

    private void StartNextCustomer()
    {
        Order order = orderGenerator.GenerateOrder(1, 4);
        int customerType = random.Next(100);

        if (customerType < 10)
        {
            currentCustomer = new ExactPayingCustomer(order);
        }
        else if (customerType < 80)
        {
            currentCustomer = new OverpayingCustomer(order);
        }
        else
        {
            currentCustomer = new UnderpayingCustomer(order);
        }

        currentPayment = currentCustomer.GeneratePayment(paymentGenerator);
        cashDrawer.AddPieces(currentPayment);
        changeGiven.Clear();

        CustomerStarted?.Invoke();
        SendState();
    }

    private void SendState()
    {
        if (currentCustomer == null || StateChanged == null)
        {
            return;
        }

        StateChanged.Invoke(
            currentCustomer.GetOrder(),
            changeChecker.GetTotal(currentPayment),
            changeChecker.GetTotal(changeGiven),
            score);
    }

    private void SendMessage(string message)
    {
        if (MessageSent != null)
        {
            MessageSent.Invoke(message);
        }
    }

    private void SendFailure(string message)
    {
        AudioPlayer.PlaySound("fail.wav");
        SendMessage(message);
    }
}