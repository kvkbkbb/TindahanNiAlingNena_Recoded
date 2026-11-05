using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.Models.Customers;

public class ExactPayingCustomer : Customer
{
    public ExactPayingCustomer(Order order) : base(order)
    {
        
    }

    public override List<Denomination> GeneratePayment(PaymentGenerator paymentGenerator)
    {
        return paymentGenerator.GenerateExactPayment(order.GetTotalCost());
    }
}