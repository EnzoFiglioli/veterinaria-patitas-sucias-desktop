using System.Collections.Generic;
using MiAppVeterinaria.Models;
using MiAppVeterinaria.DTO;

namespace MiAppVeterinaria.Services
{
    public interface ITurnoService
    {
        List<TurnoAsignadoDTO> ObtenerTurnos();
        string RegistrarTurno(TurnoDTO turnoDTO);
    }
}
