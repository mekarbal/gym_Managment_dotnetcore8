using System.Net;
using GymManagment.Connection;
using GymManagment.Exceptions;
using GymManagment.Identity;
using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Models.Enums;
using GymManagment.Services.Interfaces;
using GymManagment.Shared;

namespace GymManagment.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMailService _mailService;
        public UserService(ApplicationDBContext context,IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        public UserResponse Create(CreateUserDTO userDto, string _imageFolderPath)
        {
            try
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
                if (userDto.Image != null && userDto.Image.Length > 0)
                {
                    var imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(userDto.Image.FileName)}";
                    var imagePath = Path.Combine(_imageFolderPath, imageFileName);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        userDto.Image.CopyToAsync(stream);
                    }

                    user.Image = imageFileName;
                }

                _context.Users.Add(user);
                _context.SaveChanges();
               
                _mailService.SendEmailAsync(new Email{
                    ToEmail=userDto.Email,
                    Subject="New Subscription",
                    Body=$"Your password is {user.Password}"
                });
                UserResponse userResponse = new UserResponse();
                userResponse.Email = user.Email;
                userResponse.Id = user.Id;
                userResponse.Name = user.Name;
                userResponse.Password = user.Password;
                userResponse.Role = user.Role == Role.ADMIN ? "Admin" : "User";
                userResponse.Image = user.Image;
                return userResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public User Delete(int id)
        {
            try
            {
                User user = _context.Users.Find(id);
                if (user == null)
                    throw new NotFoundException("User not found");

                _context.Users.Remove(user);
                _context.SaveChanges();
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public User Authenticate(LoginDTO loginDto)
        {
            try
            {
                var passwordHelper = new PasswordHelper();
                
                var user = _context.Users.SingleOrDefault(x => x.Email == loginDto.Email);

                var validPAssword = passwordHelper.VerifyPassword(user.Password, loginDto.Password);
                if (user is null || !validPAssword)
                {
                    throw new NotFoundException("Email or password Incorrect");
                }
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }




        public IEnumerable<User> GetAll(int pageNumber, int pageSize, string filterByName)
        {
            try
            {
                IQueryable<User> users = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(filterByName))
                {
                    users = users.Where(u => u.Name.Contains(filterByName));
                }

                return users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public UserResponse GetById(int id)
        {
            try
            {
                User user = _context.Users.Find(id);

                if (user == null)
                    throw new NotFoundException("User not found");

                UserResponse userResponse = new UserResponse();
                userResponse.Email = user.Email;
                userResponse.Id = user.Id;
                userResponse.Name = user.Name;
                userResponse.Password = user.Password;
                userResponse.Role = user.Role == Role.ADMIN ? "Admin" : "User";
                userResponse.Image = user.Image;
                return userResponse;
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public UserResponse Update(int id, User user)
        {
            try
            {
                User exisxtedUser = _context.Users.Find(id);

                if (exisxtedUser == null)
                    throw new NotFoundException("User not found");

                _context.Users.Update(user);
                _context.SaveChanges();
                UserResponse userResponse = new UserResponse();
                userResponse.Email = user.Email;
                userResponse.Id = user.Id;
                userResponse.Name = user.Name;
                userResponse.Password = user.Password;
                userResponse.Role = user.Role == Role.ADMIN ? "Admin" : "User";
                return userResponse;
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public string ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                PasswordHelper passwordHelper = new PasswordHelper();
                User user = _context.Users.FirstOrDefault(user => user.Email == resetPasswordDto.Email);

                if (user is null)
                {
                    throw new NotFoundException("Email Not Found");
                }

                string passwordHashed = passwordHelper.HashPassword(resetPasswordDto.Password);
                user.Password = passwordHashed;
                _context.Users.Update(user);
                _context.SaveChanges();
                return "Password changed successfully";
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}