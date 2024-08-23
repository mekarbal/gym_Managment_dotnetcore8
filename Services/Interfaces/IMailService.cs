using GymManagment.Models;

namespace GymManagment.Services.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(Email mailRequest);

}