using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

public enum Gender
{
    Male,
    Female
}
public abstract class Customer
{
    protected Order order;
    private Gender gender;
    private string name;

    private static Random random = new Random();

    public Customer(Order order)
    {
        this.order = order;

        // Coin flip
        if (random.Next(2) == 0)
        {
            gender = Gender.Male;
        }
        else
        {
            gender = Gender.Female;
        }

        name = NameGenerator.GenerateName(gender);
    }

    public string GetName()
    {
        return name;
    }

    public Gender GetGender()
    {
        return gender;
    }

    public Order GetOrder()
    {
        return order;
    }

    public abstract List<Denomination> GeneratePayment(PaymentGenerator paymentGenerator);
}