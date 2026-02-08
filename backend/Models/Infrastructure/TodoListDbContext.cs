namespace TodoListAPI.Models.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using TodoListAPI.Models.Domain;

    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
            : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User
            builder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                 .HasDefaultValueSql("NEWSEQUENTIALID()");

                e.HasIndex(x => x.Email).IsUnique();

                e.Property(x => x.Name).HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(150);
                e.Property(x => x.Role).HasConversion<int>();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // RefreshToken
            builder.Entity<RefreshToken>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Token);
                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PasswordResetToken
            builder.Entity<PasswordResetToken>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Token);
                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Sections
            builder.Entity<Section>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                 .HasDefaultValueSql("NEWSEQUENTIALID()");

                e.HasOne(x => x.User)
                 .WithMany(x => x.Sections)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Task
            builder.Entity<TaskItem>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Id)
                 .HasDefaultValueSql("NEWSEQUENTIALID()");

                e.Property(x => x.Status)
                 .HasConversion<int>();

                e.HasOne(x => x.Section)
                 .WithMany(x => x.Tasks)
                 .HasForeignKey(x => x.SectionId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });
        }
    }

}
