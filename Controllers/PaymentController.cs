using System.Net;
using GymManagement.Services;
using GymManagment.Exceptions;
using GymManagment.Models;
using GymManagment.Shared;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10, 
            [FromQuery] int? month = null,
            [FromQuery] int? year = null,
            [FromQuery] int? userId=null
            )
        {
            try
            {
                var payments = _paymentService.GetAll(pageNumber, pageSize, month, year, userId);
                return Ok(new Response<IEnumerable<Payment>>("ALL PAYMENTS", payments, (int)HttpStatusCode.Accepted));            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public IActionResult GetById(int id)
        {
            try
            {
                Payment payment = _paymentService.GetById(id);
                if (payment == null)
                    return NotFound();

                return Ok(new Response<Payment>("RETREIVED PAYEMNT", payment, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
            
        
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Create([FromBody] Payment payment)
        {
            try
            {
                Payment ExistedPayment = _paymentService.Create(payment);
                
                return Ok(new Response<Payment>("Payment Created", ExistedPayment, (int)HttpStatusCode.Created));

            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Update(int id, [FromBody] Payment payment)
        {
            try
            {
                Payment paymentUpdated = _paymentService.Update(id, payment);
                return Ok(new Response<Payment>("Payment Updated Successfully", paymentUpdated,
                    (int)HttpStatusCode.Created));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int id)
        {
            try
            {
                Payment payment = _paymentService.Delete(id);
                return Ok(new Response<Payment>("Payment Updated Successfully", payment, (int)HttpStatusCode.Created));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
