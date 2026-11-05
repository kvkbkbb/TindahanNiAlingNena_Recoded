// UI/CustomerDisplay.cs
namespace TindahanNiAlingNena_Recoded.UI;

public enum CustomerState
{
    Normal,
    Happy,
    Angry
}

public class CustomerDisplay : PictureBox
{
    private const int TotalCustomers = 10;   // C1 to C10

    // which customer numbers live in which folder
    private int[] menNumbers = { 1, 3, 5, 6, 9, 10 };
    private int[] womenNumbers = { 2, 4, 7, 8 };

    private PictureBox customerPicture = new PictureBox();
    private Random random = new Random();

    public CustomerDisplay()
    {
        SizeMode = PictureBoxSizeMode.Zoom;
        BackColor = Color.LightGray;   // shown when there is no background image

        string backgroundPath = Path.Combine("Assets", "Background.png");

        if (File.Exists(backgroundPath))
        {
            Image = Image.FromFile(backgroundPath);
        }

        customerPicture.Dock = DockStyle.Fill;
        customerPicture.SizeMode = PictureBoxSizeMode.Zoom;
        customerPicture.BackColor = Color.Transparent;
        customerPicture.Visible = false;

        Controls.Add(customerPicture);
    }

    // Empties the display so only the background shows.
    public void DisplayBackground()
    {
        customerPicture.Image = null;
        customerPicture.Visible = false;
    }

    // Shows a customer in the given state.
    // If customerNumber is left out, a random customer is chosen.
    // Returns the number that was shown, so the same customer can be shown again later.
    public int DisplayCustomer(CustomerState state, int? customerNumber = null)
    {
        int numberToShow;

        if (customerNumber == null)
        {
            numberToShow = random.Next(1, TotalCustomers + 1);   // 1 to 10
        }
        else
        {
            numberToShow = customerNumber.Value;
        }

        string genderFolder = GetGenderFolder(numberToShow);
        string fileName = "C" + numberToShow + ".jpg";

        string imagePath = Path.Combine("Assets", "Customers", state.ToString(), genderFolder, fileName);

        customerPicture.Image = Image.FromFile(imagePath);
        customerPicture.Visible = true;

        return numberToShow;
    }

    // Works out whether a customer number is in the Men or Women folder.
    private string GetGenderFolder(int customerNumber)
    {
        foreach (int number in menNumbers)
        {
            if (number == customerNumber)
            {
                return "Men";
            }
        }

        foreach (int number in womenNumbers)
        {
            if (number == customerNumber)
            {
                return "Women";
            }
        }

        throw new ArgumentException("There is no customer number " + customerNumber);
    }
}