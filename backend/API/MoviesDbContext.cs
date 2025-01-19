using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API;

public partial class MoviesDbContext : DbContext
{
    public MoviesDbContext()
    {
    }

    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Filmovi> Filmovis { get; set; }

    public virtual DbSet<Glumci> Glumcis { get; set; }

    public virtual DbSet<Redatelji> Redateljis { get; set; }

    public virtual DbSet<Zanrovi> Zanrovis { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=or-labos;Username=postgres;Password=sime");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Filmovi>(entity =>
        {
            entity.HasKey(e => e.FilmId).HasName("filmovi_pkey");

            entity.ToTable("filmovi");

            entity.Property(e => e.FilmId).HasColumnName("film_id");
            entity.Property(e => e.Budzet).HasColumnName("budzet");
            entity.Property(e => e.Godina).HasColumnName("godina");
            entity.Property(e => e.ImeDistributera)
                .HasMaxLength(255)
                .HasColumnName("ime_distributera");
            entity.Property(e => e.KratkiOpis).HasColumnName("kratki_opis");
            entity.Property(e => e.Naziv)
                .HasMaxLength(255)
                .HasColumnName("naziv");
            entity.Property(e => e.Prihod).HasColumnName("prihod");
            entity.Property(e => e.ProsjecnaOcjena).HasColumnName("prosjecna_ocjena");
            entity.Property(e => e.RedateljId).HasColumnName("redatelj_id");
            entity.Property(e => e.Trajanje).HasColumnName("trajanje");
            entity.Property(e => e.TvpgOcjena).HasColumnName("tvpg_ocjena");
            entity.Property(e => e.Zemlja)
                .HasMaxLength(100)
                .HasColumnName("zemlja");

            entity.HasOne(d => d.Redatelj).WithMany(p => p.Filmovis)
                .HasForeignKey(d => d.RedateljId)
                .HasConstraintName("fk_redatelj");

            entity.HasMany(d => d.Glumacs).WithMany(p => p.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmoviGlumci",
                    r => r.HasOne<Glumci>().WithMany()
                        .HasForeignKey("GlumacId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_glumac"),
                    l => l.HasOne<Filmovi>().WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_film"),
                    j =>
                    {
                        j.HasKey("FilmId", "GlumacId").HasName("filmovi_glumci_pkey");
                        j.ToTable("filmovi_glumci");
                        j.IndexerProperty<int>("FilmId").HasColumnName("film_id");
                        j.IndexerProperty<int>("GlumacId").HasColumnName("glumac_id");
                    });

            entity.HasMany(d => d.Zanrs).WithMany(p => p.Films)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmoviZanrovi",
                    r => r.HasOne<Zanrovi>().WithMany()
                        .HasForeignKey("ZanrId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_zanr"),
                    l => l.HasOne<Filmovi>().WithMany()
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_film"),
                    j =>
                    {
                        j.HasKey("FilmId", "ZanrId").HasName("filmovi_zanrovi_pkey");
                        j.ToTable("filmovi_zanrovi");
                        j.IndexerProperty<int>("FilmId").HasColumnName("film_id");
                        j.IndexerProperty<int>("ZanrId").HasColumnName("zanr_id");
                    });
        });

        modelBuilder.Entity<Glumci>(entity =>
        {
            entity.HasKey(e => e.GlumacId).HasName("glumci_pkey");

            entity.ToTable("glumci");

            entity.Property(e => e.GlumacId).HasColumnName("glumac_id");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .HasColumnName("ime");
            entity.Property(e => e.Prezime)
                .HasMaxLength(100)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Redatelji>(entity =>
        {
            entity.HasKey(e => e.RedateljId).HasName("redatelji_pkey");

            entity.ToTable("redatelji");

            entity.Property(e => e.RedateljId).HasColumnName("redatelj_id");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .HasColumnName("ime");
            entity.Property(e => e.Prezime)
                .HasMaxLength(100)
                .HasColumnName("prezime");
        });

        modelBuilder.Entity<Zanrovi>(entity =>
        {
            entity.HasKey(e => e.ZanrId).HasName("zanr_pkey");

            entity.ToTable("zanrovi");

            entity.Property(e => e.ZanrId)
                .HasDefaultValueSql("nextval('zanr_zanr_id_seq'::regclass)")
                .HasColumnName("zanr_id");
            entity.Property(e => e.Ime)
                .HasMaxLength(100)
                .HasColumnName("ime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
