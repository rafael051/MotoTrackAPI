// File: Swagger/Examples/Agendamento/AgendamentoListItemExample.cs
using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Agendamento.Response;

namespace MotoTrackAPI.Swagger.Examples.Agendamento
{
    /// <summary>
    /// Exemplo de coleção para listagem (GET /api/agendamentos).
    /// </summary>
    public class AgendamentoListItemExample : IExamplesProvider<IEnumerable<AgendamentoListItem>>
    {
        public IEnumerable<AgendamentoListItem> GetExamples() => new[]
        {
            new AgendamentoListItem
            {
                Id = 99,
                MotoId = 1,
                DataAgendada = DateTime.Parse("10/10/2025 09:00:00"),
                Descricao = "Revisão rápida e troca de óleo"
            },
            new AgendamentoListItem
            {
                Id = 100,
                MotoId = 2,
                DataAgendada = DateTime.Parse("11/10/2025 14:00:00"),
                Descricao = "Instalação de baú e checagem elétrica"
            }
        };
    }
}
