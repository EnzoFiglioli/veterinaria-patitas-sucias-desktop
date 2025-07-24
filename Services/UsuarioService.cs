using System;
using System.Collections.Generic;
using System.Text;

using MiAppVeterinaria.Services;
using MiAppVeterinaria.Repository;
using MiAppVeterinaria.DTO;


namespace MiAppVeterinaria.Services
{
    class UsuarioService : IUserService
    {
        private readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        public UsuarioService() { }
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<UserDTO> GetAllUsers() {
            var usuarios = _usuarioRepository.GetAllUsers();
            return usuarios;
        }
        public UserDTO GetUserById(int id) {
            var usuario = new UserDTO();
            return usuario;
        }
        public void CreateUser(UserDTO userDto) { }
        public void UpdateUser(UserDTO userDto) { }
        public void DeleteUser(int id) { }
    }
}
