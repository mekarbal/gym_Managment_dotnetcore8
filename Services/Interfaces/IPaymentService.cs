
using GymManagment.Models;

namespace GymManagement.Services
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetAll(int pageNumber, int pageSize, int? month, int? year, int? userId);
        Payment GetById(int id);
        Payment Create(Payment payment);
        Payment Update(int id,Payment payment);
        Payment Delete(int id);
    }
}
