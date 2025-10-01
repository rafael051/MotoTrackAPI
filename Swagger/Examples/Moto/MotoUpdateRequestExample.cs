// File: Swagger/Examples/Moto/MotoUpdateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Moto.Request;

namespace MotoTrackAPI.Swagger.Examples.Moto
{
    /// <summary>Exemplo de payload (PUT) para atualizar moto.</summary>
    public class MotoUpdateRequestExample : IExamplesProvider<MotoUpdateRequest>
    {
        public MotoUpdateRequest GetExamples() => new()
        {
            Placa = "ABC1D23",
            Modelo = "CG 160 Start",
            Marca = "Honda",
            Ano = 2023,
            Status = "LOCADA",
            FilialId = 43,
            Latitude = -23.59021,
            Longitude = -46.63388
        };
    }
}
