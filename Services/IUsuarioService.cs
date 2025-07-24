using MiAppVeterinaria.DTO;
using System.Collections.Generic;

namespace MiAppVeterinaria.Services
{
    public interface IUserService
    {
        List<UserDTO> GetAllUsers();
        UserDTO GetUserById(int id);
        void CreateUser(UserDTO userDto);
        void UpdateUser(UserDTO userDto);
        void DeleteUser(int id);
    }
}
