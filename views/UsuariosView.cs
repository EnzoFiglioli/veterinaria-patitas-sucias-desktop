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
    public class UsuariosView : UserControl
    {
        private DataGridView dgvUsuarios;
        private Button btnCrear, btnEditar, btnEliminar;
        private BindingSource bindingSource;
        private UsuarioService usuarioService;
        private string Rol;

        public UsuariosView(string rol)
        {
            Rol = rol;
            usuarioService = new UsuarioService(); // o inyectar

            dgvUsuarios = new DataGridView
            {
                Height = 300,
                Dock = DockStyle.Top,
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.WhiteSmoke,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ID", HeaderText = "ID", ReadOnly = true });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Username", HeaderText = "Username" });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nombre", HeaderText = "Nombre" });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Apellido", HeaderText = "Apellido" });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Rol", HeaderText = "Rol" });


            dgvUsuarios.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var usuario = (UserDTO)dgvUsuarios.Rows[e.RowIndex].DataBoundItem;
                    var form = new EditarUsuarioForm(usuario);
                    form.ActualizarLista += () =>
                    {
                        dgvUsuarios.DataSource = null;
                        dgvUsuarios.DataSource = usuarioService.GetAllUsers();
                    };
                    form.ShowDialog();
                }
            };

            btnCrear = new Button { Text = "Crear Usuario", Top = 310, Left = 10 };
            btnEditar = new Button { Text = "Editar Usuario", Top = 310, Left = 130 };
            btnEliminar = new Button { Text = "Eliminar Usuario", Top = 310, Left = 250 };

            // bindingSource = new BindingSource();
            var listaUsuarios = usuarioService.GetAllUsers();
            dgvUsuarios.DataSource = listaUsuarios;

            Controls.Add(dgvUsuarios);
            Controls.Add(btnCrear);
            Controls.Add(btnEditar);
            Controls.Add(btnEliminar);

        }

       
      
    }
}
