using System;
using System.Drawing;
using System.Windows.Forms;
using MiAppVeterinaria.Services;

namespace MiAppVeterinaria.Views
{
    public class ReportesView : UserControl
    {
        private Label lblTitulo;
        private Label lblUsuarios;
        private Label lblTurnos;
        private Label lblMascotas;

        private ITurnoService _turnoService;
        private IMascotaService _mascotaService;

        private string Rol = null;

        public ReportesView(string rol, ITurnoService turnoService, IMascotaService mascotaService)
        {
            this.Rol = rol; 
            _turnoService = turnoService;
            _mascotaService = mascotaService;

            InicializarComponentes();
            CargarDatos();
        }

        private void InicializarComponentes()
        {
            lblTitulo = new Label
            {
                Text = "Reportes Generales",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblUsuarios = new Label
            {
                Text = "Usuarios: ",
                Location = new Point(40, 80),
                Font = new Font("Segoe UI", 12),
                AutoSize = true
            };

            lblTurnos = new Label
            {
                Text = "Turnos asignados: ",
                Location = new Point(40, 120),
                Font = new Font("Segoe UI", 12),
                AutoSize = true
            };

            lblMascotas = new Label
            {
                Text = "Mascotas registradas: ",
                Location = new Point(40, 160),
                Font = new Font("Segoe UI", 12),
                AutoSize = true
            };

            this.Controls.Add(lblTitulo);
            this.Controls.Add(lblUsuarios);
            this.Controls.Add(lblTurnos);
            this.Controls.Add(lblMascotas);
        }

        private void CargarDatos()
        {
            var cantidadTurnos = _turnoService.ObtenerTurnos().Count;
            var cantidadMascotas = _mascotaService.GetMascotas().Count;

            lblTurnos.Text = $"Turnos asignados: {cantidadTurnos}";
            lblMascotas.Text = $"Mascotas registradas: {cantidadMascotas}";
        }
    }
}
