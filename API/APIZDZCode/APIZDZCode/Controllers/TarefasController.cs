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
    public class TarefasController : ControllerBase
    {
        private readonly string caminhoArquivo = @"C:\Users\jande\OneDrive\Área de Trabalho\Desafio-ZDZCode\tarefas.csv"; // Ajuste o caminho do arquivo CSV

        public TarefasController()
        {
        }

        [HttpPost]
        public ActionResult<Tarefa> CadastrarTarefa([FromBody] Tarefa tarefa)
        {
            try
            {
                // Lógica para salvar a tarefa no arquivo CSV
                SalvarTarefaEmCSV(tarefa);

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao salvar a tarefa: {ex.Message}");
            }
        }

        private void SalvarTarefaEmCSV(Tarefa tarefa)
        {
            // Abre ou cria o arquivo CSV para escrita
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, true, Encoding.UTF8))
            {
                // Formata os dados da tarefa como uma linha CSV
                string linhaCSV = $"{tarefa.Id},{tarefa.Titulo},{tarefa.Data},{tarefa.Status},{tarefa.Responsavel}";
                writer.WriteLine(linhaCSV);
            }
        }

        [HttpGet]
        public IEnumerable<Tarefa> GetTarefas()
        {
            // Lógica para ler os dados do arquivo CSV
            var tarefas = LerTarefasDoCSV();

            return tarefas;
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirTarefa(int id)
        {
            try
            {
                // Lógica para excluir a tarefa do arquivo CSV
                if (ExcluirTarefaDoCSV(id))
                {
                    return Ok(); // Retorna 200 OK se a exclusão for bem-sucedida
                }
                else
                {
                    return NotFound(); // Retorna 404 Not Found se a tarefa não for encontrada
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao excluir tarefa: {ex.Message}"); // Retorna 500 Internal Server Error em caso de erro
            }
        }

        private bool ExcluirTarefaDoCSV(int id)
        {
            // Lê todas as tarefas do arquivo CSV
            var tarefas = LerTarefasDoCSV().ToList();

            // Encontra a tarefa com o ID correspondente
            var tarefaParaExcluir = tarefas.FirstOrDefault(t => t.Id == id);

            // Se a tarefa foi encontrada, remove-a da lista e atualiza o arquivo CSV
            if (tarefaParaExcluir != null)
            {
                tarefas.Remove(tarefaParaExcluir);

                // Reescreve o arquivo CSV com as tarefas atualizadas
                ReescreverArquivoCSV(tarefas);

                return true; // Retorna true indicando que a exclusão foi bem-sucedida
            }
            else
            {
                return false; // Retorna false indicando que a tarefa não foi encontrada
            }
        }

        private void ReescreverArquivoCSV(IEnumerable<Tarefa> tarefas)
        {
            // Reescreve o arquivo CSV com as tarefas atualizadas
            using (StreamWriter writer = new StreamWriter(caminhoArquivo, false, Encoding.UTF8))
            {
                foreach (var tarefa in tarefas)
                {
                    string linhaCSV = $"{tarefa.Id},{tarefa.Titulo},{tarefa.Data},{tarefa.Status},{tarefa.Responsavel}";
                    writer.WriteLine(linhaCSV);
                }
            }
        }

        private IEnumerable<Tarefa> LerTarefasDoCSV()
        {
            var tarefas = new List<Tarefa>();

            // Lê os dados do arquivo CSV e preenche a lista de tarefas
            using (StreamReader reader = new StreamReader(caminhoArquivo, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    var linha = reader.ReadLine();
                    var campos = linha.Split(',');

                    // Verifica se a linha contém pelo menos 5 campos (incluindo o responsável)
                    if (campos.Length >= 5)
                    {
                        int id;
                        if (int.TryParse(campos[0], out id))
                        {
                            // Obtém os valores dos campos
                            string titulo = campos[1];
                            DateTime data;
                            if (DateTime.TryParse(campos[2], out data))
                            {
                                string status = campos[3];
                                string responsavel = campos[4]; // Agora acessamos diretamente o índice 4

                                // Cria uma nova tarefa e a adiciona à lista de tarefas
                                tarefas.Add(new Tarefa
                                {
                                    Id = id,
                                    Titulo = titulo,
                                    Data = data,
                                    Status = status,
                                    Responsavel = responsavel
                                });
                            }
                        }
                    }
                }
            }

            return tarefas;
        }
    }
}
