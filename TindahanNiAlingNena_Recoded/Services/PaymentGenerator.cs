// Services/PaymentGenerator.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.Services;

public class PaymentGenerator
{
    private readonly Random _random = new();
    private readonly List<Denomination> _allDenominations;

    public PaymentGenerator()
    {
        // every bill and coin in one list, smallest to largest
        _allDenominations = Bill.Denominations.Values.Cast<Denomination>()
            .Concat(Coin.Denominations.Values)
            .OrderBy(d => d.Value)
            .ToList();
    }

    // Sum of the returned list equals amount.
    public List<Denomination> GenerateExactPayment(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

        return BuildPayment(amount);
    }

    // Sum of the returned list is less than amount.
    // If amount is 1, the only possible underpayment is nothing, so the list is empty.
    public List<Denomination> GenerateUnderPayment(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

        if (amount == 1)
            return new List<Denomination>();

        int underpaidAmount = _random.Next(1, amount);
        return BuildPayment(underpaidAmount);
    }

    // Sum of the returned list is greater than amount.
    public List<Denomination> GenerateOverPayment(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive.");

        int maxOverpayment = Math.Max(10, amount / 2);
        int overpaidAmount = amount + _random.Next(1, maxOverpayment + 1);   // amount + 1 up to amount + maxOverpayment
        return BuildPayment(overpaidAmount);
    }

    // Randomly picks denominations that still fit until the target is reached exactly.
    // Always finishes because the ₱1 coin fits any remaining amount.
    private List<Denomination> BuildPayment(int targetAmount)
    {
        // Denomination List
        var paymentDenominations = new List<Denomination>();
        int amountLeftToCover = targetAmount;

        while (amountLeftToCover > 0)
        {
            // Denomination List
            var denominationsThatStillFit = new List<Denomination>();

            foreach (var denomination in _allDenominations)
            {
                if (denomination.Value <= amountLeftToCover)
                {
                    denominationsThatStillFit.Add(denomination);
                }
            }

            // pick one of them at random
            int randomIndex = _random.Next(denominationsThatStillFit.Count);
            var chosenPiece = denominationsThatStillFit[randomIndex];

            paymentDenominations.Add(chosenPiece);
            amountLeftToCover -= chosenPiece.Value;
        }

        return paymentDenominations;
    }
}