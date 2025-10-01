// File: Swagger/Examples/Usuario/UsuarioResponseExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Usuario.Response;

namespace MotoTrackAPI.Swagger.Examples.Usuario
{
    /// <summary>Exemplo de resposta detalhada (GET por ID / POST criado).</summary>
    public class UsuarioResponseExample : IExamplesProvider<UsuarioResponse>
    {
        public UsuarioResponse GetExamples() => new()
        {
            Id = 77,
            Nome = "Rafael Almeida",
            Email = "rafael@mottu.com",
            Perfil = "OPERADOR",
            FilialId = 42
        };
    }
}
