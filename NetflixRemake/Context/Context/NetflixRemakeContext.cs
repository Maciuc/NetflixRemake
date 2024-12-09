using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public partial class NetflixRemakeContext : DbContext
    {
        public NetflixRemakeContext()
        {
        }

        public NetflixRemakeContext(DbContextOptions<NetflixRemakeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MoviesOption> MoviesOptions { get; set; } = null!;
        public virtual DbSet<Option> Options { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<UserMovie> UserMovies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.VideoPath).HasMaxLength(500);
            });

            modelBuilder.Entity<MoviesOption>(entity =>
            {
                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MoviesOptions)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MoviesOpt__Movie__46E78A0C");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.MoviesOptions)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MoviesOpt__Optio__47DBAE45");
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(200);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.Property(e => e.Comment).HasMaxLength(500);

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ratings__MovieId__3F466844");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ratings__UserId__403A8C7D");
            });

            modelBuilder.Entity<UserMovie>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.UserMovies)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserMovie__Movie__3A81B327");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserMovies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserMovie__UserI__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
