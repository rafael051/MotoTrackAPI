// File: Models/Moto.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using System.Text.Json.Serialization; // descomente se precisar ocultar navegações no JSON

namespace MotoTrackAPI.Models
{
    /// <summary>
    /// 🛵 Entidade: Moto
    /// Representa uma motocicleta cadastrada no sistema.
    ///
    /// Notas:
    /// - Apenas Data Annotations aqui (sem Fluent); índice, precisão e defaults viremos depois.
    /// - Relação opcional com Filial (FilialId pode ser nulo).
    /// - Datas armazenadas como TIMESTAMP (Oracle).
    /// </summary>
    [Table("TB_MOTO")]                      // nome físico da tabela
    [Index(nameof(Placa), IsUnique = true)] // índice único na placa (garante unicidade)
    public class Moto
    {
        // ===========================================================
        // 🔑 Identificação
        // ===========================================================
        /// <summary>ID único da moto.</summary>
        [Key]
        [Column("ID_MOTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Placa única (ex.: ABC1D23 ou ABC1234).</summary>
        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(20, ErrorMessage = "A placa deve ter no máximo 20 caracteres.")]
        [Column("CD_PLACA")]
        public string Placa { get; set; } = string.Empty;

        // ===========================================================
        // 📋 Especificações
        // ===========================================================
        /// <summary>Modelo (ex.: CG 160, NMax 160).</summary>
        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(120, ErrorMessage = "O modelo deve ter no máximo 120 caracteres.")]
        [Column("NM_MODELO")] // ⬅️ ajustado para ficar consistente com convenção "NM_" do Oracle
        public string Modelo { get; set; } = string.Empty;

        /// <summary>Marca (ex.: Honda, Yamaha).</summary>
        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(120, ErrorMessage = "A marca deve ter no máximo 120 caracteres.")]
        [Column("NM_MARCA")] // ⬅️ ajustado para ficar consistente com convenção "NM_" do Oracle
        public string Marca { get; set; } = string.Empty;

        /// <summary>Ano do veículo (entre 2000 e 2100).</summary>
        [Range(2000, 2100, ErrorMessage = "O ano deve ser entre 2000 e 2100.")]
        [Column("NR_ANO")]
        public int Ano { get; set; }

        /// <summary>Status operacional (ex.: Disponível, Em Manutenção, Alugada).</summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(60, ErrorMessage = "O status deve ter no máximo 60 caracteres.")]
        [Column("DS_STATUS")]
        public string Status { get; set; } = string.Empty;

        // ===========================================================
        // 🔗 Relacionamento (Filial opcional)
        // ===========================================================
        /// <summary>ID da filial onde a moto está alocada (opcional).</summary>
        [Column("ID_FILIAL")]
        public long? FilialId { get; set; }

        /// <summary>
        /// Navegação para a Filial.
        /// ⚠ Se a API ainda retornar ENTIDADES, considere [JsonIgnore] para não aninhar objetos.
        /// </summary>
        // [JsonIgnore]
        public virtual Filial? Filial { get; set; }

        // (Opcional) Navegações inversas, se desejar acessar eventos/agendamentos da moto:
        // public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();
        // public virtual ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();

        // ===========================================================
        // 🌐 Localização
        // ===========================================================
        /// <summary>Latitude em graus decimais (ex.: -23.562900). Nullable.</summary>
        [Column("VL_LATITUDE")]
        public double? Latitude { get; set; }

        /// <summary>Longitude em graus decimais (ex.: -46.654400). Nullable.</summary>
        [Column("VL_LONGITUDE")]
        public double? Longitude { get; set; }

        // ===========================================================
        // 🕒 Auditoria
        // ===========================================================
        /// <summary>
        /// Data/hora de criação do registro.
        /// 💡 Por enquanto deixamos *nullable*. Depois, no Fluent/Migration,
        /// configuraremos `DEFAULT SYSTIMESTAMP` no Oracle e poderemos torná-la não-nullable.
        /// </summary>
        [Column("DT_CRIACAO", TypeName = "TIMESTAMP")]
        public DateTime? DataCriacao { get; set; }
        // Quando aplicarmos Fluent:
        //   .HasDefaultValueSql("SYSTIMESTAMP").ValueGeneratedOnAdd()
        // você pode trocar para "DateTime DataCriacao { get; set; }" (não-nullable).
    }
}
