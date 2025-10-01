using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Usuario.Response
{
    /// <summary>
    /// Retorno detalhado (GET by id / POST created).
    /// ⚠ Não expõe senha.
    /// </summary>
    public record class UsuarioResponse
    {
        /// <summary>Identificador do usuário.</summary>
        /// <example>77</example>
        public long Id { get; init; }

        /// <summary>Nome completo.</summary>
        /// <example>Rafael Almeida</example>
        [StringLength(150)]
        public string Nome { get; init; } = string.Empty;

        /// <summary>E-mail (login).</summary>
        /// <example>rafael@mottu.com</example>
        [EmailAddress, StringLength(200)]
        public string Email { get; init; } = string.Empty;

        /// <summary>Perfil de acesso.</summary>
        /// <example>OPERADOR</example>
        [StringLength(40)]
        public string Perfil { get; init; } = string.Empty;

        /// <summary>ID da filial (opcional).</summary>
        /// <example>42</example>
        public long? FilialId { get; init; }
    }
}
