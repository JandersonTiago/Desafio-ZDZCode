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
        private readonly string caminhoArquivo = @"C:\Users\jande\OneDrive\�rea de Trabalho\Desafio-ZDZCode\usuarios.csv"; // Ajuste o caminho do arquivo CSV

        public UsuarioController()
        {
        }

        [HttpPost]
        public Usuario CadastrarUsuario([FromBody] Usuario usuario)
        {
            // L�gica para salvar o usu�rio no arquivo CSV (ou onde voc� estiver armazenando os dados)
            SalvarUsuarioEmCSV(usuario);

            return usuario;
        }

        [HttpGet]
        public IEnumerable<Usuario> GetUsuarios()
        {
            // L�gica para ler os dados do arquivo CSV (ou de onde voc� estiver armazenando os dados)
            var usuarios = LerUsuariosDoCSV();

            return usuarios;
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirUsuario(int id)
        {
            try
            {
                // L�gica para excluir o usu�rio do arquivo CSV (ou de onde voc� estiver armazenando os dados)
                if (ExcluirUsuarioDoCSV(id))
                {
                    return Ok(); // Retorna 200 OK se a exclus�o for bem-sucedida
                }
                else
                {
                    return NotFound(); // Retorna 404 Not Found se o usu�rio n�o for encontrado
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir usu�rio: {ex.Message}"); // Retorna 500 Internal Server Error em caso de erro
            }
        }

        private void SalvarUsuarioEmCSV(Usuario usuario)
        {
            // Abre ou cria o arquivo CSV para escrita
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true, Encoding.UTF8))
            {
                // Formata os dados do usu�rio como uma linha CSV
                string linhaCSV = $"{usuario.Id},{usuario.Nome}";
                writer.WriteLine(linhaCSV);
            }
        }

        private IEnumerable<Usuario> LerUsuariosDoCSV()
        {
            var usuarios = new List<Usuario>();

            // L� os dados do arquivo CSV e preenche a lista de usu�rios
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

        private bool ExcluirUsuarioDoCSV(int id)
        {
            // L� todos os usu�rios do arquivo CSV
            var usuarios = LerUsuariosDoCSV().ToList();

            // Encontra o usu�rio com o ID correspondente
            var usuarioParaExcluir = usuarios.FirstOrDefault(u => u.Id == id);

            // Se o usu�rio foi encontrado, remove-o da lista e atualiza o arquivo CSV
            if (usuarioParaExcluir != null)
            {
                usuarios.Remove(usuarioParaExcluir);

                // Reescreve o arquivo CSV com os usu�rios atualizados
                ReescreverArquivoCSV(usuarios);

                return true; // Retorna true indicando que a exclus�o foi bem-sucedida
            }
            else
            {
                return false; // Retorna false indicando que o usu�rio n�o foi encontrado
            }
        }

        private void ReescreverArquivoCSV(IEnumerable<Usuario> usuarios)
        {
            // Reescreve o arquivo CSV com os usu�rios atualizados
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, false, Encoding.UTF8))
            {
                foreach (var usuario in usuarios)
                {
                    string linhaCSV = $"{usuario.Id},{usuario.Nome}";
                    writer.WriteLine(linhaCSV);
                }
            }
        }
    }
}
