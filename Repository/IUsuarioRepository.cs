using System;
using System.Collections.Generic;
using System.Text;

using MiAppVeterinaria.DTO;

namespace MiAppVeterinaria.Repository
{
    interface IUsuarioRepository
    {
        List<UserDTO> GetAllUsers();
        UserDTO GetUserById(int id);
        void CreateUser(UserDTO userDto);
        void UpdateUser(UserDTO userDto);
        void DeleteUser(int id);
    }
}
