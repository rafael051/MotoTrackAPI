using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Models;

namespace MotoTrackAPI.Data
{
    /// <summary>
    /// DbContext da aplicação.
    /// Versão com "tuning" Fluent:
    /// - Índices nomeados
    /// - Defaults de data/hora via SYSTIMESTAMP
    /// - Regras claras de deleção
    /// - Converte bool → NUMBER(1) (Oracle não suporta BOOLEAN em tabelas)
    /// - (Opcional) precisão para latitude/longitude
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<Moto> Motos { get; set; } = null!;
        public DbSet<Filial> Filiais { get; set; } = null!;
        public DbSet<Evento> Eventos { get; set; } = null!;
        public DbSet<Agendamento> Agendamentos { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;

        // ✅ Converte todos os bools implicitamente para NUMBER(1)
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<bool>()
                   .HaveConversion<short>()       // armazena como 0/1
                   .HaveColumnType("NUMBER(1)");

            builder.Properties<bool?>()
                   .HaveConversion<short?>()
                   .HaveColumnType("NUMBER(1)");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ❌ Sem HasDefaultSchema (schema definido fora, na string de conexão ou manualmente)

            // ===========================
            // 🛵 MOTO
            // ===========================
            modelBuilder.Entity<Moto>(e =>
            {
                e.ToTable("TB_MOTO");

                e.HasIndex(x => x.Placa)
                 .IsUnique()
                 .HasDatabaseName("UX_MOTO_PLACA");

                e.HasOne(x => x.Filial)
                 .WithMany(f => f.Motos)
                 .HasForeignKey(x => x.FilialId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.Property(x => x.DataCriacao)
                 .HasColumnType("TIMESTAMP")
                 .HasDefaultValueSql("SYSTIMESTAMP")
                 .ValueGeneratedOnAdd();
            });

            // ===========================
            // 🏢 FILIAL
            // ===========================
            modelBuilder.Entity<Filial>(e =>
            {
                e.ToTable("TB_FILIAL");

                e.HasMany(f => f.Motos)
                 .WithOne(m => m.Filial)
                 .HasForeignKey(m => m.FilialId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ===========================
            // 🔄 EVENTO
            // ===========================
            modelBuilder.Entity<Evento>(e =>
            {
                e.ToTable("TB_EVENTO");

                e.HasIndex(x => x.MotoId).HasDatabaseName("IX_EVENTO_MOTO");

                e.HasOne(x => x.Moto)
                 .WithMany()
                 .HasForeignKey(x => x.MotoId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.DataHora)
                 .HasColumnType("TIMESTAMP")
                 .HasDefaultValueSql("SYSTIMESTAMP")
                 .ValueGeneratedOnAdd();
            });

            // ===========================
            // 📅 AGENDAMENTO
            // ===========================
            modelBuilder.Entity<Agendamento>(e =>
            {
                e.ToTable("TB_AGENDAMENTO");

                e.HasIndex(x => x.MotoId).HasDatabaseName("IX_AGENDAMENTO_MOTO");

                e.HasOne(x => x.Moto)
                 .WithMany()
                 .HasForeignKey(x => x.MotoId)
                 .IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.DataCriacao)
                 .HasColumnType("TIMESTAMP")
                 .HasDefaultValueSql("SYSTIMESTAMP")
                 .ValueGeneratedOnAdd();
            });

            // ===========================
            // 👤 USUARIO
            // ===========================
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("TB_USUARIO");

                e.HasIndex(x => x.Email)
                 .IsUnique()
                 .HasDatabaseName("UX_USUARIO_EMAIL");

                e.HasOne(x => x.Filial)
                 .WithMany()
                 .HasForeignKey(x => x.FilialId)
                 .OnDelete(DeleteBehavior.Restrict);

                // exemplo de flag bool → número
                // e.Property(x => x.Ativo)
                //  .HasColumnName("FL_ATIVO")
                //  .HasDefaultValue((short)1); 
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
