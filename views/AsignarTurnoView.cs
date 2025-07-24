using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Models;
using MiAppVeterinaria.Repository;
using MiAppVeterinaria.Services;
using MiAppVeterinaria.Repository;

namespace MiAppVeterinaria.Views
{
    public class AsignarTurnoView : UserControl
    {
        private ComboBox cmbMascota, cmbVeterinario;
        private DateTimePicker dtpFechaHora;
        private TextBox txtMotivo;
        private Button btnGuardar;
        private DataGridView dgvTurnos;
        private ListBox lstTurnosOcupados;

        private readonly TurnoService turnoService = new TurnoService();
        private readonly MascotaRepository mascotaRepo = new MascotaRepository();
        private readonly VeterinarioRepository veterinarioRepo = new VeterinarioRepository();
        private List<TurnoAsignadoDTO> turnos = new List<TurnoAsignadoDTO>();

        public AsignarTurnoView(string Rol)
        {
            InicializarVista();
        }

        private void InicializarVista()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 3,
                ColumnCount = 1,
                AutoScroll = true
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            layout.Controls.Add(ConstruirHeader(), 0, 0);
            layout.Controls.Add(ConstruirFormulario(), 0, 1);
            layout.Controls.Add(ConstruirTabla(), 0, 2);

            this.Controls.Add(layout);
        }

        private Control ConstruirHeader()
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 10)
            };

            panel.Controls.Add(new Label
            {
                Text = "Asignación de Turnos",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.Teal,
                AutoSize = true
            });

            panel.Controls.Add(new Label
            {
                Text = "Seleccioná la mascota, veterinario y fecha para asignar el turno.",
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            });

            return panel;
        }

        private Control ConstruirFormulario()
        {
            var contenedor = new TableLayoutPanel
            {
                ColumnCount = 2,
                Dock = DockStyle.Top,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10)
            };

            contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            cmbMascota = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = mascotaRepo.ObtenerMascotas(),
                DisplayMember = "MascotaDuenioNombre",
                ValueMember = "Id"
            };

            cmbVeterinario = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = veterinarioRepo.Obtener(),
                DisplayMember = "NombreCompleto",
                ValueMember = "Id"
            };

            dtpFechaHora = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                Width = 250,
                MinDate = DateTime.Now
            };
            lstTurnosOcupados = new ListBox
            {
                Width = 250,
                Height = 80
            };

            dtpFechaHora.ValueChanged += ActualizarTurnosOcupados;
            cmbVeterinario.SelectedIndexChanged += ActualizarTurnosOcupados;

            contenedor.Controls.Add(new Label { Text = "Turnos ya tomados:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 4);
            contenedor.Controls.Add(lstTurnosOcupados, 1, 4);

            txtMotivo = new TextBox { Width = 250, Height = 60, Multiline = true };

            btnGuardar = new Button
            {
                Text = "Asignar turno",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 150
            };
            btnGuardar.Click += GuardarTurno;

            contenedor.Controls.Add(new Label { Text = "Mascota:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            contenedor.Controls.Add(cmbMascota, 1, 0);
            contenedor.Controls.Add(new Label { Text = "Veterinario:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            contenedor.Controls.Add(cmbVeterinario, 1, 1);
            contenedor.Controls.Add(new Label { Text = "Fecha y Hora:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            contenedor.Controls.Add(dtpFechaHora, 1, 2);
            contenedor.Controls.Add(new Label { Text = "Motivo:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            contenedor.Controls.Add(txtMotivo, 1, 3);

            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Margin = new Padding(0, 10, 0, 0)
            };
            panel.Controls.Add(btnGuardar);

            var panelContenedor = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            panelContenedor.Controls.Add(contenedor);
            panelContenedor.Controls.Add(panel);

            return panelContenedor;
        }

        private Control ConstruirTabla()
        {
            dgvTurnos = new DataGridView
            {
                Height = 300,
                Dock = DockStyle.Top,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvTurnos.DataSource = turnoService.ObtenerTurnos();

            return dgvTurnos;
        }

        private void GuardarTurno(object sender, EventArgs e)
        {
            if (cmbMascota.SelectedIndex == -1 || cmbVeterinario.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtMotivo.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var turnoDTO = new TurnoDTO
            {
                IdMascota = (int)cmbMascota.SelectedValue,
                IdVeterinario = (int)cmbVeterinario.SelectedValue,
                FechaHora = dtpFechaHora.Value,
                Motivo = txtMotivo.Text.Trim()
            };

            string msg = turnoService.RegistrarTurno(turnoDTO);
            MessageBox.Show(msg);

            dgvTurnos.DataSource = null;
            dgvTurnos.DataSource = turnoService.ObtenerTurnos();

            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            cmbMascota.SelectedIndex = 0;
            cmbVeterinario.SelectedIndex = 0;
            dtpFechaHora.Value = DateTime.Now;
            txtMotivo.Clear();
        }

        private void ActualizarTurnosOcupados(object sender, EventArgs e)
        {
            lstTurnosOcupados.Items.Clear();

            if (cmbVeterinario.SelectedIndex == -1)
                return;

            int idVet = (int)cmbVeterinario.SelectedValue;
            DateTime fechaSeleccionada = dtpFechaHora.Value.Date;

            // var turnos = turnoService.ObtenerTurnosPorVeterinario(idVet);
            /*
            foreach (var turno in turnos)
            {
                if (turno.FechaHora.Date == fechaSeleccionada)
                {
                    lstTurnosOcupados.Items.Add(turno.FechaHora.ToString("HH:mm") + " - " + turno.Motivo);
                }
            }
            */
            if (lstTurnosOcupados.Items.Count == 0)
                lstTurnosOcupados.Items.Add("Sin turnos asignados");
        }

    }
}
