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

    public virtual DbSet<ProfileImage> ProfileImages { get; set; }

    public virtual DbSet<PublicationImage> PublicationImages { get; set; }
    public virtual DbSet<NoteAnnonce> NoteAnnonces { get; set; }

    public virtual DbSet<Annonce> Annonces { get; set; }
    public virtual DbSet<EncouragementPublication> EncouragementPublications { get; set; }

    public virtual DbSet<EncouragementPublicationCommentaire> EncouragementPublicationCommentaires { get; set; }
    public virtual DbSet<Groupe> Groupes { get; set; }
    public virtual DbSet<MembreGroupe> MembreGroupes { get; set; }
    public virtual DbSet<Message> Messages { get; set; }
    public virtual DbSet<NotificationUtilisateur> NotificationUtilisateurs { get; set; }

    public virtual DbSet<Participation> Participations { get; set; }

    public virtual DbSet<Publication> Publications { get; set; }
    public virtual DbSet<PublicationCommentaire> PublicationCommentaires { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportFavori> SportFavoris { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    public virtual DbSet<VuMessage> VuMessages { get; set; }

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

        modelBuilder.Entity<Annonce>(entity =>
        {
            entity.HasKey(e => e.AnnoncesId).HasName("PRIMARY");

            entity.ToTable("annonces");

            entity.HasIndex(e => e.SportId, "fk_annonces_SportID");

            entity.HasIndex(e => e.Auteur, "fk_annonces_UtilisateurID");

            entity.Property(e => e.AnnoncesId).HasColumnName("annonces_id");
            entity.Property(e => e.DateHeureAnnonce)
                .HasColumnType("datetime")
                .HasColumnName("date_heure_annonce");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GenreAttendu).HasColumnType("enum('Femme','Homme','Mixte')");
            entity.Property(e => e.Lieu)
                .HasMaxLength(100)
                .HasColumnName("lieu");
            entity.Property(e => e.Niveau).HasColumnType("enum('Débutant','Intermédiaire','Avancé')");
            entity.Property(e => e.NoteAnnonce)
                .HasPrecision(10)
                .HasColumnName("Note_annonce");
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

        modelBuilder.Entity<EncouragementPublication>(entity =>
        {
            entity.HasKey(e => e.EncouragementPublicationId).HasName("PRIMARY");

            entity.ToTable("encouragement_publication");

            entity.HasIndex(e => e.PublicationId, "fk_encouragement_publication_publication_id");

            entity.HasIndex(e => e.UtilisateurId, "fk_encouragement_publication_utilisateur_id");

            entity.Property(e => e.EncouragementPublicationId).HasColumnName("encouragement_publication_id");
            entity.Property(e => e.DateEncouragementPublication)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_encouragement_publication");
            entity.Property(e => e.PublicationId).HasColumnName("publication_id");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");

            entity.HasOne(d => d.Publication).WithMany(p => p.EncouragementPublications)
                .HasForeignKey(d => d.PublicationId)
                .HasConstraintName("fk_encouragement_publication_publication_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.EncouragementPublications)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_encouragement_publication_utilisateur_id");
        });

        modelBuilder.Entity<EncouragementPublicationCommentaire>(entity =>
        {
            entity.HasKey(e => e.EncouragementPublicationCommentaireId).HasName("PRIMARY");

            entity.ToTable("encouragement_publication_commentaire");

            entity.HasIndex(e => e.PublicationCommentaireId, "fk_encouragement_publication_commentaire_commentaire_id");

            entity.HasIndex(e => e.UtilisateurId, "fk_encouragement_publication_commentaire_utilisateur_id");

            entity.Property(e => e.EncouragementPublicationCommentaireId).HasColumnName("encouragement_publication_commentaire_id");
            entity.Property(e => e.DateEncouragementPublicationCommentaire)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("date_encouragement_publication_commentaire");
            entity.Property(e => e.PublicationCommentaireId).HasColumnName("publication_commentaire_id");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");

            entity.HasOne(d => d.PublicationCommentaire).WithMany(p => p.EncouragementPublicationCommentaires)
                .HasForeignKey(d => d.PublicationCommentaireId)
                .HasConstraintName("fk_encouragement_publication_commentaire_commentaire_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.EncouragementPublicationCommentaires)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_encouragement_publication_commentaire_utilisateur_id");
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.GroupesId).HasName("PRIMARY");

            entity.ToTable("groupes");

            entity.HasIndex(e => e.ChefDuGroupe, "fk_chef_du_groupe");

            entity.HasIndex(e => e.AnnonceId, "fk_groupes_AnnonceID");

            entity.Property(e => e.GroupesId).HasColumnName("groupes_ID");
            entity.Property(e => e.AnnonceId).HasColumnName("AnnonceID");
            entity.Property(e => e.DateCreation).HasColumnType("datetime");
            entity.Property(e => e.DateSuppression).HasColumnType("datetime");
            entity.Property(e => e.LastMessage).HasColumnName("lastMessage");
            entity.Property(e => e.Nom).HasMaxLength(100);

            entity.HasOne(d => d.Annonce).WithMany(p => p.Groupes)
                .HasForeignKey(d => d.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_groupes_AnnonceID");

            entity.HasOne(d => d.ChefDuGroupeNavigation).WithMany(p => p.Groupes)
                .HasForeignKey(d => d.ChefDuGroupe)
                .HasConstraintName("fk_chef_du_groupe");
        });

        modelBuilder.Entity<MembreGroupe>(entity =>
        {
            entity.HasKey(e => e.IdMembreGroupe).HasName("PRIMARY");

            entity.ToTable("membre_groupe");

            entity.HasIndex(e => e.GroupeId, "fk_groupe_membre_groupe");

            entity.HasIndex(e => e.UtilisateurId, "fk_utilisateur_membre_groupe");

            entity.Property(e => e.IdMembreGroupe).HasColumnName("id_membre_groupe");
            entity.Property(e => e.GroupeId).HasColumnName("groupe_id");
            entity.Property(e => e.Role)
                .HasColumnType("enum('Admin','Membre')")
                .HasColumnName("role");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");

            entity.HasOne(d => d.Groupe).WithMany(p => p.MembreGroupes)
                .HasForeignKey(d => d.GroupeId)
                .HasConstraintName("fk_groupe_membre_groupe");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.MembreGroupes)
                .HasForeignKey(d => d.UtilisateurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_utilisateur_membre_groupe");
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
            entity.Property(e => e.NomUtilisateur)
                .HasMaxLength(100)
                .HasColumnName("nom_utilisateur");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.urlProfilImage)
                .HasColumnType("text")
                .HasColumnName("url_profil_image");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");
        });

        modelBuilder.Entity<NoteAnnonce>(entity =>
        {
            entity.HasKey(e => e.NoteAnnonceId).HasName("PRIMARY");

            entity.ToTable("note_annonce");

            entity.HasIndex(e => e.AnnonceId, "fk_note_annonce_annonce_id");

            entity.HasIndex(e => e.UtilisateurId, "fk_note_annonce_utilisateur_id");

            entity.Property(e => e.NoteAnnonceId).HasColumnName("note_annonce_id");
            entity.Property(e => e.AnnonceId).HasColumnName("annonce_id");
            entity.Property(e => e.Commentaire)
                .HasColumnType("text")
                .HasColumnName("commentaire");
            entity.Property(e => e.Note)
                .HasPrecision(10)
                .HasColumnName("note");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");
            entity.Property(e => e.IsPublic).HasColumnName("is_public");

            entity.HasOne(d => d.Annonce).WithMany(p => p.NoteAnnonces)
                .HasForeignKey(d => d.AnnonceId)
                .HasConstraintName("fk_note_annonce_annonce_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.NoteAnnonces)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_note_annonce_utilisateur_id");
        });

        modelBuilder.Entity<NotificationUtilisateur>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PRIMARY");

            entity.ToTable("notification_utilisateur");

            entity.HasIndex(e => e.UtilisateurId, "fk_notification_utilisateur_utilisateur_id");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.Contenu)
                .HasColumnType("text")
                .HasColumnName("contenu");
            entity.Property(e => e.DateNotification)
                .HasColumnType("datetime")
                .HasColumnName("date_notification");
            entity.Property(e => e.TypeNotification)
                .HasColumnType("enum('Participation','Ajout en ami','Commentaire','Encouragement_commentaire','Encouragement_publication','Note')")
                .HasColumnName("type_notification");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");  
            entity.Property(e => e.UtilisateurEnvoiId).HasColumnName("utilisateur_envoi_id");
            entity.Property(e => e.Vu).HasColumnName("vu");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.NotificationUtilisateurs)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_notification_utilisateur_utilisateur_id");
        });

        modelBuilder.Entity<Participation>(entity =>
        {
            entity.HasKey(e => e.ParticipationsId).HasName("PRIMARY");

            entity.ToTable("participations");

            entity.HasIndex(e => e.AnnonceId, "fk_participations_annonce_id");

            entity.HasIndex(e => e.GroupeId, "fk_participations_groupe_id");

            entity.HasIndex(e => e.UtilisateurId, "fk_participations_utilisateur_id");

            entity.Property(e => e.ParticipationsId).HasColumnName("participations_id");
            entity.Property(e => e.AnnonceId).HasColumnName("AnnonceID");
            entity.Property(e => e.DateParticipation).HasColumnType("datetime");
            entity.Property(e => e.GroupeId).HasColumnName("GroupeID");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");

            entity.HasOne(d => d.Annonce).WithMany(p => p.Participations)
                .HasForeignKey(d => d.AnnonceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_participations_annonce_id");

            entity.HasOne(d => d.Groupe).WithMany(p => p.Participations)
                .HasForeignKey(d => d.GroupeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_participations_groupe_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Participations)
                .HasForeignKey(d => d.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_participations_utilisateur_id");
        });

        modelBuilder.Entity<ProfileImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("profile_images");

            entity.HasIndex(e => e.UtilisateursId, "fk_profile_images_utilisateur");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.Type).HasColumnType("enum('Profil','Photos')");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("URL");
            entity.Property(e => e.UtilisateursId).HasColumnName("utilisateurs_id");

            entity.HasOne(d => d.Utilisateurs).WithMany(p => p.ProfileImages)
                .HasForeignKey(d => d.UtilisateursId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_profile_images_utilisateur");
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.HasKey(e => e.PublicationsId).HasName("PRIMARY");

            entity.ToTable("publications");

            entity.HasIndex(e => e.UtilisateurId, "fk_publications_UtilisateurID");

            entity.Property(e => e.PublicationsId).HasColumnName("publications_id");
            entity.Property(e => e.Contenu).HasColumnType("text");
            entity.Property(e => e.DatePublication)
                .HasColumnType("datetime")
                .HasColumnName("Date_publication");
            entity.Property(e => e.NombreEncouragement).HasColumnName("nombre_encouragement");
            entity.Property(e => e.SportTag)
                .HasMaxLength(100)
                .HasColumnName("sport_tag");
            entity.Property(e => e.UtilisateurId).HasColumnName("UtilisateurID");
            entity.Property(e => e.Visibilite).HasColumnName("visibilite");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.Publications)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_publications_UtilisateurID");
        });

        modelBuilder.Entity<PublicationCommentaire>(entity =>
        {
            entity.HasKey(e => e.CommentaireId).HasName("PRIMARY");

            entity.ToTable("publication_commentaire");

            entity.HasIndex(e => e.PublicationId, "fk_publication_commentaire_publication_id");

            entity.HasIndex(e => e.UtilisateurId, "fk_publication_commentaire_utilisateur_id");

            entity.Property(e => e.CommentaireId).HasColumnName("commentaire_id");
            entity.Property(e => e.Contenu)
                .HasColumnType("text")
                .HasColumnName("contenu");
            entity.Property(e => e.DateCommentaire)
                .HasColumnType("datetime")
                .HasColumnName("date_commentaire");
            entity.Property(e => e.NombreEncouragementCommentaire).HasColumnName("nombre_encouragement_commentaire");
            entity.Property(e => e.PublicationId).HasColumnName("publication_id");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");

            entity.HasOne(d => d.Publication).WithMany(p => p.PublicationCommentaires)
                .HasForeignKey(d => d.PublicationId)
                .HasConstraintName("fk_publication_commentaire_publication_id");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.PublicationCommentaires)
                .HasForeignKey(d => d.UtilisateurId)
                .HasConstraintName("fk_publication_commentaire_utilisateur_id");
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
            entity.Property(e => e.Type).HasColumnType("enum('Image','Video')");
            entity.Property(e => e.Url).HasColumnName("URL");

            entity.HasOne(d => d.Publications).WithMany(p => p.PublicationImages)
                .HasForeignKey(d => d.PublicationsId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_publication_images_Publication");
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.SportsId).HasName("PRIMARY");

            entity.ToTable("sports");

            entity.Property(e => e.SportsId).HasColumnName("sports_id");
            entity.Property(e => e.Nom).HasMaxLength(255);
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

            entity.HasIndex(e => e.Pseudo, "Pseudo").IsUnique();

            entity.Property(e => e.UtilisateursId).HasColumnName("utilisateurs_id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.DescriptionSport)
                .HasColumnType("text")
                .HasColumnName("Description_sport");
            entity.Property(e => e.Disponibilites).HasColumnType("enum('Semaine-matin','Semaine-après-midi','Semaine-soir','Tout le temps','Weekends')");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Etat).HasColumnType("enum('Actif','Bloqué')");
            entity.Property(e => e.FunFact)
                .HasColumnType("text")
                .HasColumnName("Fun_fact");
            entity.Property(e => e.Genre).HasColumnType("enum('Femme','Homme')");
            entity.Property(e => e.MotDePasse).HasMaxLength(100);
            entity.Property(e => e.NiveauSport)
                .HasColumnType("enum('Débutant','Intermédiaire','Avancé')")
                .HasColumnName("Niveau_sport");
            entity.Property(e => e.Nom).HasMaxLength(255);
            entity.Property(e => e.Prenom).HasMaxLength(25);
            entity.Property(e => e.Pseudo).HasMaxLength(25);
            entity.Property(e => e.TypePartenaire)
                .HasColumnType("enum('Groupe d’amis','Compétiteurs','Personnes du même niveau','Coach')")
                .HasColumnName("Type_partenaire");
            entity.Property(e => e.Ville).HasMaxLength(255);
        });

        modelBuilder.Entity<VuMessage>(entity =>
        {
            entity.HasKey(e => e.IdMessageVu).HasName("PRIMARY");

            entity.ToTable("vu_message");

            entity.HasIndex(e => e.messages_id, "fk_messages_vuMessage");

            entity.HasIndex(e => e.UtilisateurId, "fk_utilisateurs_vu_message");

            entity.Property(e => e.IdMessageVu).HasColumnName("id_message_vu");
            entity.Property(e => e.messages_id).HasColumnName("messages_id");
            entity.Property(e => e.UtilisateurId).HasColumnName("utilisateur_id");
            entity.Property(e => e.Vu).HasColumnName("vu");

            entity.HasOne(d => d.IdMessageNavigation).WithMany(p => p.VuMessages)
                .HasForeignKey(d => d.messages_id)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_messages_vuMessage");

            entity.HasOne(d => d.Utilisateur).WithMany(p => p.VuMessages)
                .HasForeignKey(d => d.UtilisateurId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_utilisateurs_vu_message");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
