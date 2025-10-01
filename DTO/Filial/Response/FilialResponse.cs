using System.ComponentModel.DataAnnotations;

namespace MotoTrackAPI.DTO.Filial.Response
{
    /// <summary>
    /// Retorno detalhado de Filial (GET by id / POST created).
    /// Não retorna a coleção de Motos (evita objetos aninhados).
    /// Se quiser expor informações agregadas, adicione MotoCount opcional.
    /// </summary>
    public record class FilialResponse
    {
        /// <summary>Identificador da filial.</summary>
        /// <example>42</example>
        public long Id { get; init; }

        /// <summary>Nome da filial.</summary>
        /// <example>Mottu - Vila Mariana</example>
        [StringLength(150)]
        public string Nome { get; init; } = string.Empty;

        /// <summary>Endereço completo (opcional).</summary>
        /// <example>Rua Vergueiro, 1000</example>
        [StringLength(255)]
        public string? Endereco { get; init; }

        /// <summary>Bairro (opcional).</summary>
        /// <example>Vila Mariana</example>
        [StringLength(120)]
        public string? Bairro { get; init; }

        /// <summary>Cidade (opcional).</summary>
        /// <example>São Paulo</example>
        [StringLength(120)]
        public string? Cidade { get; init; }

        /// <summary>Estado (UF) opcional.</summary>
        /// <example>SP</example>
        [StringLength(60)]
        public string? Estado { get; init; }

        /// <summary>CEP (opcional).</summary>
        /// <example>04101-000</example>
        [StringLength(20)]
        public string? Cep { get; init; }

        /// <summary>Latitude (opcional).</summary>
        /// <example>-23.58990</example>
        public double? Latitude { get; init; }

        /// <summary>Longitude (opcional).</summary>
        /// <example>-46.63450</example>
        public double? Longitude { get; init; }

        /// <summary>Raio do geofence em metros (opcional).</summary>
        /// <example>300</example>
        public double? RaioGeofenceMetros { get; init; }

        /// <summary>
        /// Quantidade de motos vinculadas (opcional).
        /// Preencha via consulta agregada no controller/serviço.
        /// </summary>
        /// <example>18</example>
        public int? MotoCount { get; init; }
    }
}
