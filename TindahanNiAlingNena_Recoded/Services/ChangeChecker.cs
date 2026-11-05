// Services/ChangeChecker.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.Services;

public class ChangeChecker
{
    // Adds up the peso value of every bill and coin in the list.
    public int GetTotal(List<Denomination> pieces)
    {
        int total = 0;

        foreach (Denomination piece in pieces)
        {
            total += piece.Value;
        }

        return total;
    }

    // Counts how many pieces in the list have this value.
    public int CountPieces(List<Denomination> pieces, int value)
    {
        int count = 0;

        foreach (Denomination piece in pieces)
        {
            if (piece.Value == value)
            {
                count++;
            }
        }

        return count;
    }

    // Change is correct when it equals what the customer overpaid.
    public bool IsChangeCorrect(int orderTotal, int amountPaid, List<Denomination> changeGiven)
    {
        return GetTotal(changeGiven) == amountPaid - orderTotal;
    }
}