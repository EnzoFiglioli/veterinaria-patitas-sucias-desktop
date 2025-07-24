using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using MiAppVeterinaria.Services;
using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Models;
using MiAppVeterinaria.Repository;


namespace MiAppVeterinaria.Views
{
    public class TurnosView : UserControl
    {
        private TextBox txtNombre;
        private ComboBox cmbEspecie;
        private TextBox txtRaza;
        private NumericUpDown numPeso;
        private NumericUpDown numEdad;
        private ComboBox cmbDuenio;

        private Button btnGuardar;
        private Button btnEditar;
        private Button btnEliminar;

        private DataGridView dgvMascotas;
        private string rolUsuario;
        private int? selectedRow = null;

        private readonly MascotaService mascotaService = new MascotaService();
        private readonly DuenioRepository duenioRepository = new DuenioRepository();
        private List<TurnoAsignadoDTO> turnos = new List<TurnoAsignadoDTO>();

        private BindingList<Mascota> mascotasList;

        public TurnosView(string rol)
        {
            rolUsuario = rol;
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
                Text = "Gestión de Mascotas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true
            });

            panel.Controls.Add(new Label
            {
                Text = "Podés registrar, modificar o eliminar mascotas según tu rol.",
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

            txtNombre = new TextBox { Width = 250 };
            string[] especies = new string[] { "Perro", "Gato", "Ave", "Otro" };

            cmbEspecie = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            cmbEspecie.Items.AddRange(especies);

            if (cmbEspecie.Items.Count > 0)
            {
                cmbEspecie.SelectedIndex = 0;
            }

            txtRaza = new TextBox { Width = 250 };
            numEdad = new NumericUpDown { Width = 250, Maximum = 40 };
            numPeso = new NumericUpDown { Width = 250, Maximum = 200, DecimalPlaces = 2, Increment = 0.1M };

            var dueniosList = duenioRepository.ObtenerDuenios();
            cmbDuenio = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dueniosList,
                DisplayMember = "NombreCompleto",   // lo que se muestra
                ValueMember = "Id"
            };
            if (cmbDuenio.Items.Count > 0)
                cmbDuenio.SelectedIndex = 0;

            contenedor.Controls.Add(new Label { Text = "Nombre:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 0);
            contenedor.Controls.Add(txtNombre, 1, 0);
            contenedor.Controls.Add(new Label { Text = "Especie:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 1);
            contenedor.Controls.Add(cmbEspecie, 1, 1);
            contenedor.Controls.Add(new Label { Text = "Raza:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 2);
            contenedor.Controls.Add(txtRaza, 1, 2);
            contenedor.Controls.Add(new Label { Text = "Edad:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 3);
            contenedor.Controls.Add(numEdad, 1, 3);
            contenedor.Controls.Add(new Label { Text = "Peso:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 4);
            contenedor.Controls.Add(numPeso, 1, 4);
            contenedor.Controls.Add(new Label { Text = "Dueño:", Anchor = AnchorStyles.Right, AutoSize = true }, 0, 5);
            contenedor.Controls.Add(cmbDuenio, 1, 5);

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
            btnGuardar.Click += GuardarMascota;

            btnEditar = new Button
            {
                Text = "Editar",
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btnEditar.Click += EditarMascota;

            btnEliminar = new Button
            {
                Text = "Eliminar",
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btnEliminar.Click += EliminarMascota;

            // Permisos según el rol
            if (rolUsuario == "Administracion")
            {
                panelBotones.Controls.AddRange(new[] { btnGuardar, btnEditar, btnEliminar });
            }
            else if (rolUsuario == "Veterinario")
            {
                btnGuardar.Enabled = false;
                btnEliminar.Enabled = false;
                panelBotones.Controls.AddRange(new[] { btnEditar, btnGuardar, btnEliminar });
            }
            else if (rolUsuario == "Recepcion")
            {
                panelBotones.Controls.Add(btnGuardar);
            }

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
            dgvMascotas = new DataGridView
            {
                Height = 300,
                Dock = DockStyle.Top,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            mascotasList = mascotaService.GetMascotas();
            dgvMascotas.DataSource = mascotasList;
           
            dgvMascotas.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedRow = e.RowIndex;

                    txtNombre.Text = dgvMascotas.Rows[e.RowIndex].Cells[1].Value?.ToString();
                    cmbEspecie.SelectedItem = dgvMascotas.Rows[e.RowIndex].Cells[2].Value?.ToString();
                    txtRaza.Text = dgvMascotas.Rows[e.RowIndex].Cells[3].Value?.ToString();

                    // Manejo seguro de edad
                    var edadStr = dgvMascotas.Rows[e.RowIndex].Cells[4].Value?.ToString();
                    if (decimal.TryParse(edadStr, out decimal edad))
                        numEdad.Value = edad;
                    else
                        numEdad.Value = 0;

                    if (dgvMascotas.Columns.Count > 5)
                    {
                        var pesoValue = dgvMascotas.Rows[e.RowIndex].Cells[5].Value?.ToString();
                        if (decimal.TryParse(pesoValue, out decimal peso))
                            numPeso.Value = peso;
                        else
                            numPeso.Value = 0;
                    }

                    string nombreDuenio = dgvMascotas.Rows[e.RowIndex].Cells[6].Value?.ToString();

                    foreach (var item in cmbDuenio.Items)
                    {
                        if (((Duenio)item).NombreCompleto == nombreDuenio)
                        {
                            cmbDuenio.SelectedItem = item;
                            break;
                        }
                    }
                }

            };

            return dgvMascotas;
        }

        private void GuardarMascota(object sender, EventArgs e)
        {
            if (ValidarFormulario())
            {
                MascotaDTO mascotaDTO = new MascotaDTO
                {
                    NombreMascota = txtNombre.Text,
                    Edad = Convert.ToInt32(numEdad.Value),
                    EspecieId = cmbEspecie.SelectedIndex + 1,
                    Raza = txtRaza.Text,
                    DuenioId = (int)cmbDuenio.SelectedValue,
                    Peso = numPeso.Value
                };

                string message = mascotaService.CreateMascota(mascotaDTO);

                MessageBox.Show(message);

                int idMayor = 0;

                foreach (var m in mascotasList)
                {
                    if (m.Id > idMayor)
                        idMayor = m.Id;
                }

                int nuevoId = idMayor + 1;

                mascotasList.Add(new Mascota
                {
                    Id = nuevoId,
                    Nombre = txtNombre.Text,
                    Edad = Convert.ToInt32(numEdad.Value),
                    Especie = cmbEspecie.Text,
                    Raza = txtRaza.Text,
                    NombreCompletoDuenio = cmbDuenio.Text,
                    Peso = numPeso.Value
                });
                LimpiarCampos();
            }
        }

        private void EditarMascota(object sender, EventArgs e)
        {
            if (selectedRow != null && ValidarFormulario())
            {
                var row = dgvMascotas.Rows[(int)selectedRow];
                row.Cells[0].Value = txtNombre.Text;
                row.Cells[1].Value = cmbEspecie.Text;
                row.Cells[2].Value = txtRaza.Text;
                row.Cells[3].Value = numEdad.Value;
                row.Cells[4].Value = cmbDuenio.Text;
                LimpiarCampos();
                selectedRow = null;
            }
        }

        private void EliminarMascota(object sender, EventArgs e)
        {
            if (selectedRow != null)
            {
                var resultadoDialogo = MessageBox.Show("¿Estás seguro de que querés eliminar esta mascota?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultadoDialogo == DialogResult.Yes)
                {
                    int idMascota = ((Mascota)dgvMascotas.Rows[(int)selectedRow].DataBoundItem).Id;

                    var resultado = mascotaService.DeleteMascotaById(idMascota);
                    MessageBox.Show(resultado);
 
                    LimpiarCampos();
                    selectedRow = null;
                }
            }
            else
            {
                MessageBox.Show("Seleccioná una mascota primero.");
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtRaza.Text) || cmbDuenio.SelectedIndex == -1)
            {
                MessageBox.Show("Completá todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtRaza.Clear();
            numEdad.Value = 0;
            cmbEspecie.SelectedIndex = 0;
            cmbDuenio.SelectedIndex = 0;
        }
    }
}
