namespace APIZDZCode.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string Responsavel { get; set; }
    }
}
