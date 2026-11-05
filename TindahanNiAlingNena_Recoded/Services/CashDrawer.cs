using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.Services;

public class CashDrawer
{
    private List<Denomination> pieces = new List<Denomination>();

    public CashDrawer()
    {
        // Small denominations first, since they are used most often for change.
        AddStartingPieces(Coin.Denominations[1], 30);
        AddStartingPieces(Coin.Denominations[5], 20);
        AddStartingPieces(Coin.Denominations[10], 10);

        AddStartingPieces(Bill.Denominations[20], 10);
        AddStartingPieces(Bill.Denominations[50], 5);
        AddStartingPieces(Bill.Denominations[100], 2);
        //AddStartingPieces(Bill.Denominations[200], 2);
        //AddStartingPieces(Bill.Denominations[500], 1);
    }

    private void AddStartingPieces(Denomination denomination, int amountOfPieces)
    {
        for (int i = 0; i < amountOfPieces; i++)
        {
            pieces.Add(denomination);
        }
    }

    // Puts one bill or coin into the drawer (for example, when a customer pays).
    public void AddPiece(Denomination piece)
    {
        pieces.Add(piece);
    }

    // Puts a whole payment into the drawer.
    public void AddPieces(List<Denomination> payment)
    {
        foreach (Denomination piece in payment)
        {
            pieces.Add(piece);
        }
    }

    // Takes one bill or coin with this value out of the drawer.
    // Returns false if the drawer has none left.
    public bool RemovePiece(int value)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].Value == value)
            {
                pieces.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    // How many bills or coins of this value are in the drawer.
    public int GetPieceCount(int value)
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

    // Total pesos in the drawer.
    public int GetTotalAmount()
    {
        int totalAmount = 0;

        foreach (Denomination piece in pieces)
        {
            totalAmount += piece.Value;
        }

        return totalAmount;
    }
}