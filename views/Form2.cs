using System;
using System.Drawing;
using System.Windows.Forms;
using MiAppVeterinaria.Views;
using MiAppVeterinaria.Services;


namespace MiAppVeterinaria
{
    public partial class Form2 : Form
    {
        private Button btnUsuarios;
        private Button btnMascotas;
        private Button btnConsultas;
        private Button btnTurnos;
        private Button btnReportes;
        private Button btnTurnosAsignados;
        private Button btnListarDuenios;
        private Button btnRegistrarMascota;
        private Button btnAsignarTurno;
        private Button btnHistoriaClinica;
        private string Rol;

        private readonly ITurnoService turnoService = new TurnoService();
        private readonly IMascotaService mascotaService = new MascotaService();

        public Form2(string rol)
        {
            this.Rol = rol;
            InitializeComponent();
            InicializarBotones();
            MostrarMenuPorRol(rol);
        }

        private void InicializarBotones()
        {
            btnUsuarios = CrearBoton("Usuarios", () => RenderizarEnPanel(new UsuariosView(Rol)));
            btnMascotas = CrearBoton("Mascotas", () => RenderizarEnPanel(new MascotasView(Rol)));
            btnConsultas = CrearBoton("Consultas", () => RenderizarEnPanel(new ConsultasView(Rol)));
            btnTurnos = CrearBoton("Turnos", () => RenderizarEnPanel(new AsignarTurnoView(Rol)));
            btnReportes = CrearBoton("Reportes", () => RenderizarEnPanel(new ReportesView(Rol,turnoService,mascotaService)));
            btnHistoriaClinica = CrearBoton("Historial Clinico", () => RenderizarEnPanel(new ReportesView(Rol,turnoService,mascotaService)));

            btnRegistrarMascota = CrearBoton("Registrar Mascota", () => RenderizarEnPanel(new RegistrarMascotaView(Rol)));
            btnAsignarTurno = CrearBoton("Asignar Turno", () => RenderizarEnPanel(new AsignarTurnoView(Rol)));
            btnTurnosAsignados = CrearBoton("Turnos Asignados", () => RenderizarEnPanel(new TurnosAsignadosView(Rol)));
            btnListarDuenios = CrearBoton("Listar Dueños", () => RenderizarEnPanel(new ListarDueniosView(Rol)));
        }


        private void RenderizarEnPanel(Control control)
        {
            mainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(control);
        }
        private Button CrearBoton(string texto, Action onClick = null)
        {
            var btn = new Button
            {
                Text = texto,
                Width = 210,
                Height = 40,
                Margin = new Padding(10, 5, 10, 5),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Gainsboro,
                ForeColor = Color.Black
            };

            if (onClick != null)
                btn.Click += (s, e) => onClick();

            return btn;
        }


        private void MostrarMenuPorRol(string rol)
        {
            navbar.SuspendLayout();
            navbar.Controls.Clear();
            navbar.Controls.Add(pictureBox1); // Volvés a agregar el logo

            switch (rol)
            {
                case "Administracion":
                    AgregarBotones(btnUsuarios, btnMascotas, btnTurnos, btnReportes);
                    break;
                case "Veterinario":
                    AgregarBotones(btnConsultas, btnMascotas, btnTurnosAsignados);
                    break;
                case "Recepcion":
                    AgregarBotones(btnRegistrarMascota, btnAsignarTurno, btnListarDuenios);
                    break;
            }

            navbar.ResumeLayout();
        }

        private void AgregarBotones(params Button[] botones)
        {
            foreach (var btn in botones)
                navbar.Controls.Add(btn);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }
    }
}
