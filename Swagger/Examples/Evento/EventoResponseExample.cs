// File: Swagger/Examples/Evento/EventoResponseExample.cs
using System;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Evento.Response;

namespace MotoTrackAPI.Swagger.Examples.Evento
{
    /// <summary>Exemplo de resposta detalhada (GET por ID / POST criado).</summary>
    public class EventoResponseExample : IExamplesProvider<EventoResponse>
    {
        public EventoResponse GetExamples() => new()
        {
            Id = 123,
            MotoId = 1,
            Tipo = "MANUTENCAO",
            Motivo = "Troca de pastilhas de freio",
            DataHora = DateTime.Parse("15/10/2025 14:30:00"),
            Localizacao = "Filial Vila Mariana - Box 3"
        };
    }
}
