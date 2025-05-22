namespace MotoTrackAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }  // ✅ Chave primária obrigatória

        public string Nome { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }  // Ex.: Administrador, Operador, Gestor
    }
}
