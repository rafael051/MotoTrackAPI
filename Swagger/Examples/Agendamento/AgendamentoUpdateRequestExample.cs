// File: Swagger/Examples/Agendamento/AgendamentoUpdateRequestExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Agendamento.Request;

namespace MotoTrackAPI.Swagger.Examples.Agendamento
{
    /// <summary>
    /// Exemplo de payload para atualização de agendamento (PUT).
    /// </summary>
    public class AgendamentoUpdateRequestExample : IExamplesProvider<AgendamentoUpdateRequest>
    {
        public AgendamentoUpdateRequest GetExamples() => new AgendamentoUpdateRequest
        {
            DataAgendada = DateTime.Parse("10/10/2025 10:30:00"),
            Descricao = "Ajuste de horário por indisponibilidade do cliente"
        };
    }
}
