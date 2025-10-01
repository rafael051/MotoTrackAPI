// File: Swagger/Examples/Agendamento/AgendamentoCreateRequestExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Agendamento.Request;

namespace MotoTrackAPI.Swagger.Examples.Agendamento
{
    /// <summary>
    /// Exemplo completo de payload para criação de agendamento (POST).
    /// </summary>
    public class AgendamentoCreateRequestExample : IExamplesProvider<AgendamentoCreateRequest>
    {
        /// <summary>
        /// Retorna um exemplo pronto, alinhado ao DTO atual (MotoId, DataAgendada, Descricao).
        /// </summary>
        public AgendamentoCreateRequest GetExamples() => new AgendamentoCreateRequest
        {
            MotoId = 1,
            DataAgendada = DateTime.Parse("10/10/2025 09:00:00"),
            Descricao = "Troca de óleo e revisão de 10.000 km"
        };
    }
}
