using Microsoft.EntityFrameworkCore;
using eventLib.Models;

namespace eventLib.Data
{
    public class EventManagerContext : DbContext
    {
        public EventManagerContext(DbContextOptions<EventManagerContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public DbSet<EventPerformer> EventPerformers { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<LogModel> Logs { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");
                entity.HasKey(e => e.IDUser);
                entity.Property(e => e.IDUser).HasColumnName("IDUser").ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PwdHash).IsRequired();
                entity.Property(e => e.PwdSalt).IsRequired();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
                
                // Relationship with UserRole
                entity.HasOne(u => u.UserRole)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.UserRoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ignore computed properties
                entity.Ignore(e => e.RoleName);
            });

            // Configure UserRole entity
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("userRole");
                entity.HasKey(e => e.IDUserRole);
                entity.Property(e => e.IDUserRole).HasColumnName("IDUserRole").ValueGeneratedOnAdd();
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
            });

            // Configure Event entity
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("event");
                entity.HasKey(e => e.IDEvent);
                entity.Property(e => e.IDEvent).HasColumnName("IDEvent").ValueGeneratedOnAdd();
                entity.Property(e => e.EventName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.eventTypeID).IsRequired();
                
                // Relationship with EventType
                entity.HasOne(e => e.EventType)
                    .WithMany()
                    .HasForeignKey(e => e.eventTypeID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ignore computed properties
                entity.Ignore(e => e.EventTypeName);
                entity.Ignore(e => e.ImageName);
                entity.Ignore(e => e.ImageData);
            });

            // Configure EventType entity
            modelBuilder.Entity<EventType>(entity =>
            {
                entity.ToTable("eventType");
                entity.HasKey(e => e.IDEventType);
                entity.Property(e => e.IDEventType).HasColumnName("IDEventType").ValueGeneratedOnAdd();
                entity.Property(e => e.EventTypeName).IsRequired().HasMaxLength(50);
            });

            // Configure Performer entity
            modelBuilder.Entity<Performer>(entity =>
            {
                entity.ToTable("performer");
                entity.HasKey(e => e.IDPerformer);
                entity.Property(e => e.IDPerformer).HasColumnName("IDPerformer").ValueGeneratedOnAdd();
                entity.Property(e => e.PerformerName).IsRequired().HasMaxLength(100);
            });

            // Configure EventPerformer entity (many-to-many relationship)
            modelBuilder.Entity<EventPerformer>(entity =>
            {
                entity.ToTable("eventPerformer");
                entity.HasKey(e => e.IDEventPerformer);
                entity.Property(e => e.IDEventPerformer).HasColumnName("IDEventPerformer").ValueGeneratedOnAdd();
                entity.Property(e => e.EventID).IsRequired();
                entity.Property(e => e.PerformerID).IsRequired();
                
                // Relationships
                entity.HasOne(ep => ep.Event)
                    .WithMany(e => e.EventPerformers)
                    .HasForeignKey(ep => ep.EventID)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ep => ep.Performer)
                    .WithMany(p => p.EventPerformers)
                    .HasForeignKey(ep => ep.PerformerID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Ignore computed properties
                entity.Ignore(ep => ep.PerformerName);
            });

            // Configure EventRegistration entity
            modelBuilder.Entity<EventRegistration>(entity =>
            {
                entity.ToTable("eventRegistration");
                entity.HasKey(e => e.IDEventRegistration);
                entity.Property(e => e.IDEventRegistration).HasColumnName("IDEventRegistration").ValueGeneratedOnAdd();
                entity.Property(e => e.EventID).IsRequired();
                entity.Property(e => e.UserID).IsRequired();
                
                // Relationships
                entity.HasOne(er => er.Event)
                    .WithMany(e => e.EventRegistrations)
                    .HasForeignKey(er => er.EventID)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(er => er.User)
                    .WithMany(u => u.EventRegistrations)
                    .HasForeignKey(er => er.UserID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Ignore computed properties
                entity.Ignore(er => er.EventName);
                entity.Ignore(er => er.Description);
                entity.Ignore(er => er.EventTypeName);
                entity.Ignore(er => er.Date);
                entity.Ignore(er => er.ImageName);
                entity.Ignore(er => er.ImageData);
            });

            // Configure LogModel entity
            modelBuilder.Entity<LogModel>(entity =>
            {
                entity.ToTable("log");
                entity.HasKey(e => e.IDLog);
                entity.Property(e => e.IDLog).HasColumnName("IDLog").ValueGeneratedOnAdd();
                entity.Property(e => e.IDLevel).IsRequired();
                entity.Property(e => e.InfoMessage).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();

                // Ignore computed properties
                entity.Ignore(l => l.LevelName);
            });

            // Configure Image entity
            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");
                entity.HasKey(e => e.IDImage);
                entity.Property(e => e.IDImage).HasColumnName("IDImage").ValueGeneratedOnAdd();
                entity.Property(e => e.ImageName).HasMaxLength(255);
                entity.Property(e => e.ImageData).HasColumnType("varbinary(max)");
            });
        }
    }
} 