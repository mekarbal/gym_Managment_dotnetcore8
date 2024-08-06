using GymManagment.Connection;
using GymManagment.Exceptions;
using GymManagment.Identity;
using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Services.Interfaces;

namespace GymManagment.Services
{
    public class UserService : IUserService
    {

        private readonly ApplicationDBContext _context;

        public UserService(ApplicationDBContext context) {
            _context = context;
        }
        public User Create(CreateUserDTO userDto)
        {
            var passwordHelper = new PasswordHelper();

            string password = userDto.Password is not null
                ? userDto.Password
                : passwordHelper.GenerateRandomPassword();
            string hashedPassword = passwordHelper.HashPassword(password);

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = hashedPassword,
                Role = userDto.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Delete(int id)
        {
            User user = _context.Users.Find(id);
            if (user == null)
                throw new NotFoundException("User not found");
          
                _context.Users.Remove(user);
                _context.SaveChanges();
            return user;
        }

        public IEnumerable<User> GetAll(int pageNumber, int pageSize, string filterByName)
        {

            IQueryable<User> users = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filterByName))
            {
                users = users.Where(u => u.Name.Contains(filterByName));
            }

            return users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public User GetById(int id)
        {
            User user = _context.Users.Find(id);

            if (user == null)
                throw new NotFoundException("User not found");

            return user;

        }

        public User Update(int id,User user)
        {
            User exisxtedUser = _context.Users.Find(id);

            if (exisxtedUser == null)
                throw new NotFoundException("User not found");

            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
