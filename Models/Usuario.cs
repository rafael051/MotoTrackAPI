// File: Models/Usuario.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
// using System.Text.Json.Serialization; // descomente se precisar ocultar navegações ou campos no JSON

namespace MotoTrackAPI.Models
{
    /// <summary>
    /// 👤 Entidade: Usuario
    /// Representa um usuário do sistema com credenciais e perfil funcional.
    ///
    /// Notas:
    /// - Apenas Data Annotations aqui (sem Fluent). Índices, defaults, etc. serão tratados depois.
    /// - Nunca exponha a senha em DTOs de resposta; armazene apenas o HASH.
    /// </summary>
    [Table("TB_USUARIO")]                      // nome físico da tabela no Oracle
    [Index(nameof(Email), IsUnique = true)]    // e-mail único
    public class Usuario
    {
        // ===========================================================
        // 🔑 Identificação
        // ===========================================================
        /// <summary>ID único do usuário.</summary>
        [Key]
        [Column("ID_USUARIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        // ===========================================================
        // 🧑 Dados pessoais
        // ===========================================================
        /// <summary>Nome completo do usuário.</summary>
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
        [Column("NM_USUARIO")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>E-mail do usuário (deve ser único).</summary>
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(200, ErrorMessage = "O email deve ter no máximo 200 caracteres.")]
        [Column("DS_EMAIL")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Hash da senha do usuário.
        /// ⚠ Armazene SEMPRE o HASH (ex.: BCrypt/Argon2/PBKDF2). Nunca retorne em DTOs de saída.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(255, ErrorMessage = "A senha deve ter no máximo 255 caracteres.")]
        [Column("DS_SENHA")]
        // [JsonIgnore] // descomente se, por alguma razão, a entidade for serializada em respostas
        public string Senha { get; set; } = string.Empty;

        /// <summary>
        /// Perfil de acesso (ex.: OPERADOR, GESTOR, ADMINISTRADOR).
        /// 💡 Se quiser restringir valores, use validação no DTO (Regex/Enum) e/ou regra de negócio.
        /// </summary>
        [Required(ErrorMessage = "O perfil é obrigatório.")]
        [StringLength(40, ErrorMessage = "O perfil deve ter no máximo 40 caracteres.")]
        [Column("TP_PERFIL")]
        public string Perfil { get; set; } = string.Empty; // OPERADOR | GESTOR | ADMINISTRADOR

        // ===========================================================
        // 🔗 Relacionamento (Filial opcional)
        // ===========================================================
        /// <summary>ID da filial associada (opcional).</summary>
        [Column("ID_FILIAL")]
        public long? FilialId { get; set; }

        /// <summary>
        /// Navegação para a Filial.
        /// ⚠ Se a API ainda retornar ENTIDADES, considere [JsonIgnore] para não aninhar objetos.
        /// </summary>
        // [JsonIgnore]
        public virtual Filial? Filial { get; set; }
    }
}
