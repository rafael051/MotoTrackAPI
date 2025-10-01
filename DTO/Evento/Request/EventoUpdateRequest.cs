using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Evento.Request
{
    /// <summary>
    /// Payload para atualização de Evento.
    /// Datas seguem o formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class EventoUpdateRequest
    {
        /// <summary>Tipo do evento (ex.: MANUTENCAO, SINISTRO, DEVOLUCAO).</summary>
        /// <example>MANUTENCAO</example>
        [Required, StringLength(100)]
        public string Tipo { get; init; } = string.Empty;

        /// <summary>Motivo/descrição breve do evento.</summary>
        /// <example>Troca de pastilhas de freio</example>
        [Required, StringLength(255)]
        public string Motivo { get; init; } = string.Empty;

        /// <summary>Data/hora do evento.</summary>
        /// <example>15/10/2025 15:00:00</example>
        [Required]
        public DateTime DataHora { get; init; }

        /// <summary>Localização textual opcional.</summary>
        /// <example>Filial Vila Mariana - Box 3</example>
        [StringLength(255)]
        public string? Localizacao { get; init; }
    }
}
