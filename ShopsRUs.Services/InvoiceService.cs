using ShopsRUs.Core;
using ShopsRUs.Data.Domain;
using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IRepository<Invoices> _invoiceRepository;
    private readonly ICustomerService _customerService;
    private readonly IDiscountService _discountService;

    public InvoiceService(IRepository<Invoices> invoiceRepository, ICustomerService customerService, IDiscountService discountService)
    {
        _invoiceRepository = invoiceRepository;
        _customerService = customerService;
        _discountService = discountService;
    }

    public InvoiceDtoModel? GetInvoiceCalculate(GetInvoiceRequestModel request)
    {
        var invoice = GetInvoice(request);
        if (invoice != null)
        {
            var amount = invoice.TotalAmount;
            var discountAmount = 0.00M;
            var customerRequestModel = new GetCustomerRequestModel { CustomerId = invoice.CustomerId };
            var customer = _customerService.GetCustomerById(customerRequestModel);

            if (customer != null)
            {
                if (!invoice.IsGroceries)
                {
                    if (string.IsNullOrEmpty(customer.CustomerType))
                    {
                        if (customer.CreatedDateTime <= DateTime.Now.AddYears(-2))
                        {
                            customer.CustomerType = "OldCustomer";
                        }
                    }

                    var discountRequestModel = new GetDiscountRequestModel { TypeName = customer.CustomerType };
                    var discount = _discountService.GetDiscountByType(discountRequestModel);
                    if (discount != null)
                    {
                        var calculateDiscountAmount = CalculateByDiscountRate(invoice.TotalAmount, discount.DiscountRate);
                        amount -= calculateDiscountAmount;
                        discountAmount += calculateDiscountAmount;
                    }
                }

                var calculate = CalculateByTotalAmount(invoice.TotalAmount);
                amount -= calculate;
                discountAmount += calculate;

                invoice.NetAmount = amount;
                invoice.DiscountAmount = discountAmount;
            }
        }
        
        return invoice;
    }

    private decimal CalculateByDiscountRate(decimal totalAmount, decimal discountRate)
    {
        var calculateAmount =(totalAmount * discountRate) / 100;
        return calculateAmount;
    }

    private decimal CalculateByTotalAmount(decimal totalAmount)
    {
        var calculateAmount = (totalAmount / 100) * 5;
        return calculateAmount;
    }

    private InvoiceDtoModel? GetInvoice(GetInvoiceRequestModel request)
    {
        var invoice = _invoiceRepository.Filter(x => x.IsActive == true && x.Id == request.InvoiceId).Select(x => new InvoiceDtoModel
        {
            Id = x.Id,
            IsGroceries = x.IsGroceries,
            TotalAmount = x.TotalAmount,
            CustomerId = x.CustomerId,
            NetAmount = 0.00M,
            DiscountAmount = 0.00M
        }).FirstOrDefault();

        return invoice;
    }
}