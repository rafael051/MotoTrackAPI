// File: Models/Agendamento.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.Json.Serialization; // descomente se precisar ocultar navegação no JSON

namespace MotoTrackAPI.Models
{
    /// <summary>
    /// 📅 Entidade: Agendamento
    /// Representa um agendamento (manutenção/serviço/evento) vinculado a uma moto.
    /// - Relação por ID (MotoId) + navegação (Moto)
    /// - Datas armazenadas como TIMESTAMP no Oracle
    /// - Sem Fluent API aqui; tudo o que é “de banco” (índice, default, cascade) faremos depois
    /// </summary>
    [Table("TB_AGENDAMENTO")] // nome físico da tabela no Oracle
    public class Agendamento
    {
        // ============================
        // 🔑 Identificação
        // ============================

        /// <summary>ID único do agendamento.</summary>
        [Key]
        [Column("ID_AGENDAMENTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // ============================
        // 🔗 Relacionamento (FK)
        // ============================

        /// <summary>ID da moto relacionada ao agendamento (FK obrigatória).</summary>
        [Required(ErrorMessage = "A moto é obrigatória.")]
        [Column("ID_MOTO")]
        [ForeignKey(nameof(Moto))]
        public long MotoId { get; set; }

        /// <summary>
        /// Navegação para a moto.
        /// ⚠ Se algum endpoint ainda retorna ENTIDADES, considere usar [JsonIgnore]
        /// para não vazar objetos aninhados (preferimos DTOs na API).
        /// </summary>
        // [JsonIgnore]
        public virtual Moto Moto { get; set; } = null!;

        // ============================
        // 📆 Dados do agendamento
        // ============================

        /// <summary>Data e hora programadas (TIMESTAMP no Oracle).</summary>
        [Required(ErrorMessage = "A data agendada é obrigatória.")]
        [Column("DT_AGENDADA", TypeName = "TIMESTAMP")]
        public DateTime DataAgendada { get; set; }

        /// <summary>Descrição breve do agendamento (máx. 255).</summary>
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(255, ErrorMessage = "A descrição deve ter no máximo 255 caracteres.")]
        [Column("DS_DESCRICAO")]
        public string Descricao { get; set; } = string.Empty;

        // ============================
        // 🕒 Auditoria
        // ============================

        /// <summary>
        /// Timestamp de criação.
        /// 💡 Por enquanto deixamos *nullable* porque o DEFAULT do banco (ex.: SYSTIMESTAMP)
        /// será configurado depois via Fluent/Migration. Assim evitamos falha em INSERT.
        /// </summary>
        [Column("DT_CRIACAO", TypeName = "TIMESTAMP")]
        public DateTime? DataCriacao { get; set; }
        // Quando aplicarmos Fluent:
        //   .HasDefaultValueSql("SYSTIMESTAMP").ValueGeneratedOnAdd()
        // você pode trocar para "DateTime DataCriacao { get; set; }" (não-nullable).
    }
}
