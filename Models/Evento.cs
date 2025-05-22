using System;

namespace MotoTrackAPI.Models
{
    public class Evento
    {
        public int Id { get; set; }  // ✅ Chave primária obrigatória

        public string Tipo { get; set; }
        public DateTime DataHora { get; set; }
        public string Motivo { get; set; }
        public string Localizacao { get; set; }
    }
}
