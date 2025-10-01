// File: Swagger/Examples/Usuario/UsuarioListItemExample.cs
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MotoTrackAPI.DTO.Usuario.Response;

namespace MotoTrackAPI.Swagger.Examples.Usuario
{
    /// <summary>Exemplo de coleção para listagem (GET /api/usuarios).</summary>
    public class UsuarioListItemExample : IExamplesProvider<IEnumerable<UsuarioListItem>>
    {
        public IEnumerable<UsuarioListItem> GetExamples() => new[]
        {
            new UsuarioListItem
            {
                Id = 77,
                Nome = "Rafael Almeida",
                Email = "rafael@mottu.com",
                Perfil = "OPERADOR",
                FilialId = 42
            },
            new UsuarioListItem
            {
                Id = 78,
                Nome = "Carla Nunes",
                Email = "carla@mottu.com",
                Perfil = "ADMINISTRADOR",
                FilialId = null
            }
        };
    }
}
