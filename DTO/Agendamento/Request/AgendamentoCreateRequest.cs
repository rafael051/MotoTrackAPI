using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Agendamento.Request
{
    /// <summary>
    /// Payload para criação de agendamento.
    /// Relações por ID (sem objetos aninhados).
    /// Datas seguem o formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class AgendamentoCreateRequest
    {
        /// <summary>ID da moto a ser agendada.</summary>
        /// <example>1</example>
        [Required(ErrorMessage = "A moto é obrigatória.")]
        public long MotoId { get; init; }

        /// <summary>Data/hora alvo do agendamento.</summary>
        /// <example>10/10/2025 09:00:00</example>
        [Required(ErrorMessage = "A data agendada é obrigatória.")]
        public DateTime DataAgendada { get; init; }

        /// <summary>Descrição resumida do agendamento.</summary>
        /// <example>Troca de óleo e revisão de 10.000 km</example>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(255, ErrorMessage = "Descrição deve ter no máximo 255 caracteres.")]
        public string Descricao { get; init; } = string.Empty;
    }
}
