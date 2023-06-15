using Business.Abstract;
using Entity.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Counter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(InvoiceAddDto invoiceAddDto)
        {
            var result =await _invoiceService.AddInvoice(invoiceAddDto);
            return Ok(result);
        }
    }
}
