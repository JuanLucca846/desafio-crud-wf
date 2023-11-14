using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desafio.CRUD.WF.DotNetCore
{
    internal class DB : IDisposable
    {
        private SqlConnection conn;

        public void createCon()
        {
            string constring = ConfigurationManager.ConnectionStrings["myconstring"].ConnectionString;
            conn = new SqlConnection(constring);
            conn.Open();
        }

        private bool checkIfCpfIsValid(string cpf)
        {
            return !string.IsNullOrWhiteSpace(cpf) && cpf.Length == 11;
        }

        private bool checkIfCpfAlreadyExists(string cpf)
        {
            string query = "SELECT COUNT(1) FROM [dbo].[user] WHERE [CPF] = @CPF";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CPF", cpf);

                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        private bool checkIfEmailIsValid(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }

        private bool CheckIfSexIsValid(string sex)
        {
            return !string.IsNullOrWhiteSpace(sex) && (sex.ToLower() == "masculino" || sex.ToLower() == "feminino");
        }

        public void saveUserData(User user)
        {
            if (!checkIfCpfIsValid(user.CPF))
            {
                MessageBox.Show("O CPF deve possuir 11 caracteres.");
                return;
            }

            if (checkIfCpfAlreadyExists(user.CPF))
            {
                MessageBox.Show("Esse CPF, já foi cadastrado.");
                return;
            }

            if (!checkIfEmailIsValid(user.Email))
            {
                MessageBox.Show("O email deve possuir @.");
                return;
            }

            if(!CheckIfSexIsValid(user.Sexo))
            {
                MessageBox.Show("O sexo deve ser: 'Masculino' ou 'Feminino'.");
                return;
            }

            string query = "INSERT INTO [dbo].[user]([Nome], [CPF], [Telefone], [Email], [Sexo], [Endereco], [DataNascimento]) " +
                           "VALUES (@Nome, @CPF, @Telefone, @Email, @Sexo, @Endereco, @DataNascimento)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Nome", SqlDbType.VarChar) { Value = user.Nome },
                new SqlParameter("@CPF", SqlDbType.VarChar) { Value = user.CPF },
                new SqlParameter("@Telefone", SqlDbType.VarChar) { Value = user.Telefone },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email },
                new SqlParameter("@Sexo", SqlDbType.VarChar) { Value = user.Sexo },
                new SqlParameter("@Endereco", SqlDbType.VarChar) { Value = user.Endereco },
                new SqlParameter("@DataNascimento", SqlDbType.DateTime) { Value = user.DataNascimento }
            };

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {                              
                cmd.Parameters.AddRange(parameters);     

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    if(rows > 0)
                    {
                        MessageBox.Show("Dados salvos com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar os dados: " + ex);
                }
            };          
        }

        public List<User> findAllUsers()
        {
            List<User> users = new List<User>();

            string query = "SELECT * FROM [dbo].[user]";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    using (SqlDataReader readUserData = cmd.ExecuteReader())
                    {
                        while (readUserData.Read())
                        {
                            User user = new User
                            {
                                Nome = readUserData["Nome"].ToString(),
                                CPF = readUserData["CPF"].ToString(),
                                Telefone = readUserData["Telefone"].ToString(),
                                Email = readUserData["Email"].ToString(),
                                Sexo = readUserData["Sexo"].ToString(),
                                Endereco = readUserData["Endereco"].ToString(),
                                DataNascimento = Convert.ToDateTime(readUserData["DataNascimento"])
                            };

                            users.Add(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao buscar os dados! " + ex);
                }
            }

            return users;
        }

        public void updateUserData(User user)
        {
            if (!checkIfCpfIsValid(user.CPF))
            {
                MessageBox.Show("O CPF deve possuir 11 caracteres.");
                return;
            }

            if (!checkIfEmailIsValid(user.Email))
            {
                MessageBox.Show("Email deve possuir @.");
                return;
            }

            if (!CheckIfSexIsValid(user.Sexo))
            {
                MessageBox.Show("O sexo deve ser: 'Masculino' ou 'Feminino'.");
                return;
            }

            string query = "UPDATE [dbo].[user] SET [Nome] = @Nome, [Telefone] = @Telefone, [Email] = @Email, " +
                   "[Sexo] = @Sexo, [Endereco] = @Endereco, [DataNascimento] = @DataNascimento WHERE [CPF] = @CPF";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Nome", SqlDbType.VarChar) { Value = user.Nome },
                new SqlParameter("@CPF", SqlDbType.VarChar) { Value = user.CPF },
                new SqlParameter("@Telefone", SqlDbType.VarChar) { Value = user.Telefone },
                new SqlParameter("@Email", SqlDbType.VarChar) { Value = user.Email },
                new SqlParameter("@Sexo", SqlDbType.VarChar) { Value = user.Sexo },
                new SqlParameter("@Endereco", SqlDbType.VarChar) { Value = user.Endereco },
                new SqlParameter("@DataNascimento", SqlDbType.DateTime) { Value = user.DataNascimento }
            };

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Dados atualizados com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar os dados: " + ex);
                }
            };
        }

        public void deleteUser(string cpf)
        {
            if (!checkIfCpfIsValid(cpf))
            {
                MessageBox.Show("O CPF deve possuir 11 caracteres.");
                return;
            }

            if (!checkIfCpfAlreadyExists(cpf))
            {
                MessageBox.Show("Informe um CPF valido.");
                return;
            }

            string query = "DELETE FROM [dbo].[user] WHERE [CPF] = @CPF";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@CPF", cpf);

                try
                {
                    int rows = cmd.ExecuteNonQuery();
                    if( rows > 0 )
                    {
                        MessageBox.Show("Usuario excluido com sucesso.");
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Erro ao excluir o usuario: " + ex);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {               
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }           
        }

    }
}
