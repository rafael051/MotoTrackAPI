// File: Swagger/Examples/Filial/FilialListItemExample.cs
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Filial.Response;

namespace MotoTrackAPI.Swagger.Examples.Filial
{
    /// <summary>Exemplo de coleção para listagem (GET /api/filiais).</summary>
    public class FilialListItemExample : IExamplesProvider<IEnumerable<FilialListItem>>
    {
        public IEnumerable<FilialListItem> GetExamples() => new[]
        {
            new FilialListItem
            {
                Id = 42,
                Nome = "Mottu - Vila Mariana",
                Endereco = "Rua Vergueiro, 1000",
                Bairro = "Vila Mariana",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "04101-000",
                Latitude = -23.58990,
                Longitude = -46.63450,
                RaioGeofenceMetros = 300
            },
            new FilialListItem
            {
                Id = 43,
                Nome = "Mottu - Moema",
                Endereco = "Av. Ibirapuera, 2500",
                Bairro = "Moema",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "04028-001",
                Latitude = -23.60123,
                Longitude = -46.66321,
                RaioGeofenceMetros = 250
            }
        };
    }
}
