using System;

namespace MotoTrackAPI.Services.Exceptions
{
    /// <summary>Exceção para violações de regras de negócio (HTTP 400).</summary>
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }
    }
}
