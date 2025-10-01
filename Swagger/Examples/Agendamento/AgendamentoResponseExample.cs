// File: Swagger/Examples/Agendamento/AgendamentoResponseExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Agendamento.Response;

namespace MotoTrackAPI.Swagger.Examples.Agendamento
{
    /// <summary>
    /// Exemplo de resposta detalhada (GET por ID / POST criado).
    /// </summary>
    public class AgendamentoResponseExample : IExamplesProvider<AgendamentoResponse>
    {
        public AgendamentoResponse GetExamples() => new AgendamentoResponse
        {
            Id = 99,
            MotoId = 1,
            DataAgendada = DateTime.Parse("10/10/2025 09:00:00"),
            Descricao = "Troca de óleo e revisão de 10.000 km",
            DataCriacao = DateTime.Parse("01/10/2025 12:34:56")
        };
    }
}
