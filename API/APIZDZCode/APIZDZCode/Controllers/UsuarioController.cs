using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using APIZDZCode.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIZDZCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly string caminhoArquivo = @"C:\Users\jande\OneDrive\Área de Trabalho\Desafio-ZDZCode\usuarios.csv"; // Ajuste o caminho do arquivo CSV

        public UsuarioController()
        {
        }

        [HttpPost]
        public Usuario CadastrarUsuario([FromBody] Usuario usuario)
        {
            // Lógica para salvar o usuário no arquivo CSV (ou onde você estiver armazenando os dados)
            SalvarUsuarioEmCSV(usuario);

            return usuario;
        }

        [HttpGet]
        public IEnumerable<Usuario> GetUsuarios()
        {
            // Lógica para ler os dados do arquivo CSV (ou de onde você estiver armazenando os dados)
            var usuarios = LerUsuariosDoCSV();

            return usuarios;
        }

        private void SalvarUsuarioEmCSV(Usuario usuario)
        {
            // Abre ou cria o arquivo CSV para escrita
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true, Encoding.UTF8))
            {
                // Formata os dados do usuário como uma linha CSV
                string linhaCSV = $"{usuario.Id},{usuario.Nome}";
                writer.WriteLine(linhaCSV);
            }
        }

        private IEnumerable<Usuario> LerUsuariosDoCSV()
        {
            var usuarios = new List<Usuario>();

            // Lê os dados do arquivo CSV e preenche a lista de usuários
            using (StreamReader reader = new StreamReader(caminhoArquivo, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var campos = linha.Split(',');
                    if (campos.Length == 2)
                    {
                        int id;
                        if (int.TryParse(campos[0], out id))
                        {
                            usuarios.Add(new Usuario { Id = id, Nome = campos[1] });
                        }
                    }
                }
            }

            return usuarios;
        }
    }
}
