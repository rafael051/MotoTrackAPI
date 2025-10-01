using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Usuario.Request
{
    /// <summary>
    /// Payload para criação de Usuário.
    /// ⚠ A senha vem em texto e deve ser convertida para HASH na camada de serviço antes de persistir.
    /// Relações via ID (FilialId opcional).
    /// </summary>
    public record class UsuarioCreateRequest
    {
        /// <summary>Nome completo do usuário.</summary>
        /// <example>Rafael Almeida</example>
        [Required, StringLength(150)]
        public string Nome { get; init; } = string.Empty;

        /// <summary>E-mail do usuário (login).</summary>
        /// <example>rafael@mottu.com</example>
        [Required, EmailAddress, StringLength(200)]
        public string Email { get; init; } = string.Empty;

        /// <summary>
        /// Senha em texto puro (apenas no request).
        /// ⚠ NUNCA devolver em responses; gerar HASH antes de persistir.
        /// </summary>
        /// <example>Str0ng@123</example>
        [Required, StringLength(255, ErrorMessage = "A senha deve ter no máximo 255 caracteres.")]
        public string Senha { get; init; } = string.Empty;

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
