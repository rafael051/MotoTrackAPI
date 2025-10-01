using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Moto.Request
{
    /// <summary>
    /// Payload para criação de Moto.
    /// Relações via ID (FilialId opcional).
    /// </summary>
    public record class MotoCreateRequest
    {
        /// <summary>Placa da moto.</summary>
        /// <example>ABC1D23</example>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(20, ErrorMessage = "A placa deve ter no máximo 20 caracteres.")]
        // Dica: se quiser validar padrão Mercosul/antigo, adicione um [RegularExpression(...)]
        // ✅ já aplicando validação para ambos os formatos:
        // - Antigo: ABC1234
        // - Mercosul: ABC1D23 (4º dígito é número, 5º é letra/dígito, 6º-7º números)
        [RegularExpression(@"^([A-Z]{3}\d{4}|[A-Z]{3}\d[A-Z0-9]\d{2})$",
            ErrorMessage = "Placa inválida. Use o formato antigo (ABC1234) ou Mercosul (ABC1D23).")]
        public string Placa { get; init; } = string.Empty;

        /// <summary>Modelo da moto.</summary>
        /// <example>CG 160 Start</example>
        [Required, StringLength(120, ErrorMessage = "O modelo deve ter no máximo 120 caracteres.")]
        public string Modelo { get; init; } = string.Empty;

        /// <summary>Marca da moto.</summary>
        /// <example>Honda</example>
        [Required, StringLength(120, ErrorMessage = "A marca deve ter no máximo 120 caracteres.")]
        public string Marca { get; init; } = string.Empty;

        /// <summary>Ano de fabricação.</summary>
        /// <example>2022</example>
        [Range(2000, 2100, ErrorMessage = "O ano deve ser entre 2000 e 2100.")]
        public int Ano { get; init; }

        /// <summary>Status operacional (ex.: DISPONIVEL, LOCADA, MANUTENCAO).</summary>
        /// <example>DISPONIVEL</example>
        [Required, StringLength(60, ErrorMessage = "O status deve ter no máximo 60 caracteres.")]
        public string Status { get; init; } = "DISPONIVEL";

        /// <summary>ID da filial (opcional).</summary>
        /// <example>42</example>
        public long? FilialId { get; init; }

        /// <summary>Latitude atual (opcional).</summary>
        /// <example>-23.58990</example>
        [Range(-90, 90, ErrorMessage = "Latitude deve estar entre -90 e 90.")]
        public double? Latitude { get; init; }

        /// <summary>Longitude atual (opcional).</summary>
        /// <example>-46.63450</example>
        [Range(-180, 180, ErrorMessage = "Longitude deve estar entre -180 e 180.")]
        public double? Longitude { get; init; }
    }
}
