using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Repository;


namespace MiAppVeterinaria.Views
{
    public class TurnosAsignadosView : UserControl
    {
        private List<TurnoAsignadoDTO> listaTurnos = new List<TurnoAsignadoDTO>();
        private DataGridView dgvTurnos;
        private string Rol = null;

        private readonly TurnoRepository turnoRepository = new TurnoRepository();

        public TurnosAsignadosView(string rol)
        {
            Rol = rol; 
            InicializarComponentes();
            CargarTurnosSimulados();
        }

        private void InicializarComponentes()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.White;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 2,
                ColumnCount = 1,
                AutoScroll = true
            };

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // header
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // tabla

            layout.Controls.Add(ConstruirHeader(), 0, 0);
            layout.Controls.Add(ConstruirTabla(), 0, 1);

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
                Text = "Turnos Asignados",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true
            });

            panel.Controls.Add(new Label
            {
                Text = "Listado de turnos veterinarios ya asignados.",
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            });

            return panel;
        }

        private Control ConstruirTabla()
        {
            dgvTurnos = new DataGridView
            {
                Dock = DockStyle.Fill,
                Height = 350,
                BackgroundColor = Color.WhiteSmoke,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };

            dgvTurnos.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvTurnos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvTurnos.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumPurple;
            dgvTurnos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTurnos.EnableHeadersVisualStyles = false;

            return dgvTurnos;
        }

        private void CargarTurnosSimulados()
        {
            listaTurnos = turnoRepository.ListarTurnos();
            dgvTurnos.DataSource = listaTurnos;
        }
    }
}
