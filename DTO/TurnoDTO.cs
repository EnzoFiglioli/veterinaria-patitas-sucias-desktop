using System;
using System.Collections.Generic;
using System.Text;

namespace MiAppVeterinaria.DTO
{
    public class TurnoDTO
    {
        public int IdMascota {get; set;}
        public int IdVeterinario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }

        public TurnoDTO() { }

        public TurnoDTO(int idMascota, int idVeterinario, DateTime fechaHora, string motivo)
        {
            IdMascota = idMascota;
            IdVeterinario = idVeterinario;
            FechaHora = fechaHora;
            Motivo = motivo;
        }
    }
}
