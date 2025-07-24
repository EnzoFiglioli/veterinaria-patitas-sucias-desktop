using MiAppVeterinaria.DTO;
using System.Collections.Generic;

namespace MiAppVeterinaria.Repository
{
    interface IConsultaRepository
    {
        void CreateConsulta(ConsultaDTO consulta);
        List<ConsultaDTO> ListarConsultas();
        string EliminarConsulta(int idConsulta);
        string ActualizarConsulta(ConsultaDTO consultaDTO);
    }
}
