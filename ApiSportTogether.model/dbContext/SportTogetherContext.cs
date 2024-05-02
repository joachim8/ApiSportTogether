using ApiSportTogether.model.ObjectContext;

using Microsoft.EntityFrameworkCore;

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
    public virtual DbSet<AnnonceImage> AnnonceImages { get; set; }
    public virtual DbSet<ProfileImage> ProfileImages { get; set; }

    public virtual DbSet<PublicationImage> PublicationImages { get; set; }

    public virtual DbSet<Annonce> Annonces { get; set; }

    public virtual DbSet<Groupe> Groupes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportFavori> SportFavoris { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }



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

            entity.Property(e => e.AmisId).HasColumnName("amis_id");
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
        modelBuilder.Entity<AnnonceImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("annonce_images");

            entity.HasIndex(e => e.Url, "URL").IsUnique();

            entity.HasIndex(e => e.AnnoncesId, "fk_annonce_images_Annonce");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.AnnoncesId).HasColumnName("annonces_id");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Url).HasColumnName("URL");

            entity.HasOne(d => d.Annonces).WithMany(p => p.AnnonceImages)
                .HasForeignKey(d => d.AnnoncesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_annonce_images_Annonce");
        });
        modelBuilder.Entity<Annonce>(entity =>
        {
            entity.HasKey(e => e.AnnoncesId).HasName("PRIMARY");

            entity.ToTable("annonces");

            entity.HasIndex(e => e.SportId, "fk_annonces_SportID");

            entity.HasIndex(e => e.Auteur, "fk_annonces_UtilisateurID");


            entity.Property(e => e.AnnoncesId).HasColumnName("annonces_id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GenreAttendu).HasColumnType("enum('Femme','Homme','Mixte')");
            entity.Property(e => e.Lieu)
                .HasMaxLength(100)
                .HasColumnName("lieu");
            entity.Property(e => e.DateHeureAnnonce).HasColumnType("datetime");
            entity.Property(e => e.SportId).HasColumnName("SportID");
            entity.Property(e => e.Titre).HasMaxLength(255);
            entity.Property(e => e.Ville)
                .HasMaxLength(50)
                .HasColumnName("ville");

            entity.HasOne(d => d.AuteurNavigation).WithMany(p => p.Annonces)
                .HasForeignKey(d => d.Auteur)
                .HasConstraintName("fk_annonces_UtilisateurID");


            entity.HasOne(d => d.Sport).WithMany(p => p.Annonces)
                .HasForeignKey(d => d.SportId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_annonces_SportID");
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.GroupesId).HasName("PRIMARY");

            entity.ToTable("groupes");

            entity.HasIndex(e => e.AnnonceId, "fk_groupes_AnnonceID");

            entity.Property(e => e.GroupesId).HasColumnName("groupes_ID");
            entity.Property(e => e.AnnonceId).HasColumnName("AnnonceID");
            entity.Property(e => e.DateCreation).HasColumnType("datetime");
            entity.Property(e => e.DateSuppression).HasColumnType("datetime");

            entity.HasOne(d => d.Annonce).WithMany(p => p.Groupes)
                .HasForeignKey(d => d.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_groupes_AnnonceID");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessagesId).HasName("PRIMARY");

            entity.ToTable("messages");

            entity.HasIndex(e => e.GroupeId, "fk_messages_GroupeID");

            entity.HasIndex(e => e.UtilisateurId, "fk_messages_UtilisateurID");

            entity.Property(e => e.MessagesId).HasColumnName("messages_id");
            entity.Property(e => e.Contenu).HasColumnType("text");
            entity.Property(e => e.GroupeId).HasColumnName("GroupeID");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(e => e.ParticipationsId).HasName("PRIMARY");

            entity.ToTable("participations");

            entity.HasIndex(e => e.AnnonceId, "fk_participations_AnnonceID");

            entity.HasIndex(e => e.GroupeId, "fk_participations_GroupeID");

            entity.HasIndex(e => e.UtilisateurId, "fk_participations_UtilisateurID");

            entity.Property(e => e.ParticipationsId).HasColumnName("participations_id");
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

        

            entity.Property(e => e.PublicationsId).HasColumnName("publications_id");
            entity.Property(e => e.Contenu).HasColumnType("text");

            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");

         

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Publications)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_publications_UtilisateurID");
        });
        modelBuilder.Entity<ProfileImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("profile_images");

            entity.HasIndex(e => e.Url, "URL").IsUnique();

            entity.HasIndex(e => e.UtilisateursId, "fk_profile_images_utilisateur");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Type).HasColumnType("enum('Profil','Photos')");
            entity.Property(e => e.Url).HasColumnName("URL");
            entity.Property(e => e.UtilisateursId).HasColumnName("utilisateurs_id");

            entity.HasOne(d => d.Utilisateurs).WithMany(p => p.ProfileImages)
                .HasForeignKey(d => d.UtilisateursId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_profile_images_utilisateur");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.SportsId).HasName("PRIMARY");

            entity.ToTable("sports");

            entity.Property(e => e.SportsId).HasColumnName("sports_id");
            entity.Property(e => e.Nom).HasMaxLength(255);
        });
        modelBuilder.Entity<PublicationImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("publication_images");

            entity.HasIndex(e => e.Url, "URL").IsUnique();

            entity.HasIndex(e => e.PublicationsId, "fk_publication_images_Publication");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.PublicationsId).HasColumnName("publications_id");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Url).HasColumnName("URL");

            entity.HasOne(d => d.Publications).WithMany(p => p.PublicationImages)
                .HasForeignKey(d => d.PublicationsId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_publication_images_Publication");
        });
        modelBuilder.Entity<SportFavori>(entity =>
        {
            entity.HasKey(e => e.SportFavoriId).HasName("PRIMARY");

            entity
                .ToTable("sport_favori")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.UtilisateursId, "fk_sport_favori_utilisateursID");

            entity.HasIndex(e => e.SportsId, "fk_sports_sportsFavoriID");

            entity.Property(e => e.SportFavoriId).HasColumnName("sport_favori_id");
            entity.Property(e => e.SportsId).HasColumnName("sports_id");
            entity.Property(e => e.UtilisateursId).HasColumnName("utilisateurs_id");

            entity.HasOne(d => d.Sports).WithMany(p => p.SportFavoris)
                .HasForeignKey(d => d.SportsId)
                .HasConstraintName("fk_sports_sportsFavoriID");

            entity.HasOne(d => d.Utilisateurs).WithMany(p => p.SportFavoris)
                .HasForeignKey(d => d.UtilisateursId)
                .HasConstraintName("fk_sport_favori_utilisateursID");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.UtilisateursId).HasName("PRIMARY");

            entity.ToTable("utilisateurs");

          

            entity.Property(e => e.UtilisateursId).HasColumnName("utilisateurs_id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Etat).HasColumnType("enum('Actif','Bloqué')");
            entity.Property(e => e.Genre).HasColumnType("enum('Femme','Homme','Mixte', 'Non-genre')");

            entity.Property(e => e.MotDePasse).HasMaxLength(100);
            entity.Property(e => e.Nom).HasMaxLength(255);
            entity.Property(e => e.Prenom).HasMaxLength(25);
            entity.Property(e => e.Pseudo).HasMaxLength(25);
            entity.Property(e => e.Ville).HasMaxLength(255);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
