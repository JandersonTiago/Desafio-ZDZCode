using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using APIZDZCode.Models; // Importe o namespace onde a classe Tarefa está localizada
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
    }
}
