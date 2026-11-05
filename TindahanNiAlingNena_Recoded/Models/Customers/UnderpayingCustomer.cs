using TindahanNiAlingNena_Recoded.Models.Money;
using TindahanNiAlingNena_Recoded.Services;

namespace TindahanNiAlingNena_Recoded.Models.Customers;

public class UnderpayingCustomer : Customer
{
    public UnderpayingCustomer(Order order) : base(order)
    {
        
    }

    public override List<Denomination> GeneratePayment(PaymentGenerator paymentGenerator)
    {
        return paymentGenerator.GenerateUnderPayment(order.GetTotalCost());
    }
}