using System;
using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Agendamento.Response
{
    /// <summary>
    /// Retorno detalhado (GET por ID / POST criado).
    /// Datas seguem o formato "dd/MM/yyyy HH:mm:ss" (converter global).
    /// </summary>
    public record class AgendamentoResponse
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

        /// <summary>Descrição do agendamento.</summary>
        /// <example>Troca de óleo e revisão de 10.000 km</example>
        [StringLength(255)]
        public string? Descricao { get; init; }

        /// <summary>Data/hora de criação do registro (audit).</summary>
        /// <example>01/10/2025 12:34:56</example>
        public DateTime? DataCriacao { get; init; }
    }
}
