// File: Swagger/Examples/Moto/MotoCreateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Moto.Request;

namespace MotoTrackAPI.Swagger.Examples.Moto
{
    /// <summary>Exemplo de payload (POST) para criar moto.</summary>
    public class MotoCreateRequestExample : IExamplesProvider<MotoCreateRequest>
    {
        public MotoCreateRequest GetExamples() => new()
        {
            Placa = "ABC1D23",
            Modelo = "CG 160 Start",
            Marca = "Honda",
            Ano = 2022,
            Status = "DISPONIVEL",
            FilialId = 1,
            Latitude = -23.58990,
            Longitude = -46.63450
        };
    }
}
