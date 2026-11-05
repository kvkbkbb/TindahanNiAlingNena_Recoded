// UI/DenominationButton.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationButton : PictureBox
{
    private Denomination denomination;

    public DenominationButton(Denomination denomination)
    {
        this.denomination = denomination;

        Image = Image.FromFile(denomination.ImagePath);
        SizeMode = PictureBoxSizeMode.Zoom;
        Width = 80;
        Height = 40;
    }

    public Denomination GetDenomination()
    {
        return denomination;
    }
}