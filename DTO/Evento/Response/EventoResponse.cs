using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Evento.Response
{
    /// <summary>
    /// Retorno detalhado de Evento (GET por ID / POST criado).
    /// Datas seguem o formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class EventoResponse
    {
        /// <summary>Identificador do evento.</summary>
        /// <example>123</example>
        public long Id { get; init; }

        /// <summary>ID da moto relacionada.</summary>
        /// <example>1</example>
        public long MotoId { get; init; }

        /// <summary>Tipo do evento.</summary>
        /// <example>MANUTENCAO</example>
        [StringLength(100)]
        public string Tipo { get; init; } = string.Empty;

        /// <summary>Motivo/descrição breve.</summary>
        /// <example>Troca de pastilhas de freio</example>
        [StringLength(255)]
        public string Motivo { get; init; } = string.Empty;

        /// <summary>Data/hora do evento.</summary>
        /// <example>15/10/2025 14:30:00</example>
        public DateTime DataHora { get; init; }

        /// <summary>Localização textual opcional.</summary>
        /// <example>Filial Vila Mariana - Box 3</example>
        [StringLength(255)]
        public string? Localizacao { get; init; }
    }
}
