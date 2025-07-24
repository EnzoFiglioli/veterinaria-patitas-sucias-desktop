using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Models;
using MiAppVeterinaria.Repository;
using System;
using System.ComponentModel;


namespace MiAppVeterinaria.Services
{
    class MascotaService : IMascotaService
    {
        public readonly MascotaRepository _mascotaRepository = new MascotaRepository();

        public MascotaService() { }

        public BindingList<Mascota> GetMascotas()
        {
            var mascotas = _mascotaRepository.ObtenerMascotas(); 
            var lista = new BindingList<Mascota>();

            foreach (var m in mascotas)
            {
                lista.Add(new Mascota
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Edad = m.Edad,
                    Especie = m.Especie,
                    Raza = m.Raza,
                    NombreCompletoDuenio= m.NombreCompletoDuenio,
                    Peso = m.Peso,
                    ContactoDuenio = m.ContactoDuenio
                });
            }

            return lista;
        }


        public string CreateMascota(MascotaDTO mascota)
        {
            try
            {
                if (mascota == null)
                {
                    return "Todos los campos son necesario";
                }
                _mascotaRepository.CrearMascota(mascota);
                return "Mascota creada exitosamente";
            }
            catch (Exception ex)
            {
                return "Ocurrió un error al crear la mascota: " + ex.Message;
            }
        }
        public string DeleteMascotaById(int idMascota) {
            return _mascotaRepository.EliminarMascotaPorId(idMascota);            
        }
        public string UpdateMascotaById(MascotaDTO m) {
            return _mascotaRepository.ActualizarMascota(m);
        }

        private string ObtenerNombreEspeciePorId(int especieId)
        {
            string[] especies = { "Perro", "Gato", "Ave", "Otro" };
            return especieId >= 0 && especieId < especies.Length ? especies[especieId] : "Desconocido";
        }

        private string ObtenerNombreDuenioPorId(int duenioId)
        {
            string[] duenios = { "Enzo Figlioli", "Florencia Iriarte", "Nicolás Iriarte", "Marcelo Iriarte" };
            return duenioId >= 0 && duenioId < duenios.Length ? duenios[duenioId] : "Desconocido";
        }
    }
}
