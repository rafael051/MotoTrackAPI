// File: Swagger/Examples/Moto/MotoResponseExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Moto.Response;

namespace MotoTrackAPI.Swagger.Examples.Moto
{
    /// <summary>Exemplo de resposta detalhada (GET por ID / POST criado).</summary>
    public class MotoResponseExample : IExamplesProvider<MotoResponse>
    {
        public MotoResponse GetExamples() => new()
        {
            Id = 101,
            Placa = "ABC1D23",
            Modelo = "CG 160 Start",
            Marca = "Honda",
            Ano = 2022,
            Status = "DISPONIVEL",
            FilialId = 42,
            Latitude = -23.58990,
            Longitude = -46.63450,
            DataCriacao = DateTime.Parse("01/10/2025 12:34:56")
        };
    }
}
