// File: Models/Evento.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.Json.Serialization; // descomente se precisar ocultar a navegação no JSON

namespace MotoTrackAPI.Models
{
    /// <summary>
    /// 🔄 Entidade: Evento
    /// Registra uma ocorrência relacionada à moto (ex.: "Saída", "Manutenção", "Realocação").
    ///
    /// Notas:
    /// - Relação por ID (MotoId) + navegação (Moto).
    /// - Datas armazenadas como TIMESTAMP (Oracle).
    /// - Nada de Fluent API aqui; ajustes de banco (índice, default, cascade) faremos depois.
    /// </summary>
    [Table("TB_EVENTO")] // nome físico da tabela no Oracle
    public class Evento
    {
        // ===========================================================
        // 🔑 Identificação
        // ===========================================================
        /// <summary>ID único do evento.</summary>
        [Key]
        [Column("ID_EVENTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // ===========================================================
        // 🔗 Relacionamento (FK obrigatória para Moto)
        // ===========================================================
        /// <summary>ID da moto relacionada ao evento.</summary>
        [Required(ErrorMessage = "A moto é obrigatória.")]
        [Column("ID_MOTO")]
        [ForeignKey(nameof(Moto))]
        public long MotoId { get; set; }

        /// <summary>
        /// Propriedade de navegação para a Moto.
        /// ⚠ Se algum endpoint ainda retornar ENTIDADES, considere usar [JsonIgnore]
        /// para não vazar objetos aninhados (preferimos DTOs na camada de API).
        /// </summary>
        // [JsonIgnore]
        public virtual Moto Moto { get; set; } = null!;

        // ===========================================================
        // 🏷️ Detalhes do Evento
        // ===========================================================
        /// <summary>
        /// Tipo do evento (ex.: "Saída", "Entrada", "Manutenção", "Realocação").
        /// </summary>
        [Required(ErrorMessage = "O tipo do evento é obrigatório.")]
        [StringLength(100, ErrorMessage = "O tipo deve ter no máximo 100 caracteres.")]
        [Column("TP_EVENTO")]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Motivo/descritivo da ocorrência (até 255 caracteres).
        /// </summary>
        [Required(ErrorMessage = "O motivo do evento é obrigatório.")]
        [StringLength(255, ErrorMessage = "O motivo deve ter no máximo 255 caracteres.")]
        [Column("DS_MOTIVO")]
        public string Motivo { get; set; } = string.Empty;

        /// <summary>
        /// Data e hora do evento.
        /// 💡 Se quiser timestamp automático do banco (SYSTIMESTAMP),
        /// faremos via Fluent/Migration depois; por isso NÃO definimos valor default aqui.
        /// </summary>
        [Required(ErrorMessage = "A data/hora do evento é obrigatória.")]
        [Column("DT_HR_EVENTO", TypeName = "TIMESTAMP")]
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Localização textual (ex.: "Filial Centro", "Oficina A", "KM 12, Av. Brasil").
        /// </summary>
        [StringLength(255, ErrorMessage = "A localização deve ter no máximo 255 caracteres.")]
        [Column("DS_LOCALIZACAO")]
        public string? Localizacao { get; set; }
    }
}
