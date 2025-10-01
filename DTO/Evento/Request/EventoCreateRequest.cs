using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Evento.Request
{
    /// <summary>Payload para criação de Evento (relações via IDs).</summary>
    public record class EventoCreateRequest
    {
        /// <summary>ID da moto relacionada ao evento.</summary>
        /// <example>1</example>
        [Required(ErrorMessage = "A moto é obrigatória.")]
        public long MotoId { get; init; }

        /// <summary>Tipo do evento (ex.: MANUTENCAO, SINISTRO, DEVOLUCAO).</summary>
        /// <example>MANUTENCAO</example>
        [Required, StringLength(100, ErrorMessage = "Tipo até 100 chars.")]
        public string Tipo { get; init; } = string.Empty;

        /// <summary>Motivo/descrição breve do evento.</summary>
        /// <example>Troca de pastilhas de freio</example>
        [Required, StringLength(255, ErrorMessage = "Motivo até 255 chars.")]
        public string Motivo { get; init; } = string.Empty;

        /// <summary>Data/hora do evento.</summary>
        /// <example>15/10/2025 14:30:00</example>
        [Required(ErrorMessage = "A data/hora do evento é obrigatória.")]
        public DateTime DataHora { get; init; }

        /// <summary>Localização textual opcional.</summary>
        /// <example>Filial Vila Mariana - Box 3</example>
        [StringLength(255, ErrorMessage = "Localização até 255 chars.")]
        public string? Localizacao { get; init; }
    }
}
