using MiAppVeterinaria.DTO;
using MiAppVeterinaria.handlers;
using MiAppVeterinaria.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MiAppVeterinaria.Repository
{
    class MascotaRepository : IMascotaRepository
    {
        public MascotaRepository() { }

        public BindingList<Mascota> ObtenerMascotas()
        {
            BindingList<Mascota> listaMascotas = new BindingList<Mascota>();
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("ListarMascotas", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Mascota m = new Mascota
                                {
                                    Id = Convert.ToInt32(reader["ID"]),
                                    Nombre = reader["Mascota"].ToString(),
                                    Especie = reader["Especie"].ToString(),
                                    Raza = reader["Raza"].ToString(),
                                    Edad = Convert.ToInt32(reader["Edad"]),
                                    Peso = Convert.ToDecimal(reader["Peso"]),
                                    NombreCompletoDuenio = reader["Nombre y Apellido"].ToString(),
                                    ContactoDuenio = reader["Contacto"].ToString()
                                };

                                listaMascotas.Add(m);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listaMascotas;
        }

        public List<GetRazaEspecieDTO> ObtenerRazaAnimalId(int id)
        {
            List<GetRazaEspecieDTO> listarRazas = new List<GetRazaEspecieDTO>();
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();

                    var query = @"SELECT id_raza, nombre_raza, id_especie nombre_especie FROM raza rz INNER JOIN especie e ON e.id_especie = rz.especie WHERE e.id_especie = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GetRazaEspecieDTO r = new GetRazaEspecieDTO
                                {
                                    IdEspecie = Convert.ToInt32(reader["id_especie"]),
                                    NombreEspecie = reader["nombre_especie"].ToString(),
                                    IdRaza = Convert.ToInt32(reader["id_raza"]),
                                    NombreRaza = reader["nombre_raza"].ToString()
                                };
                                listarRazas.Add(r);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return listarRazas;
        }

        public string CrearMascota(MascotaDTO mascota)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("insertar_mascota", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@in_nombre_mascota", mascota.NombreMascota);
                        cmd.Parameters.AddWithValue("@in_especie", mascota.EspecieId);
                        cmd.Parameters.AddWithValue("@in_raza", mascota.Raza);
                        cmd.Parameters.AddWithValue("@in_edad", mascota.Edad);
                        cmd.Parameters.AddWithValue("@in_peso", mascota.Peso);
                        cmd.Parameters.AddWithValue("@in_duenio", mascota.DuenioId);

                        cmd.ExecuteNonQuery();
                    }
                }
                return "Mascota creada exitosamente.";
            }
            catch (MySqlException ex)
            {
                // Mostrar error de MySQL con detalle
                return $"Error MySQL: {ex.Message} Código: {ex.Number}";
            }
            catch (Exception ex)
            {
                // Error genérico
                return $"Error: {ex.Message}";
            }
        }

        public string EliminarMascotaPorId(int id)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("EliminarMascota", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@m_id", id);

                        MySqlParameter mensajeParam = new MySqlParameter("@v_message", MySqlDbType.VarChar, 100);
                        mensajeParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeParam);

                        cmd.ExecuteNonQuery();

                        return mensajeParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar: " + ex.Message;
            }
        }
        public string ActualizarMascota(MascotaDTO m)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Mascota 
                                    SET 
                                        nombre_mascota = @NombreMascota, 
                                        especie = @Especie, 
                                        raza = @Raza,
                                        peso = @Peso,
                                        edad = @Edad,
                                        dueño = @Dueño
                                    WHERE id_mascota = @MascotaId; ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NombreMascota", m.NombreMascota);
                        cmd.Parameters.AddWithValue("@Especie", m.EspecieId);
                        cmd.Parameters.AddWithValue("@Raza", m.Raza);
                        cmd.Parameters.AddWithValue("@Peso", m.Peso);
                        cmd.Parameters.AddWithValue("@Edad", m.Edad);
                        cmd.Parameters.AddWithValue("@Dueño", m.DuenioId);
                        cmd.Parameters.AddWithValue("@MascotaId", m.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
            return "Mascota actualizada correctamente";
        }
    }
}
