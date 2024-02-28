﻿using System;
using System.Collections.Generic;
using ApiSportTogether.model.ObjectContext;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ApiSportTogether.model.dbContext;

public partial class SportTogetherContext : DbContext
{
    public SportTogetherContext()
    {
    }

    public SportTogetherContext(DbContextOptions<SportTogetherContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ami> Amis { get; set; }

    public virtual DbSet<Annonce> Annonces { get; set; }

    public virtual DbSet<Groupe> Groupes { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=sport-together;port=3306", ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Ami>(entity =>
        {
            entity.HasKey(e => e.AmisId).HasName("PRIMARY");

            entity.ToTable("amis");

            entity.HasIndex(e => e.UtilisateurId1, "fk_amis_UtilisateurID1");

            entity.HasIndex(e => e.UtilisateurId2, "fk_amis_UtilisateurID2");

            entity.Property(e => e.AmisId)
                .ValueGeneratedNever()
                .HasColumnName("amis_id");
            entity.Property(e => e.DateAjout).HasColumnType("datetime");
            entity.Property(e => e.UtilisateurId1).HasColumnName("UtilisateurID1");
            entity.Property(e => e.UtilisateurId2).HasColumnName("UtilisateurID2");

            entity.HasOne(d => d.UtilisateurId1Navigation).WithMany(p => p.AmiUtilisateurId1Navigations)
                .HasForeignKey(d => d.UtilisateurId1)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_amis_UtilisateurID1");

            entity.HasOne(d => d.UtilisateurId2Navigation).WithMany(p => p.AmiUtilisateurId2Navigations)
                .HasForeignKey(d => d.UtilisateurId2)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_amis_UtilisateurID2");
        });

        modelBuilder.Entity<Annonce>(entity =>
        {
            entity.HasKey(e => e.AnnoncesId).HasName("PRIMARY");

            entity.ToTable("annonces");

            entity.HasIndex(e => e.SportId, "fk_annonces_SportID");

            entity.HasIndex(e => e.Auteur, "fk_annonces_UtilisateurID");

            entity.HasIndex(e => e.ImageId, "fk_annonces_image_id");

            entity.Property(e => e.AnnoncesId)
                .ValueGeneratedNever()
                .HasColumnName("annonces_id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GenreAttendu).HasColumnType("enum('Femme','Homme','Mixte')");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.Lieu)
                .HasMaxLength(100)
                .HasColumnName("lieu");
            entity.Property(e => e.SportId).HasColumnName("SportID");
            entity.Property(e => e.Titre).HasMaxLength(255);
            entity.Property(e => e.Ville)
                .HasMaxLength(50)
                .HasColumnName("ville");

            entity.HasOne(d => d.AuteurNavigation).WithMany(p => p.Annonces)
                .HasForeignKey(d => d.Auteur)
                .HasConstraintName("fk_annonces_UtilisateurID");

            entity.HasOne(d => d.Image).WithMany(p => p.Annonces)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_annonces_image_id");

            entity.HasOne(d => d.Sport).WithMany(p => p.Annonces)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_annonces_SportID");
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("groupes");

            entity.HasIndex(e => e.AnnonceId, "fk_groupes_AnnonceID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AnnonceId).HasColumnName("AnnonceID");
            entity.Property(e => e.DateCreation).HasColumnType("datetime");
            entity.Property(e => e.DateSuppression).HasColumnType("datetime");

            entity.HasOne(d => d.Annonce).WithMany(p => p.Groupes)
                .HasForeignKey(d => d.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_groupes_AnnonceID");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImagesId).HasName("PRIMARY");

            entity.ToTable("images");

            entity.HasIndex(e => e.UtilisateurId, "fk_images_UtilisateurID");

            entity.Property(e => e.ImagesId)
                .ValueGeneratedNever()
                .HasColumnName("images_id");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Type).HasColumnType("enum('Profil','Publication','ActuSport')");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Images)
                .HasForeignKey(d => d.UtilisateurId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_images_UtilisateurID");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("messages");

            entity.HasIndex(e => e.GroupeId, "fk_messages_GroupeID");

            entity.HasIndex(e => e.UtilisateurId, "fk_messages_UtilisateurID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Contenu).HasColumnType("text");
            entity.Property(e => e.GroupeId).HasColumnName("GroupeID");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("participations");

            entity.HasIndex(e => e.AnnonceId, "fk_participations_AnnonceID");

            entity.HasIndex(e => e.GroupeId, "fk_participations_GroupeID");

            entity.HasIndex(e => e.UtilisateurId, "fk_participations_UtilisateurID");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.AnnonceId).HasColumnName("AnnonceID");
            entity.Property(e => e.DateParticipation).HasColumnType("datetime");
            entity.Property(e => e.GroupeId).HasColumnName("GroupeID");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.HasKey(e => e.PublicationsId).HasName("PRIMARY");

            entity.ToTable("publications");

            entity.HasIndex(e => e.UtilisateurId, "fk_publications_UtilisateurID");

            entity.HasIndex(e => e.ImageId, "fk_publications_image_id");

            entity.Property(e => e.PublicationsId)
                .ValueGeneratedNever()
                .HasColumnName("publications_id");
            entity.Property(e => e.Contenu).HasColumnType("text");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");

            entity.HasOne(d => d.Image).WithMany(p => p.Publications)
                .HasForeignKey(d => d.ImageId)
                .HasConstraintName("fk_publications_image_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Publications)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_publications_UtilisateurID");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.SportsId).HasName("PRIMARY");

            entity.ToTable("sports");

            entity.Property(e => e.SportsId)
                .ValueGeneratedNever()
                .HasColumnName("sports_id");
            entity.Property(e => e.Nom).HasMaxLength(255);
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("utilisateurs");

            entity.HasIndex(e => e.ImageId, "fk_utilisateurs_image_id");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Etat).HasColumnType("enum('Actif','Bloqué')");
            entity.Property(e => e.Genre).HasColumnType("enum('Femme','Homme','Mixte')");
            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.MotDePasse).HasMaxLength(100);
            entity.Property(e => e.Nom).HasMaxLength(255);
            entity.Property(e => e.Prenom).HasMaxLength(25);
            entity.Property(e => e.Pseudo).HasMaxLength(25);
            entity.Property(e => e.SportsFavoris).HasMaxLength(255);
            entity.Property(e => e.Ville).HasMaxLength(255);

            entity.HasOne(d => d.Image).WithMany(p => p.Utilisateurs)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_utilisateurs_image_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}