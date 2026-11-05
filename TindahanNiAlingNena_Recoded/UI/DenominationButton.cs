// UI/DenominationButton.cs
using TindahanNiAlingNena_Recoded.Models.Money;

namespace TindahanNiAlingNena_Recoded.UI;

public class DenominationButton : PictureBox
{
    private Denomination denomination;
    private const float ImageScale = 0.16f;
    private readonly ToolTip toolTip = new();

    public DenominationButton(Denomination denomination)
    {
        SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        this.denomination = denomination;

        Image = Image.FromFile(denomination.ImagePath);
        SizeMode = PictureBoxSizeMode.Zoom;
        Width = (int)(Image.Width * ImageScale);
        Height = (int)(Image.Height * ImageScale);
        Cursor = Cursors.Hand;
        BackColor = Color.Transparent;
        AccessibleName = $"₱{denomination.Value}";
        AccessibleDescription = $"Add a {denomination.Value} peso denomination to the change.";
        toolTip.SetToolTip(this, $"Add ₱{denomination.Value}");

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