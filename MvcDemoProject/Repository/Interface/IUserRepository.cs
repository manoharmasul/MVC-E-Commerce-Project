using MvcDemoProject.Models;

namespace MvcDemoProject.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<int> AddNewUser(UsertRegistrationModel userRegistration);
        Task<User> UserlogIn(logUserModel userlogin);
        Task<int> UpdateUser(User user);
        Task<int> DeleteUser(int id);
        Task<User> GetById(int id);

    }
}
