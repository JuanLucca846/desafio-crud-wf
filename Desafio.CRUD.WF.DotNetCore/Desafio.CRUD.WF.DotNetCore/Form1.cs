using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desafio.CRUD.WF.DotNetCore
{
    public partial class Form1 : Form
    {

        private DB database;
        

        public Form1()
        {
            InitializeComponent();
            database = new DB();
        }

        private void clearTextBox()
        {
            txtNome.Clear();
            txtCPF.Clear(); 
            txtTelefone.Clear();    
            txtEmail.Clear();
            txtSexo.Clear();
            txtEndereco.Clear();
            txtDataNascimento.Clear();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                User user = UserFactory.CreateUser(
                    txtNome.Text,
                    txtCPF.Text,
                    txtTelefone.Text,
                    txtEmail.Text,
                    txtSexo.Text,
                    txtEndereco.Text,
                    txtDataNascimento.Text
                );
              
                using(database) 
                {
                    database.createCon();
                    database.saveUserData(user);
                    clearTextBox();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Erro ao salvar os dados: " + ex);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                using (database)
                {
                    database.createCon();
                    List<User> userList = database.findAllUsers();
                    dataGridView1.DataSource = userList;
                }                   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar os dados: " + ex);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                User user = UserFactory.UpdateUser(
                    txtNome.Text,
                    txtCPF.Text,
                    txtTelefone.Text,
                    txtEmail.Text,
                    txtSexo.Text,
                    txtEndereco.Text,
                    txtDataNascimento.Text
                );

                using (database)
                {
                    database.createCon();
                    database.updateUserData(user);
                    clearTextBox();
                }
            }
            catch ( Exception ex )
            {
                MessageBox.Show("Erro ao atualizar os dados: " + ex);
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            try
            {
                using (database)
                {
                    database.createCon();
                    string cpf = txtCPF.Text;
                    database.deleteUser(cpf);
                    clearTextBox();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir os dados: " + ex);
            }
        }
    }
}
