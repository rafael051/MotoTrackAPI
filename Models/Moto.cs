namespace MotoTrackAPI.Models
{
    public class Moto
    {
        public int Id { get; set; }  // ✅ Chave primária obrigatória

        public string Placa { get; set; }   // Ex.: ABC1234
        public string Modelo { get; set; }  // Ex.: CG 160
        public string Marca { get; set; }   // Ex.: Honda
        public int Ano { get; set; }        // Ex.: 2023
        public string Status { get; set; }  // Ex.: Disponível, Locada, Manutenção
    }
}
