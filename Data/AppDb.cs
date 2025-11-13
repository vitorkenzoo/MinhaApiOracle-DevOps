using Microsoft.EntityFrameworkCore;
using MinhaApiOracle.Models;

namespace MinhaApiOracle.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options) {}

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Certificado> Certificados { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<LogAuditoria> LogAuditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de índices únicos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Cpf)
                .IsUnique();

            modelBuilder.Entity<Empresa>()
                .HasIndex(e => e.Cnpj)
                .IsUnique();

            // Configuração de relacionamentos
            modelBuilder.Entity<Certificado>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Certificados)
                .HasForeignKey(c => c.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Certificado>()
                .HasOne(c => c.Curso)
                .WithMany(cur => cur.Certificados)
                .HasForeignKey(c => c.IdCurso)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vaga>()
                .HasOne(v => v.Empresa)
                .WithMany(e => e.Vagas)
                .HasForeignKey(v => v.IdEmpresa)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
