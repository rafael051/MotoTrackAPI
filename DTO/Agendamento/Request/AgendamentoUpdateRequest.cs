using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Agendamento.Request
{
    /// <summary>
    /// Payload para atualização (PUT) do agendamento.
    /// Datas seguem o formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class AgendamentoUpdateRequest
    {
        /// <summary>Nova data/hora do agendamento.</summary>
        /// <example>10/10/2025 10:30:00</example>
        [Required(ErrorMessage = "A data agendada é obrigatória.")]
        public DateTime DataAgendada { get; init; }

        /// <summary>Nova descrição do agendamento.</summary>
        /// <example>Ajuste de horário por indisponibilidade do cliente</example>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(255, ErrorMessage = "Descrição deve ter no máximo 255 caracteres.")]
        public string Descricao { get; init; } = string.Empty;
    }
}
