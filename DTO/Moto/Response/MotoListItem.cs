using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Moto.Response
{
    /// <summary>
    /// Item enxuto para listagens (GET /api/motos).
    /// Retorna apenas os campos principais (sem navegações).
    /// </summary>
    public record class MotoListItem
    {
        /// <summary>Identificador da moto.</summary>
        /// <example>101</example>
        public long Id { get; init; }

        /// <summary>Placa da moto.</summary>
        /// <example>ABC1D23</example>
        [StringLength(20, ErrorMessage = "A placa deve ter no máximo 20 caracteres.")]
        public string Placa { get; init; } = string.Empty;

        /// <summary>Modelo da moto.</summary>
        /// <example>CG 160 Start</example>
        [StringLength(120, ErrorMessage = "O modelo deve ter no máximo 120 caracteres.")]
        public string Modelo { get; init; } = string.Empty;

        /// <summary>Marca da moto.</summary>
        /// <example>Honda</example>
        [StringLength(120, ErrorMessage = "A marca deve ter no máximo 120 caracteres.")]
        public string Marca { get; init; } = string.Empty;

        /// <summary>Ano de fabricação.</summary>
        /// <example>2022</example>
        public int Ano { get; init; }

        /// <summary>Status operacional.</summary>
        /// <example>DISPONIVEL</example>
        [StringLength(60, ErrorMessage = "O status deve ter no máximo 60 caracteres.")]
        public string Status { get; init; } = string.Empty;

        /// <summary>ID da filial (opcional).</summary>
        /// <example>42</example>
        public long? FilialId { get; init; }
    }
}
