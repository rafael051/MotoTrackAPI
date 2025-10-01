// File: Swagger/Examples/Filial/FilialUpdateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Filial.Request;

namespace MotoTrackAPI.Swagger.Examples.Filial
{
    /// <summary>Exemplo de payload (PUT) para atualizar filial.</summary>
    public class FilialUpdateRequestExample : IExamplesProvider<FilialUpdateRequest>
    {
        public FilialUpdateRequest GetExamples() => new()
        {
            Nome = "Mottu - Vila Mariana (Atualizada)",
            Endereco = "Rua Vergueiro, 1200",
            Bairro = "Vila Mariana",
            Cidade = "São Paulo",
            Estado = "SP",
            Cep = "04101-100",
            Latitude = -23.59021,
            Longitude = -46.63388,
            RaioGeofenceMetros = 350
        };
    }
}
