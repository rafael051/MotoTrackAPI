// File: Swagger/Examples/Usuario/UsuarioCreateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Usuario.Request;

namespace MotoTrackAPI.Swagger.Examples.Usuario
{
    /// <summary>Exemplo de payload (POST) para criar usuário.</summary>
    public class UsuarioCreateRequestExample : IExamplesProvider<UsuarioCreateRequest>
    {
        public UsuarioCreateRequest GetExamples() => new()
        {
            Nome = "Rafael Almeida",
            Email = "rafael@mottu.com",
            Senha = "Str0ng@123", // ⚠ somente em request
            Perfil = "OPERADOR",
            FilialId = 42
        };
    }
}
