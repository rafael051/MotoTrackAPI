// File: Models/Filial.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.Json.Serialization; // descomente se precisar ocultar navegações no JSON

namespace MotoTrackAPI.Models
{
    /// <summary>
    /// 🏢 Entidade: Filial
    /// Representa uma unidade (polo) com endereço e geolocalização.
    /// Usada para alocar motos e controlar geofencing.
    ///
    /// Notas:
    /// - Apenas Data Annotations aqui (sem Fluent); índices/precisão/defaults ficam para depois.
    /// - Relação 1:N com Moto (coleção de navegação).
    /// </summary>
    [Table("TB_FILIAL")] // nome físico da tabela no Oracle
    public class Filial
    {
        // ===========================================================
        // 🔑 Identificação
        // ===========================================================
        /// <summary>ID único da filial.</summary>
        [Key]
        [Column("ID_FILIAL")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // ===========================================================
        // 🏷️ Dados da Filial
        // ===========================================================
        /// <summary>Nome da filial (obrigatório, até 150 chars).</summary>
        [Required(ErrorMessage = "O nome da filial é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        [Column("NM_FILIAL")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>Endereço (rua/avenida e número).</summary>
        [StringLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres.")]
        [Column("DS_ENDERECO")]
        public string? Endereco { get; set; }

        /// <summary>Bairro.</summary>
        [StringLength(120, ErrorMessage = "O bairro deve ter no máximo 120 caracteres.")]
        [Column("DS_BAIRRO")]
        public string? Bairro { get; set; }

        /// <summary>Cidade.</summary>
        [StringLength(120, ErrorMessage = "A cidade deve ter no máximo 120 caracteres.")]
        [Column("DS_CIDADE")]
        public string? Cidade { get; set; }

        /// <summary>Estado (UF ou nome; ajuste conforme sua regra).</summary>
        [StringLength(60, ErrorMessage = "O estado deve ter no máximo 60 caracteres.")]
        [Column("DS_ESTADO")]
        public string? Estado { get; set; }

        /// <summary>CEP (apenas formato texto; validação de máscara pode ser aplicada no DTO).</summary>
        [StringLength(20, ErrorMessage = "O CEP deve ter no máximo 20 caracteres.")]
        [Column("NR_CEP")]
        public string? Cep { get; set; }

        // ===========================================================
        // 🌐 Geolocalização
        // ===========================================================
        /// <summary>Latitude em graus decimais (ex.: -23.5629). Nullable.</summary>
        [Column("VL_LATITUDE")]
        public double? Latitude { get; set; }

        /// <summary>Longitude em graus decimais (ex.: -46.6544). Nullable.</summary>
        [Column("VL_LONGITUDE")]
        public double? Longitude { get; set; }

        /// <summary>Raio do geofence em metros (ex.: 150.0). Nullable.</summary>
        [Column("RAIO_GEOFENCE_M")]
        public double? RaioGeofenceMetros { get; set; }

        // ===========================================================
        // 🔗 Relacionamentos
        // ===========================================================
        /// <summary>
        /// Coleção de motos alocadas nesta filial (1:N).
        /// ⚠ Se você ainda retornar ENTIDADES na API, considere ocultar para evitar loops:
        ///    [JsonIgnore]
        /// </summary>
        // [JsonIgnore]
        public virtual ICollection<Moto> Motos { get; set; } = new List<Moto>();
    }
}
