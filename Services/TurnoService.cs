using System;
using System.Collections.Generic;
using System.Text;
using MiAppVeterinaria.Models;
using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Repository;


namespace MiAppVeterinaria.Services
{
    class TurnoService : ITurnoService
    {
        private readonly TurnoRepository turnoRepository = new TurnoRepository();
        public List<TurnoAsignadoDTO> ObtenerTurnos()
        {
            return turnoRepository.ListarTurnos();
        }

        public string RegistrarTurno(TurnoDTO turnoDTO)
        {
            var resultado = turnoRepository.CrearTurno(turnoDTO);
            return resultado;
        }
        /*public List<TurnoDTO> ObtenerTurnosPorVeterinario(int idVeterinario)
        {
            return turnoRepository.ObtenerTurnosPorVeterinario(idVeterinario);
        }*/

    }
}
