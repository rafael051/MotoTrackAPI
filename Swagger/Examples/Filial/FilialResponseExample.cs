// File: Swagger/Examples/Filial/FilialResponseExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Filial.Response;

namespace MotoTrackAPI.Swagger.Examples.Filial
{
    /// <summary>Exemplo de resposta detalhada (GET por ID / POST criado).</summary>
    public class FilialResponseExample : IExamplesProvider<FilialResponse>
    {
        public FilialResponse GetExamples() => new()
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
            RaioGeofenceMetros = 300,
            MotoCount = 18
        };
    }
}
