using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll(int pageNumber, int pageSize, string filterByName);
        UserResponse GetById(int id);
        UserResponse Create(CreateUserDTO user,string _imageFolderPath);
        UserResponse Update(int id,User user);
        User Delete(int id);
        User Authenticate(LoginDTO loginDto);

        string ResetPassword(ResetPasswordDTO resetPasswordDto);
    }
}
