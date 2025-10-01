// File: Swagger/Examples/Filial/FilialCreateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Filial.Request;

namespace MotoTrackAPI.Swagger.Examples.Filial
{
    /// <summary>Exemplo de payload (POST) para criar filial.</summary>
    public class FilialCreateRequestExample : IExamplesProvider<FilialCreateRequest>
    {
        public FilialCreateRequest GetExamples() => new()
        {
            Nome = "Mottu - Vila Mariana",
            Endereco = "Rua Vergueiro, 1000",
            Bairro = "Vila Mariana",
            Cidade = "São Paulo",
            Estado = "SP",
            Cep = "04101-000",
            Latitude = -23.58990,
            Longitude = -46.63450,
            RaioGeofenceMetros = 300
        };
    }
}
