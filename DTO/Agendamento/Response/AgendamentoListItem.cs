using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Agendamento.Response
{
    /// <summary>
    /// Item enxuto para listagens (GET /api/agendamentos).
    /// Datas no formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class AgendamentoListItem
    {
        /// <summary>Identificador do agendamento.</summary>
        /// <example>99</example>
        public long Id { get; init; }

        /// <summary>ID da moto vinculada.</summary>
        /// <example>1</example>
        public long MotoId { get; init; }

        /// <summary>Data/hora agendada.</summary>
        /// <example>10/10/2025 09:00:00</example>
        public DateTime DataAgendada { get; init; }

        /// <summary>Descrição breve do agendamento.</summary>
        /// <example>Revisão rápida e troca de óleo</example>
        [StringLength(255)]
        public string? Descricao { get; init; }
    }
}
