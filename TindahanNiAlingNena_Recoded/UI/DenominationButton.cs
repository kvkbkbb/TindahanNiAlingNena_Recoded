// UI/DenominationButton.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationButton : PictureBox
{
    private Denomination denomination;
    private float ImageScale = 0.16f;

    public DenominationButton(Denomination denomination)
    {
        SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true); // Makes the button only listen to double clicks.
        this.denomination = denomination;

        Image = Image.FromFile(denomination.ImagePath);
        SizeMode = PictureBoxSizeMode.Zoom;
        Width = (int)(Image.Width * ImageScale);
        Height = (int)(Image.Height * ImageScale);

        if (denomination is Bill)
        {
            Height = (int)(Image.Width * ImageScale);
            Width = (int)(Image.Height * ImageScale);
            Image.RotateFlip(RotateFlipType.Rotate90FlipNone);   
        }
    }

    public Denomination GetDenomination()
    {
        return denomination;
    }
}