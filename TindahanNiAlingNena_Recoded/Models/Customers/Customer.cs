using TindahanNiAlingNena_Recoded.Models;
using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

public abstract class Customer
{
    protected Order order;

    public Customer(Order order)
    {
        this.order = order;
    }

    public Order GetOrder()
    {
        return order;
    }

    public abstract List<Denomination> GeneratePayment(PaymentGenerator paymentGenerator);
}