using GymManagment.Models;
using GymManagment.Models.DTOs;

namespace GymManagment.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll(int pageNumber, int pageSize, string filterByName);
        User GetById(int id);
        User Create(CreateUserDTO user);
        User Update(int id,User user);
        User Delete(int id);
    }
}
