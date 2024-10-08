﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MicroservicioClientePersona.Models;

public partial class DB_ClientePersonaContext : DbContext
{
    public DB_ClientePersonaContext(DbContextOptions<DB_ClientePersonaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CLIENTE> CLIENTE { get; set; }

    public virtual DbSet<PERSONA> PERSONA { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CLIENTE>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__CLIENTE__71ABD0878C3A576A");

            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Persona).WithMany(p => p.CLIENTE)
                .HasForeignKey(d => d.PersonaId)
                .HasConstraintName("FK__CLIENTE__Persona__3A81B327");
        });

        modelBuilder.Entity<PERSONA>(entity =>
        {
            entity.HasKey(e => e.PersonaId).HasName("PK__PERSONA__7C34D303B683F70E");

            entity.HasIndex(e => e.Identificacion, "UQ__PERSONA__D6F931E5D63B04D4").IsUnique();

            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.Genero)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Identificacion)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}