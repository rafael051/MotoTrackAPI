using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Moto.Response
{
    /// <summary>
    /// Retorno detalhado (GET by id / POST created).
    /// Não retorna objetos aninhados; somente IDs em relações.
    /// </summary>
    public record class MotoResponse
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

        /// <summary>Status operacional (ex.: DISPONIVEL, LOCADA, MANUTENCAO).</summary>
        /// <example>DISPONIVEL</example>
        [StringLength(60, ErrorMessage = "O status deve ter no máximo 60 caracteres.")]
        public string Status { get; init; } = string.Empty;

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

        /// <summary>
        /// Data/hora de criação do registro (audit).
        /// 💡 Mantida como <c>nullable</c> até configurar DEFAULT SYSTIMESTAMP no banco.
        /// </summary>
        /// <example>2025-10-01T12:34:56</example>
        public DateTime? DataCriacao { get; init; }
    }
}
