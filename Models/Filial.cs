namespace MotoTrackAPI.Models
{
    public class Filial
    {
        public int Id { get; set; }  // ✅ Chave primária obrigatória

        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
