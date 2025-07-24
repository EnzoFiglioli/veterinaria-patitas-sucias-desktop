using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Models;
using System.Collections.Generic;
using System.ComponentModel;



namespace MiAppVeterinaria.Services
{
    public interface IMascotaService
    {
        BindingList<Mascota> GetMascotas();
        string CreateMascota(MascotaDTO mascota);
        string DeleteMascotaById(int IdMascota);
        string UpdateMascotaById(MascotaDTO m);
    }
}
