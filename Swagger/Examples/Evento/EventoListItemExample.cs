// File: Swagger/Examples/Evento/EventoListItemExample.cs
using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Evento.Response;

namespace MotoTrackAPI.Swagger.Examples.Evento
{
    /// <summary>Exemplo de coleção para listagem (GET /api/eventos).</summary>
    public class EventoListItemExample : IExamplesProvider<IEnumerable<EventoListItem>>
    {
        public IEnumerable<EventoListItem> GetExamples() => new[]
        {
            new EventoListItem
            {
                Id = 123,
                MotoId = 1,
                Tipo = "MANUTENCAO",
                Motivo = "Troca de pastilhas de freio",
                DataHora = DateTime.Parse("15/10/2025 14:30:00")
            },
            new EventoListItem
            {
                Id = 124,
                MotoId = 2,
                Tipo = "SINISTRO",
                Motivo = "Queda sem vítima",
                DataHora = DateTime.Parse("16/10/2025 09:10:00")
            }
        };
    }
}
