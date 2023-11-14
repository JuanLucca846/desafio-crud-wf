using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.CRUD.WF.DotNetCore
{
    public class User
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Sexo { get; set; }
        public string Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
    }

    public static class UserFactory
    {
        public static User CreateUser(string nome, string cpf, string telefone, string email, string sexo, string endereco, string dataNascimento)
        {
            ValidateUserInput(nome, cpf, telefone, email, sexo, endereco, dataNascimento);

            return new User
            {
                Nome = nome,
                CPF = cpf,
                Telefone = telefone,
                Email = email,
                Sexo = sexo,
                Endereco = endereco,
                DataNascimento = DateTime.Parse(dataNascimento)
            };
        }

        public static User UpdateUser(string nome, string cpf, string telefone, string email, string sexo, string endereco, string dataNascimento)
        {
            ValidateUserInput(nome, cpf, telefone, email, sexo, endereco, dataNascimento);

            return new User
            {
                Nome = nome,
                CPF = cpf,
                Telefone = telefone,
                Email = email,
                Sexo = sexo,
                Endereco = endereco,
                DataNascimento = DateTime.Parse(dataNascimento)
            };
        }

        private static void ValidateUserInput(string nome, string cpf, string telefone, string email, string sexo, string endereco, string dataNascimento)
        {
            if (string.IsNullOrEmpty(nome) || 
                string.IsNullOrEmpty(cpf) || 
                string.IsNullOrEmpty(telefone) || 
                string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(sexo) || 
                string.IsNullOrEmpty(endereco) || 
                !DateTime.TryParseExact(dataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Informe todos os dados corretamente.");
            }
        }

    }
}
