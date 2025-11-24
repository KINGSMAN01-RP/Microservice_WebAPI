using AuthService_WebAPI.Models.RoleModel;
using AuthService_WebAPI.Models.TokenModel;
using AuthService_WebAPI.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace AuthService_WebAPI.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> Options) : base(Options)
        {

        }


        //Registering The Models From Which Tables Are to Be Created in The Database

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        //Applying Custom Configurations Using Fluent API For Both Tabe and Columns

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("tbl_User");

            modelBuilder.Entity<RefreshToken>().ToTable("tbl_RefreshToken");

            modelBuilder.Entity<Role>().ToTable("tbl_Role");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                .HasColumnName("UserId")
                .HasMaxLength(100);

                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.PasswordHash)
                      .IsRequired();

                entity.Property(e => e.PasswordSalt)
                      .IsRequired();

                entity.Property(e => e.Role)
                      .IsRequired()
                      .HasMaxLength(100);

            });


            modelBuilder.Entity<Role>(entity => 
            {
                entity.HasKey(e => e.ID);

                entity.Property(e => e.ID)
                      .HasColumnName("RoleId");

                entity.Property(e => e.RoleName)
                      .HasColumnName("RoleName")
                      .IsRequired(true)
                      .HasMaxLength(100);

                entity.Property(e => e.RoleDescription)
                      .HasColumnName("RoleDetails")
                      .HasMaxLength(250);

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("CreatedDate")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.CreatedBy)
                        .HasColumnName("CreatedBy")
                        .IsRequired(false)
                        .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt)
                        .HasColumnName("UpdatedDate")
                        .HasMaxLength(100);

                entity.Property(e => e.UpdatedBy)
                        .HasColumnName("UpdatedBy")
                        .IsRequired(false)
                        .HasMaxLength(100);

            });




        }

    }
}
