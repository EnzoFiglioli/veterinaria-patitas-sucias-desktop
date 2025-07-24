namespace MiAppVeterinaria.DTO
{
    public class MascotaDTO
    {
        public int Id { get; set; }
        public string NombreMascota { get; set; }

        public int EspecieId { get; set; }
        public string EspecieNombre { get; set; }

        public string Raza { get; set; }

        public int Edad { get; set; }

        public decimal Peso { get; set; }

        public int DuenioId { get; set; }
        public string DuenioNombre { get; set; }

        public MascotaDTO() { }

        public MascotaDTO(int id, string nombreMascota, int especieId, string especieNombre, string raza, int edad, decimal peso, int duenioId, string duenioNombre)
        {
            Id = id;
            NombreMascota = nombreMascota;
            EspecieId = especieId;
            EspecieNombre = especieNombre;
            Raza = raza;
            Edad = edad;
            Peso = peso;
            DuenioId = duenioId;
            DuenioNombre = duenioNombre;
        }

        public override string ToString()
        {
            return $"ID: {Id}\nNombre: {NombreMascota}\nEspecie: {EspecieNombre} (ID: {EspecieId})\nRaza: {Raza}\nEdad: {Edad}\nPeso: {Peso}\nDueño: {DuenioNombre} (ID: {DuenioId})";
        }


    }
}
