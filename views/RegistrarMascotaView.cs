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
    public class RegistrarMascotaView : UserControl
    {
        private TextBox txtNombre, txtRaza;
        private ComboBox cmbEspecie, cmbDuenio;
        private NumericUpDown numEdad, numPeso;
        private Button btnGuardar, btnEditar, btnEliminar;
        private DataGridView dgvMascotas;
        private string rolUsuario;
        private int? selectedRow = null;

        private readonly MascotaService mascotaService = new MascotaService();
        private readonly DuenioRepository duenioRepository = new DuenioRepository();
        private BindingList<Mascota> mascotasList;

        public RegistrarMascotaView(string rol)
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
            txtRaza = new TextBox { Width = 250 };

            cmbEspecie = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEspecie.Items.AddRange(new string[] { "Perro", "Gato", "Ave", "Otro" });
            cmbEspecie.SelectedIndex = 0;

            numEdad = new NumericUpDown { Width = 250, Maximum = 40 };
            numPeso = new NumericUpDown { Width = 250, Maximum = 200, DecimalPlaces = 2, Increment = 0.1M };

            cmbDuenio = new ComboBox
            {
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = duenioRepository.ObtenerDuenios(),
                DisplayMember = "NombreCompleto",
                ValueMember = "Id"
            };
            if (cmbDuenio.Items.Count > 0) cmbDuenio.SelectedIndex = 0;

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

            var panelBotones = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.LeftToRight, AutoSize = true, Margin = new Padding(0, 10, 0, 0) };

            btnGuardar = CrearBoton("Guardar", Color.MediumSeaGreen, GuardarMascota);
            btnEditar = CrearBoton("Editar", Color.RoyalBlue, EditarMascota);
            btnEliminar = CrearBoton("Eliminar", Color.Firebrick, EliminarMascota);

            if (rolUsuario == "Administracion")
                panelBotones.Controls.AddRange(new[] { btnGuardar, btnEditar, btnEliminar });
            else if (rolUsuario == "Veterinario")
                panelBotones.Controls.AddRange(new[] { btnEditar });
            else if (rolUsuario == "Recepcion")
                panelBotones.Controls.AddRange(new[] { btnGuardar });

            var panelContenedor = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.TopDown, AutoSize = true };
            panelContenedor.Controls.Add(contenedor);
            panelContenedor.Controls.Add(panelBotones);

            return panelContenedor;
        }

        private Button CrearBoton(string texto, Color color, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = texto,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 100
            };
            btn.Click += onClick;
            return btn;
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

                    var row = dgvMascotas.Rows[e.RowIndex];
                    txtNombre.Text = row.Cells[1].Value?.ToString();
                    cmbEspecie.SelectedItem = row.Cells[2].Value?.ToString();
                    txtRaza.Text = row.Cells[3].Value?.ToString();
                    numEdad.Value = decimal.TryParse(row.Cells[4].Value?.ToString(), out var edad) ? edad : 0;
                    numPeso.Value = decimal.TryParse(row.Cells[5].Value?.ToString(), out var peso) ? peso : 0;

                    string nombreDuenio = row.Cells[6].Value?.ToString();
                    foreach (var item in cmbDuenio.Items)
                        if (((Duenio)item).NombreCompleto == nombreDuenio)
                        {
                            cmbDuenio.SelectedItem = item;
                            break;
                        }
                }
            };

            return dgvMascotas;
        }

        private void GuardarMascota(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            var dto = new MascotaDTO
            {
                NombreMascota = txtNombre.Text,
                Edad = (int)numEdad.Value,
                EspecieId = cmbEspecie.SelectedIndex + 1,
                Raza = txtRaza.Text,
                DuenioId = (int)cmbDuenio.SelectedValue,
                Peso = numPeso.Value
            };

            string msg = mascotaService.CreateMascota(dto);
            MessageBox.Show(msg);

            int nuevoId = mascotasList.Count > 0 ? mascotasList[^1].Id + 1 : 1;

            mascotasList.Add(new Mascota
            {
                Id = nuevoId,
                Nombre = dto.NombreMascota,
                Edad = dto.Edad,
                Especie = cmbEspecie.Text,
                Raza = dto.Raza,
                NombreCompletoDuenio = cmbDuenio.Text,
                Peso = dto.Peso
            });

            LimpiarCampos();
        }

        private void EditarMascota(object sender, EventArgs e)
        {
            if (selectedRow == null || !ValidarFormulario()) return;

            var row = dgvMascotas.Rows[(int)selectedRow];
            row.Cells[1].Value = txtNombre.Text;
            row.Cells[2].Value = cmbEspecie.Text;
            row.Cells[3].Value = txtRaza.Text;
            row.Cells[4].Value = numEdad.Value;
            row.Cells[5].Value = numPeso.Value;
            row.Cells[6].Value = cmbDuenio.Text;

            LimpiarCampos();
            dgvMascotas.Refresh();
            selectedRow = null;
        }

        private void EliminarMascota(object sender, EventArgs e)
        {
            if (selectedRow == null)
            {
                MessageBox.Show("Seleccioná una mascota primero.");
                return;
            }

            var confirmar = MessageBox.Show("¿Eliminar mascota seleccionada?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmar != DialogResult.Yes) return;

            var mascota = (Mascota)dgvMascotas.Rows[(int)selectedRow].DataBoundItem;
            string msg = mascotaService.DeleteMascotaById(mascota.Id);
            MessageBox.Show(msg);

            mascotasList.Remove(mascota);
            LimpiarCampos();
            selectedRow = null;
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
            numPeso.Value = 0;
            cmbEspecie.SelectedIndex = 0;
            if (cmbDuenio.Items.Count > 0)
                cmbDuenio.SelectedIndex = 0;
        }
    }
}
