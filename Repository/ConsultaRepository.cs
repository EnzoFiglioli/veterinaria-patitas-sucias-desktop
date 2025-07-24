using MiAppVeterinaria.DTO;
using MiAppVeterinaria.handlers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;


namespace MiAppVeterinaria.Repository
{
    public class ConsultaRepository : IConsultaRepository
    {
        public void CreateConsulta(ConsultaDTO consulta)
        {
            try
            {
                if (consulta == null)
                    throw new ArgumentNullException(nameof(consulta));

                if (consulta.MascotaId <= 0)
                    throw new ArgumentException("MascotaId inválido.");

                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Consulta(MascotaId, Sintoma, Emergencia, Veterinario) VALUES(@i_MascotaID, @i_Sintoma, @i_Emergencia, @i_Veterinario)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@i_MascotaID", consulta.MascotaId);
                        cmd.Parameters.AddWithValue("@i_Sintoma", consulta.Sintomas);
                        cmd.Parameters.AddWithValue("@i_Emergencia", consulta.Emergencia);
                        cmd.Parameters.AddWithValue("@i_Veterinario", consulta.VeterinarioId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear consulta: {ex.Message}");
                throw;
            }
        }
        public List<ConsultaDTO> ListarConsultas()
        {
            var listaHistorial = new List<ConsultaDTO>();
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();

                    string query = "SELECT c.ConsultaId, c.Sintoma,c.FechaConsulta,c.Emergencia, CONCAT(v.Nombre, ' ', v.Apellido )AS Veterinario,m.id_mascota AS MascotaId, m.nombre_mascota AS Mascota, c.Veterinario AS VeterinarioId FROM Consulta c INNER JOIN Mascota m ON m.id_mascota = c.MascotaId INNER JOIN veterinario v ON v.VeterinarioId = c.veterinario";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader()){
                            while (reader.Read())
                            {
                                ConsultaDTO c = new ConsultaDTO
                                { 
                                    Id = Convert.ToInt32(reader["ConsultaId"]),
                                    Sintomas = reader["Sintoma"].ToString(),
                                    Fecha = Convert.ToDateTime(reader["FechaConsulta"]),
                                    Veterinario = reader["Veterinario"].ToString(),
                                    VeterinarioId = Convert.ToInt32(reader["VeterinarioId"]),
                                    Emergencia = Convert.ToBoolean(reader["Emergencia"]),
                                    MascotaId = Convert.ToInt32(reader["MascotaId"]), 
                                    Mascota = reader["Mascota"].ToString(),
                                };
                                listaHistorial.Add(c);
                            }
                        }
                    }
                }
                return listaHistorial;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public string EliminarConsulta(int idConsulta)
        {
            try
            {
                using (MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    System.Diagnostics.Debug.WriteLine("[INFO] Creando conexión...");
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("[INFO] Conexión abierta.");

                    string query = "DELETE FROM consulta WHERE ConsultaId = @idConsulta";
                    System.Diagnostics.Debug.WriteLine("[SQL] " + query);
                    System.Diagnostics.Debug.WriteLine("[PARAM] @idConsulta = " + idConsulta);

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idConsulta", idConsulta);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine("[RESULT] Filas afectadas: " + rowsAffected);

                        if (rowsAffected == 0)
                            return "No se encontró la consulta a eliminar.";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR] " + ex.Message);
                return $"Error al eliminar: {ex.Message}";
            }

            return "Consulta eliminada con éxito.";
        }
        public string ActualizarConsulta(ConsultaDTO c)
        {
            try
            {
                using(MySqlConnection conn = DBConnection.GetInstance().CreateConnection())
                {
                    conn.Open();
                    string query = @"UPDATE Consulta 
                                    SET MascotaId = @MascotaId, 
                                        Sintoma = @Sintoma, 
                                        Veterinario = @Veterinario, 
                                        FechaConsulta = @FechaConsulta  
                                    WHERE ConsultaId = @id; ";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn)){
                        cmd.Parameters.AddWithValue("@MascotaId", c.MascotaId);
                        cmd.Parameters.AddWithValue("@Sintoma", c.Sintomas);
                        cmd.Parameters.AddWithValue("@Veterinario", c.Veterinario);
                        cmd.Parameters.AddWithValue("@FechaConsulta", c.Fecha);
                        cmd.Parameters.AddWithValue("@id", c.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                return $"{ex.Message}";
            }
            return "Consulta actualizada correctamente";
        }
    }
}
