using System;

namespace MotoTrackAPI.Services.Exceptions
{
    /// <summary>Exceção de recursos não encontrados (HTTP 404).</summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
