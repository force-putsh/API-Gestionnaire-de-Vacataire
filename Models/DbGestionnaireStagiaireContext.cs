using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API_Gestionnaire_de_Vacataire.Models
{
    public partial class DbGestionnaireStagiaireContext : DbContext
    {
        public DbGestionnaireStagiaireContext()
        {
        }

        public DbGestionnaireStagiaireContext(DbContextOptions<DbGestionnaireStagiaireContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contrat> Contrat { get; set; }
        public virtual DbSet<EmploiDeTemps> EmploiDeTemps { get; set; }
        public virtual DbSet<Payement> Payement { get; set; }
        public virtual DbSet<Pointage> Pointage { get; set; }
        public virtual DbSet<Vacataire> Vacataire { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=DbGestionnaireStagiaire.;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contrat>(entity =>
            {
                entity.Property(e => e.NomCours)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NomVacataire)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SalaireHoraire).HasColumnType("money");

                entity.HasOne(d => d.IdVacataireNavigation)
                    .WithMany(p => p.Contrat)
                    .HasForeignKey(d => d.IdVacataire)
                    .HasConstraintName("FK__Contrat__IdVacat__30F848ED");
            });

            modelBuilder.Entity<EmploiDeTemps>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.NomCours)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdVacataireNavigation)
                    .WithMany(p => p.EmploiDeTemps)
                    .HasForeignKey(d => d.IdVacataire)
                    .HasConstraintName("FK__EmploiDeT__IdVac__2B3F6F97");
            });

            modelBuilder.Entity<Payement>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.SalaireActuel).HasColumnType("money");

                entity.Property(e => e.SalairePrevisionel).HasColumnType("money");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Payement)
                    .HasForeignKey<Payement>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Payement__Id__36B12243");
            });

            modelBuilder.Entity<Pointage>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.PathPhoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdVacataireNavigation)
                    .WithMany(p => p.Pointage)
                    .HasForeignKey(d => d.IdVacataire)
                    .HasConstraintName("FK__Pointage__IdVaca__25869641");
            });

            modelBuilder.Entity<Vacataire>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NumTel)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Prenom)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
