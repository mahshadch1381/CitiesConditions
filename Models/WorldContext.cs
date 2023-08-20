using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBFIRST_Cities3.Models;

public partial class WorldContext : DbContext
{
    public WorldContext()
    {
    }

    public WorldContext(DbContextOptions<WorldContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CompeleNameofcity> CompeleNameofcities { get; set; }

    public virtual DbSet<Country1> Country1s { get; set; }

    public virtual DbSet<Humidity> Humiditys { get; set; }

    public virtual DbSet<Latitude> Latitudes { get; set; }

    public virtual DbSet<Longitude> Longitudes { get; set; }

    public virtual DbSet<Pop> Pops { get; set; }

    public virtual DbSet<Temperature> Temperatures { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ACADEMY11\\SQLEXPRESS;Initial Catalog=world;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.Property(e => e.CityName).HasMaxLength(50);
            entity.Property(e => e.CompeleteNameId).HasColumnName("compeleteNameID");
            entity.Property(e => e.CountryId).HasColumnName("countryId");
            entity.Property(e => e.HumidityId).HasColumnName("humidityID");
            entity.Property(e => e.Modifitime)
                .HasMaxLength(50)
                .HasColumnName("modifitime");
            entity.Property(e => e.PopulationId).HasColumnName("populationID");
            entity.Property(e => e.TempId).HasColumnName("TempID");

            entity.HasOne(d => d.CompeleteName).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CompeleteNameId)
                .HasConstraintName("FK_Cities_Cities");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_Cities_COUNTRY1");

            entity.HasOne(d => d.Humidity).WithMany(p => p.Cities)
                .HasForeignKey(d => d.HumidityId)
                .HasConstraintName("FK_Cities_Humiditys");

            entity.HasOne(d => d.Lat).WithMany(p => p.Cities)
                .HasForeignKey(d => d.LatId)
                .HasConstraintName("FK_Cities_Latitudes");

            entity.HasOne(d => d.Long).WithMany(p => p.Cities)
                .HasForeignKey(d => d.LongId)
                .HasConstraintName("FK_Cities_Longitudes");

            entity.HasOne(d => d.Population).WithMany(p => p.Cities)
                .HasForeignKey(d => d.PopulationId)
                .HasConstraintName("FK_Cities_pop");

            entity.HasOne(d => d.Temp).WithMany(p => p.Cities)
                .HasForeignKey(d => d.TempId)
                .HasConstraintName("FK_Cities_Temperatures");
        });

        modelBuilder.Entity<CompeleNameofcity>(entity =>
        {
            entity.HasKey(e => e.CompId);

            entity.ToTable("compeleNameofcity");

            entity.Property(e => e.CompId).HasColumnName("CompID");
            entity.Property(e => e.CopmName).HasMaxLength(50);
        });

        modelBuilder.Entity<Country1>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("COUNTRY1");

            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Humidity>(entity =>
        {
            entity.Property(e => e.Humidity1).HasColumnName("Humidity");
        });

        modelBuilder.Entity<Latitude>(entity =>
        {
            entity.Property(e => e.Latitude1)
                .HasMaxLength(50)
                .HasColumnName("Latitude");
        });

        modelBuilder.Entity<Longitude>(entity =>
        {
            entity.Property(e => e.Longitude1)
                .HasMaxLength(50)
                .HasColumnName("Longitude");
        });

        modelBuilder.Entity<Pop>(entity =>
        {
            entity.ToTable("pop");

            entity.Property(e => e.PopId).HasColumnName("popID");
        });

        modelBuilder.Entity<Temperature>(entity =>
        {
            entity.HasKey(e => e.TempId);

            entity.Property(e => e.TempId).HasColumnName("TempID");
            entity.Property(e => e.Temperature1).HasColumnName("Temperature");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
