// File: Swagger/Examples/Evento/EventoCreateRequestExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Evento.Request;

namespace MotoTrackAPI.Swagger.Examples.Evento
{
    /// <summary>Exemplo de payload (POST) para criar evento.</summary>
    public class EventoCreateRequestExample : IExamplesProvider<EventoCreateRequest>
    {
        public EventoCreateRequest GetExamples() => new()
        {
            MotoId = 1,
            Tipo = "MANUTENCAO",
            Motivo = "Troca de pastilhas de freio",
            DataHora = DateTime.Parse("15/10/2025 14:30:00"),
            Localizacao = "Filial Vila Mariana - Box 3"
        };
    }
}
