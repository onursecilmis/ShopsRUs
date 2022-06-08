using ShopsRUs.Services.DTO;
using ShopsRUs.Services.Models;

namespace ShopsRUs.Services.Interfaces;

public interface IInvoiceService
{
    InvoiceDtoModel? GetInvoiceCalculate(GetInvoiceRequestModel request);
}