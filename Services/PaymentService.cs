using GymManagement.Services;
using GymManagment.Connection;
using GymManagment.Exceptions;
using GymManagment.Models;

namespace GymManagment.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDBContext _context;

        public PaymentService(ApplicationDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Payment> GetAll(int pageNumber, int pageSize, int? month, int? year, int? userId)
        {
            IQueryable<Payment> payments = _context.Payments.AsQueryable();

            if (month.HasValue && year.HasValue)
            {
                payments = payments.Where(p => p.Date.Month == month.Value && p.Date.Year == year.Value);
            }

            if (userId.HasValue)
            {
                payments = payments.Where(p => p.UserId == userId.Value);
            }
            return payments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public Payment GetById(int id)
        {
            Payment payment = _context.Payments.Find(id);
            if (payment == null)
                throw new NotFoundException("Payment not found");

            return payment;
        }

        public Payment Create(Payment payment)
        {
            payment.ExpireAt = payment.Date.AddMonths(1);

            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }

        public Payment Update(int id,Payment payment)
        {
            Payment existedPayment = _context.Payments.Find(id);

            if (existedPayment is null)
                throw new NotFoundException("Payment not found");

            payment.ExpireAt = payment.Date.AddMonths(1);

            _context.Payments.Update(payment);
            _context.SaveChanges();
            return payment;
        }

        public Payment Delete(int id)
        {
            Payment payment = _context.Payments.Find(id);
            if (payment == null)
                throw new NotFoundException("Payment not found");
           
                _context.Payments.Remove(payment);
                _context.SaveChanges();
            return payment;
        }
    }
}
