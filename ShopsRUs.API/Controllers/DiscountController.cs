using Microsoft.AspNetCore.Mvc;
using ShopsRUs.Services.Interfaces;
using ShopsRUs.Services.Models;

namespace ShopsRUs.API.Controllers;

[Route("")]
public class DiscountController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public DiscountController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Discount API");
    }

    [HttpPost]
    public IActionResult GetInvoiceDiscount(GetInvoiceRequestModel request)
    {
       var invoice =  _invoiceService.GetInvoiceCalculate(request);
        return Ok(invoice);
    }
}