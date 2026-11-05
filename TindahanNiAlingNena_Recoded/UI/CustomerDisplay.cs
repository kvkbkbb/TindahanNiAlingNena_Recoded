namespace TindahanNiAlingNena_Recoded.UI;

public enum CustomerState
{
    Normal,
    Happy,
    Angry
}

public class CustomerDisplay : PictureBox
{
    private const int TotalCustomers = 10;

    private readonly int[] menNumbers = { 1, 3, 5, 6, 9, 10 };
    private readonly int[] womenNumbers = { 2, 4, 7, 8 };
    private readonly Random random = new();
    private Image? customerImage;

    public CustomerDisplay()
    {
        BackColor = Color.FromArgb(32, 34, 30);
        SizeMode = PictureBoxSizeMode.Normal;
    }

    public void DisplayBackground()
    {
        customerImage?.Dispose();
        customerImage = null;
        Invalidate();
    }

    public int DisplayCustomer(CustomerState state, int? customerNumber = null, Gender? gender = null)
    {
        int numberToShow = customerNumber ?? GetRandomCustomerNumber(gender);
        string genderFolder = GetGenderFolder(numberToShow);
        string fileName = $"C{numberToShow}.jpg";
        string imagePath = Path.Combine("Assets", "Customers", state.ToString(), genderFolder, fileName);

        using Image sourceImage = Image.FromFile(imagePath);
        Image nextImage = new Bitmap(sourceImage);
        Image? previousImage = customerImage;
        customerImage = nextImage;
        previousImage?.Dispose();
        Invalidate();

        return numberToShow;
    }

    private int GetRandomCustomerNumber(Gender? gender)
    {
        int[] numbers = gender switch
        {
            Gender.Male => menNumbers,
            Gender.Female => womenNumbers,
            null => Enumerable.Range(1, TotalCustomers).ToArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(gender))
        };

        return numbers[random.Next(numbers.Length)];
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (customerImage == null || ClientSize.Width == 0 || ClientSize.Height == 0)
        {
            return;
        }

        float scale = Math.Max(
            ClientSize.Width / (float)customerImage.Width,
            ClientSize.Height / (float)customerImage.Height);
        int width = (int)Math.Ceiling(customerImage.Width * scale);
        int height = (int)Math.Ceiling(customerImage.Height * scale);
        Rectangle destination = new(
            (ClientSize.Width - width) / 2,
            (ClientSize.Height - height) / 2,
            width,
            height);

        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        e.Graphics.DrawImage(customerImage, destination);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            customerImage?.Dispose();
        }

        base.Dispose(disposing);
    }

    private string GetGenderFolder(int customerNumber)
    {
        if (menNumbers.Contains(customerNumber))
        {
            return "Men";
        }

        if (womenNumbers.Contains(customerNumber))
        {
            return "Women";
        }

        throw new ArgumentException($"There is no customer number {customerNumber}", nameof(customerNumber));
    }
}
