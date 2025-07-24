using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Repository;


namespace MiAppVeterinaria.Views
{
    class ConsultasView : UserControl
    {
        private string Rol;

        private ComboBox txtMascota;
        private DateTimePicker txtFecha;
        private TextBox txtSintoma;
        private ComboBox txtVeterinario;

        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnEliminar;

        private DataGridView dgvConsultas;
        private int? selectedRow = null;
        private int? idConsulta = null;

        private readonly ConsultaRepository consultaRepository = new ConsultaRepository();
        private readonly MascotaRepository mascotaRepository = new MascotaRepository();
        private readonly VeterinarioRepository veterinarioRepository = new VeterinarioRepository();
        
        private List<ConsultaDTO> listaConsultas = new List<ConsultaDTO>();

        public ConsultasView(string rol)
        {
            var data = consultaRepository.ListarConsultas();

            foreach (var consulta in data)
            {
                listaConsultas.Add(consulta);
            }

            this.Rol = rol;
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

            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Header
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Formulario
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Tabla

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
                Text = "Gestión de Consultas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true
            });

            panel.Controls.Add(new Label
            {
                Text = "Podés registrar, modificar o eliminar consultas según tu rol.",
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

            contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
            contenedor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            txtMascota = new ComboBox { Width = 250 };
            var listaMascota = mascotaRepository.ObtenerMascotas();
            txtMascota.DataSource = listaMascota;
            txtMascota.DisplayMember = "MascotaDuenioNombre";
            txtMascota.ValueMember = "ID";

            txtMascota.DropDownStyle = ComboBoxStyle.DropDownList;
            txtFecha = new DateTimePicker { Width = 250, Format = DateTimePickerFormat.Short };
            txtSintoma = new TextBox { Width = 250, Multiline = true, Height = 60 };
            txtVeterinario = new ComboBox { Width = 250 };
            var listaVetes = veterinarioRepository.Obtener();
            txtVeterinario.DataSource = listaVetes;
            txtVeterinario.DropDownStyle = ComboBoxStyle.DropDownList;
            txtVeterinario.DisplayMember = "NombreApellido";
            txtVeterinario.ValueMember = "Id";

            contenedor.Controls.Add(new Label { Text = "Mascota:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            contenedor.Controls.Add(txtMascota, 1, 0);
            contenedor.Controls.Add(new Label { Text = "Fecha:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            contenedor.Controls.Add(txtFecha, 1, 1);
            contenedor.Controls.Add(new Label { Text = "Síntoma:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            contenedor.Controls.Add(txtSintoma, 1, 2);
            contenedor.Controls.Add(new Label { Text = "Veterinario:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            contenedor.Controls.Add(txtVeterinario, 1, 3);

            var panelBotones = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                Margin = new Padding(0, 10, 0, 0),
                AutoSize = true
            };

            btnGuardar = new Button
            {
                Text = "Guardar",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btnGuardar.Click += GuardarConsulta;

            btnActualizar = new Button
            {
                Text = "Actualizar",
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btnActualizar.Click += ActualizarConsulta;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btnEliminar.Click += EliminarConsulta;

            // Rol-based logic
            if (Rol == "Veterinario")
                panelBotones.Controls.AddRange(new[] { btnGuardar, btnActualizar, btnEliminar });
            else if (Rol == "Administracion")
                panelBotones.Controls.AddRange(new[] { btnActualizar, btnEliminar });

            var panelContenedor = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            panelContenedor.Controls.Add(contenedor);
            panelContenedor.Controls.Add(panelBotones);

            return panelContenedor;
        }

        private Control ConstruirTabla()
        {
            dgvConsultas = new DataGridView
            {
                Height = 300,
                Dock = DockStyle.Top,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvConsultas.DataSource = listaConsultas;
            if (dgvConsultas.Columns.Contains("VeterinarioId"))
            {
                dgvConsultas.Columns["VeterinarioId"].Visible = false;
            }


            dgvConsultas.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedRow = e.RowIndex; 

                    var consulta = (ConsultaDTO)dgvConsultas.Rows[e.RowIndex].DataBoundItem;

                    txtSintoma.Text = consulta.Sintomas;
                    MessageBox.Show("Asignando VeterinarioId: " + consulta.VeterinarioId);
                    txtVeterinario.SelectedValue = consulta.VeterinarioId;
                    txtFecha.Value = consulta.Fecha;
                    txtMascota.SelectedValue = consulta.MascotaId;
                }
            };

            return dgvConsultas;
        }

        private void GuardarConsulta(object sender, EventArgs e)
        {
            if (txtMascota.SelectedValue == null || txtVeterinario.SelectedValue == null)
            {
                MessageBox.Show("Seleccioná una mascota y un veterinario válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtMascota.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccioná una mascota.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (ValidarFormulario())
            {
                ConsultaDTO c = new ConsultaDTO
                { 
                    Emergencia = false, 
                    Fecha = txtFecha.Value , 
                    MascotaId = (int)txtMascota.SelectedValue, 
                    Sintomas = txtSintoma.Text,   
                    VeterinarioId = (int)txtVeterinario.SelectedValue,
                };
                idConsulta = listaConsultas.Count + 1;
                
                listaConsultas.Add(c);

                consultaRepository.CreateConsulta(c);
                LimpiarCampos();
            }
        }

        private void ActualizarConsulta(object sender, EventArgs e)
        {
            if (selectedRow != null && ValidarFormulario())
            {
                var consulta = listaConsultas[(int)selectedRow];
                consulta.Sintomas = txtSintoma.Text;
                consulta.Fecha = txtFecha.Value;
                consulta.Veterinario = txtVeterinario.SelectedValue.ToString();
                consulta.Mascota = txtMascota.SelectedValue.ToString();

                var updateConsulta = consultaRepository.ActualizarConsulta(consulta);

                MessageBox.Show(updateConsulta);

                dgvConsultas.Refresh(); 
                selectedRow = null;
                LimpiarCampos();
            }
        }

        private void EliminarConsulta(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                var consulta = listaConsultas[(int)selectedRow];

                if (consulta.Id != 0)
                {
                    var message = consultaRepository.EliminarConsulta(consulta.Id);
                    MessageBox.Show(message);
                }

                listaConsultas.RemoveAt((int)selectedRow);
                dgvConsultas.DataSource = null;
                dgvConsultas.DataSource = listaConsultas;

                selectedRow = null;
                LimpiarCampos();
            }
        }


        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtMascota.Text) || string.IsNullOrWhiteSpace(txtSintoma.Text) || string.IsNullOrWhiteSpace(txtVeterinario.Text))
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void LimpiarCampos()
        {
            txtMascota.SelectedIndex = 0;
            txtSintoma.Clear();
            txtVeterinario.SelectedIndex = 0;
            txtFecha.Value = DateTime.Today;
        }
    }
}
