using GymManagement.Services;
using GymManagment.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, 
            [FromQuery] int? month = null,
            [FromQuery] int? year = null,
            [FromQuery] int? userId=null
            )
        {
            var payments = _paymentService.GetAll(pageNumber, pageSize, month, year, userId);
            return Ok(payments);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var payment = _paymentService.GetById(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Payment payment)
        {
            _paymentService.Create(payment);
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, payment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Payment payment)
        {
            _paymentService.Update(id, payment);
            return Ok(payment);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Payment payment=_paymentService.Delete(id);
            return Ok(payment);
        }
    }
}
