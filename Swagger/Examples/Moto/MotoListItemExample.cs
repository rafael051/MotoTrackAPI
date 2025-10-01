// File: Swagger/Examples/Moto/MotoListItemExample.cs
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Moto.Response;

namespace MotoTrackAPI.Swagger.Examples.Moto
{
    /// <summary>Exemplo de coleção para listagem (GET /api/motos).</summary>
    public class MotoListItemExample : IExamplesProvider<IEnumerable<MotoListItem>>
    {
        public IEnumerable<MotoListItem> GetExamples() => new[]
        {
            new MotoListItem
            {
                Id = 101,
                Placa = "ABC1D23",
                Modelo = "CG 160 Start",
                Marca  = "Honda",
                Ano    = 2022,
                Status = "DISPONIVEL",
                FilialId = 42
            },
            new MotoListItem
            {
                Id = 102,
                Placa = "DEF4G56",
                Modelo = "Factor 150",
                Marca  = "Yamaha",
                Ano    = 2021,
                Status = "MANUTENCAO",
                FilialId = 43
            }
        };
    }
}
