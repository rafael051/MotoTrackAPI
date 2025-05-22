using Microsoft.EntityFrameworkCore;
using MotoTrackAPI.Models;

namespace MotoTrackAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Moto> Motos { get; set; }
        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
