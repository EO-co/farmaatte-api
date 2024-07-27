using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace farmaatte_api.Models;

public partial class FarmaatteDbContext : DbContext
{
    public FarmaatteDbContext()
    {
    }

    public FarmaatteDbContext(DbContextOptions<FarmaatteDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Grouppicture> Grouppictures { get; set; }

    public virtual DbSet<Profilepicture> Profilepictures { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<User> Users { get; set; }

    // Connection string here! Now we just need to implement .env files!
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=http://postgres:5432;Database=farmaatte_db;username=myuser;Password=mypassword");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Gameid).HasName("games_pkey");

            entity.ToTable("games");

            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Groupid).HasName("groups_pkey");

            entity.ToTable("groups");

            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Grouppicture>(entity =>
        {
            entity.HasKey(e => e.Grouppictureid).HasName("grouppictures_pkey");

            entity.ToTable("grouppictures");

            entity.Property(e => e.Grouppictureid).HasColumnName("grouppictureid");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Image).HasColumnName("image");

            entity.HasOne(d => d.Group).WithMany(p => p.Grouppictures)
                .HasForeignKey(d => d.Groupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("grouppictures_groupid_fkey");
        });

        modelBuilder.Entity<Profilepicture>(entity =>
        {
            entity.HasKey(e => e.Profilepictureid).HasName("profilepictures_pkey");

            entity.ToTable("profilepictures");

            entity.Property(e => e.Profilepictureid).HasColumnName("profilepictureid");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Profilepictures)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profilepictures_userid_fkey");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Resultid).HasName("results_pkey");

            entity.ToTable("results");

            entity.Property(e => e.Resultid).HasColumnName("resultid");
            entity.Property(e => e.Gameid).HasColumnName("gameid");
            entity.Property(e => e.Player1Id).HasColumnName("player1_id");
            entity.Property(e => e.Player2Id).HasColumnName("player2_id");
            entity.Property(e => e.Winner).HasColumnName("winner");

            entity.HasOne(d => d.Game).WithMany(p => p.Results)
                .HasForeignKey(d => d.Gameid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("results_gameid_fkey");

            entity.HasOne(d => d.Player1).WithMany(p => p.ResultPlayer1s)
                .HasForeignKey(d => d.Player1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("results_player1_id_fkey");

            entity.HasOne(d => d.Player2).WithMany(p => p.ResultPlayer2s)
                .HasForeignKey(d => d.Player2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("results_player2_id_fkey");

            entity.HasOne(d => d.WinnerNavigation).WithMany(p => p.ResultWinnerNavigations)
                .HasForeignKey(d => d.Winner)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("results_winner_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Nickname).HasColumnName("nickname");
            entity.Property(e => e.Pwhash)
                .HasMaxLength(64)
                .IsFixedLength()
                .HasColumnName("pwhash");
            entity.Property(e => e.Username).HasColumnName("username");

            entity.HasOne(d => d.Group).WithMany(p => p.Users)
                .HasForeignKey(d => d.Groupid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_groupid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
