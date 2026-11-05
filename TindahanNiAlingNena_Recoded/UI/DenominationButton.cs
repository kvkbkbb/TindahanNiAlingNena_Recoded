// UI/DenominationButton.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationButton : PictureBox
{
    private Denomination denomination;
    private float ImageScale = 0.16f;
    private const int HoverGrowAmount = 10;

    public DenominationButton(Denomination denomination)
    {
        // Lets this PictureBox raise Click and DoubleClick events.
        SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
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

        MouseEnter += OnMouseEntered;
        MouseLeave += OnMouseLeft;
    }

    public Denomination GetDenomination()
    {
        return denomination;
    }

    private void OnMouseEntered(object? sender, EventArgs e)
    {
        Width = Width + HoverGrowAmount;
        Height = Height + HoverGrowAmount;
    }

    private void OnMouseLeft(object? sender, EventArgs e)
    {
        Width = Width - HoverGrowAmount;
        Height = Height - HoverGrowAmount;
    }
}