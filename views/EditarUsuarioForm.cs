using System;
using System.Windows.Forms;
using MiAppVeterinaria.DTO;
using MiAppVeterinaria.Services;

namespace MiAppVeterinaria.Views
{
    public partial class EditarUsuarioForm : Form
    {
        private TextBox txtID,txtUsername, txtNombre, txtApellido, txtRol;
        private Button btnGuardar, btnEliminar;
        private UserDTO usuario;
        public event Action ActualizarLista;

        private UsuarioService usuarioService;

        public EditarUsuarioForm(UserDTO usuario)
        {
            this.usuario = usuario;
            usuarioService = new UsuarioService();

            Text = "Editar Usuario";
            Size = new System.Drawing.Size(300, 300);
            
            Label lbl0 = new Label { Text = "ID", Top = 20, Left = 10 };
            txtID = new TextBox { Top = 40, Left = 10, Width = 250, Text = Convert.ToString(usuario.Id) };

            Label lbl1 = new Label { Text = "Username", Top = 20, Left = 10 };
            txtUsername = new TextBox { Top = 40, Left = 10, Width = 250, Text = usuario.Username };

            Label lbl2 = new Label { Text = "Nombre", Top = 70, Left = 10 };
            txtNombre = new TextBox { Top = 90, Left = 10, Width = 250, Text = usuario.Nombre };

            Label lbl3 = new Label { Text = "Apellido", Top = 120, Left = 10 };
            txtApellido = new TextBox { Top = 140, Left = 10, Width = 250, Text = usuario.Apellido };

            Label lbl4 = new Label { Text = "Rol", Top = 170, Left = 10 };
            txtRol = new TextBox { Top = 190, Left = 10, Width = 250, Text = usuario.Rol };

            btnGuardar = new Button { Text = "Guardar", Top = 230, Left = 10 };
            btnEliminar = new Button { Text = "Eliminar", Top = 230, Left = 110 };

            btnGuardar.Click += (s, e) =>
            {
                usuario.Username = txtUsername.Text;
                usuario.Nombre = txtNombre.Text;
                usuario.Apellido = txtApellido.Text;
                usuario.Rol = txtRol.Text;

                // usuarioService.ActualizarUsuario(usuario);
                MessageBox.Show("Usuario actualizado");
                ActualizarLista?.Invoke();
                Close();
            };

            btnEliminar.Click += (s, e) =>
            {
                var confirmar = MessageBox.Show("¿Estás seguro que querés eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo);
                if (confirmar == DialogResult.Yes)
                {
                    // usuarioService.EliminarUsuario(usuario.IdUsuario);
                    MessageBox.Show("Usuario eliminado");
                    ActualizarLista?.Invoke();
                    Close();
                }
            };
            Controls.Add(lbl0); Controls.Add(txtID);
            Controls.Add(lbl1); Controls.Add(txtUsername);
            Controls.Add(lbl2); Controls.Add(txtNombre);
            Controls.Add(lbl3); Controls.Add(txtApellido);
            Controls.Add(lbl4); Controls.Add(txtRol);
            Controls.Add(btnGuardar); Controls.Add(btnEliminar);
        }
    }
}
