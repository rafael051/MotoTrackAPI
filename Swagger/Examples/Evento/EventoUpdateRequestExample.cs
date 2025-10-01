// File: Swagger/Examples/Evento/EventoUpdateRequestExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Evento.Request;

namespace MotoTrackAPI.Swagger.Examples.Evento
{
    /// <summary>Exemplo de payload (PUT) para atualizar evento.</summary>
    public class EventoUpdateRequestExample : IExamplesProvider<EventoUpdateRequest>
    {
        public EventoUpdateRequest GetExamples() => new()
        {
            Tipo = "MANUTENCAO",
            Motivo = "Revisão pós-troca para checagem",
            DataHora = DateTime.Parse("15/10/2025 15:00:00"),
            Localizacao = "Filial Vila Mariana - Box 3"
        };
    }
}
