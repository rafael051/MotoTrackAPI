using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Usuario.Request
{
    /// <summary>
    /// Payload para atualização de Usuário.
    /// Não inclui senha (use um endpoint específico para troca de senha, se necessário).
    /// Usa os mesmos campos básicos do request de criação.
    /// </summary>
    public record class UsuarioUpdateRequest
    {
        /// <summary>Nome completo do usuário.</summary>
        /// <example>Rafael Almeida</example>
        [Required, StringLength(150)]
        public string Nome { get; init; } = string.Empty;

        /// <summary>E-mail do usuário (login).</summary>
        /// <example>rafael@mottu.com</example>
        [Required, EmailAddress, StringLength(200)]
        public string Email { get; init; } = string.Empty;

        /// <summary>Perfil de acesso (OPERADOR, GESTOR, ADMINISTRADOR).</summary>
        /// <example>OPERADOR</example>
        [Required, StringLength(40)]
        [RegularExpression("^(OPERADOR|GESTOR|ADMINISTRADOR)$",
            ErrorMessage = "Perfil inválido. Use OPERADOR, GESTOR ou ADMINISTRADOR.")]
        public string Perfil { get; init; } = "OPERADOR";

        /// <summary>ID da filial (opcional) à qual o usuário está vinculado.</summary>
        /// <example>42</example>
        public long? FilialId { get; init; }
    }
}
