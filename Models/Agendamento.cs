namespace MotoTrackAPI.Models
{
    public class Agendamento
    {
        public int Id { get; set; }  // ✅ Isso é obrigatório!
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
    }
}
