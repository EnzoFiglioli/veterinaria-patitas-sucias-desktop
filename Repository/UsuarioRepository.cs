using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

using MiAppVeterinaria.handlers;
using MiAppVeterinaria.DTO;

namespace MiAppVeterinaria.Repository
{
    class UsuarioRepository : IUsuarioRepository
    {
        public List<UserDTO> GetAllUsers()
        {
            var listaUsers = new List<UserDTO>();
            try
            {
            using(MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
            {
                conn.Open();
                string query = "SELECT id_usuario AS ID, nombre, apellido, rolname as rol, username FROM usuario INNER JOIN rol r ON r.id_rol = rol";
                using(MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserDTO u = new UserDTO
                            {
                                Id = Convert.ToInt32(reader["ID"]),
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Rol = reader["rol"].ToString(),
                                Username = reader["username"].ToString()
                            };
                            listaUsers.Add(u);
                        }
                    }
                }
            }
            }
            catch(Exception ex)
            {
                throw ex;
            }

                return listaUsers;
        }

        public UserDTO GetUserById(int id)
        {
            var usuario = new UserDTO();
            return usuario;
        }
        public void  CreateUser(UserDTO userDto)
        {

        }
        public void UpdateUser(UserDTO userDto)
        {

        }
        public void DeleteUser(int id)
        {

        }
    }
}
