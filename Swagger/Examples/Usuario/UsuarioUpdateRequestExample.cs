// File: Swagger/Examples/Usuario/UsuarioUpdateRequestExample.cs
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Usuario.Request;

namespace MotoTrackAPI.Swagger.Examples.Usuario
{
    /// <summary>Exemplo de payload (PUT) para atualizar usuário.</summary>
    public class UsuarioUpdateRequestExample : IExamplesProvider<UsuarioUpdateRequest>
    {
        public UsuarioUpdateRequest GetExamples() => new()
        {
            Nome = "Rafael Almeida",
            Email = "rafael@mottu.com",
            Perfil = "GESTOR",
            FilialId = 43
        };
    }
}
