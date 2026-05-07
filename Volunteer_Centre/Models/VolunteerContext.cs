using Microsoft.EntityFrameworkCore;

namespace VolunteerApp.Models
{
    public class VolunteerContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<RegistrationStatus> RegistrationStatuses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VolunteerEvent> Events { get; set; }
        public DbSet<VolunteerRegistration> Registrations { get; set; }
        public DbSet<VolunteerSkill> VolunteerSkills { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            opt.UseNpgsql("Host=localhost;Port=5432;Database=volunteer_center;Username=postgres;Password=postgres");
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Role>(e =>
            {
                e.ToTable("roles");
                e.Property(r => r.Id).HasColumnName("id");
                e.Property(r => r.Name).HasColumnName("name");
            });

            mb.Entity<EventCategory>(e =>
            {
                e.ToTable("event_categories");
                e.Property(c => c.Id).HasColumnName("id");
                e.Property(c => c.Name).HasColumnName("name");
            });

            mb.Entity<EventStatus>(e =>
            {
                e.ToTable("event_statuses");
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.Name).HasColumnName("name");
            });

            mb.Entity<RegistrationStatus>(e =>
            {
                e.ToTable("registration_statuses");
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.Name).HasColumnName("name");
            });

            mb.Entity<Location>(e =>
            {
                e.ToTable("locations");
                e.Property(l => l.Id).HasColumnName("id");
                e.Property(l => l.Name).HasColumnName("name");
                e.Property(l => l.Address).HasColumnName("address");
            });

            mb.Entity<Skill>(e =>
            {
                e.ToTable("skills");
                e.Property(s => s.Id).HasColumnName("id");
                e.Property(s => s.Name).HasColumnName("name");
            });

            mb.Entity<User>(e =>
            {
                e.ToTable("users");
                e.Property(u => u.Id).HasColumnName("id");
                e.Property(u => u.FullName).HasColumnName("full_name");
                e.Property(u => u.RoleId).HasColumnName("role_id");
                e.Property(u => u.Email).HasColumnName("email");
                e.Property(u => u.Login).HasColumnName("login");
                e.Property(u => u.PasswordText).HasColumnName("password_text");
                e.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            });

            mb.Entity<VolunteerEvent>(e =>
            {
                e.ToTable("events");
                e.Property(v => v.Id).HasColumnName("id");
                e.Property(v => v.Title).HasColumnName("title");
                e.Property(v => v.CategoryId).HasColumnName("category_id");
                e.Property(v => v.EventDate).HasColumnName("event_date");
                e.Property(v => v.LocationId).HasColumnName("location_id");
                e.Property(v => v.VolunteersNeeded).HasColumnName("volunteers_needed");
                e.Property(v => v.CoordinatorId).HasColumnName("coordinator_id");
                e.Property(v => v.StatusId).HasColumnName("status_id");
                e.HasOne(v => v.Category).WithMany(c => c.Events).HasForeignKey(v => v.CategoryId);
                e.HasOne(v => v.Location).WithMany(l => l.Events).HasForeignKey(v => v.LocationId);
                e.HasOne(v => v.Coordinator).WithMany(u => u.CoordinatedEvents).HasForeignKey(v => v.CoordinatorId);
                e.HasOne(v => v.Status).WithMany(s => s.Events).HasForeignKey(v => v.StatusId);
            });

            mb.Entity<VolunteerRegistration>(e =>
            {
                e.ToTable("volunteer_registrations");
                e.Property(r => r.Id).HasColumnName("id");
                e.Property(r => r.EventId).HasColumnName("event_id");
                e.Property(r => r.VolunteerId).HasColumnName("volunteer_id");
                e.Property(r => r.RegistrationDate).HasColumnName("registration_date");
                e.Property(r => r.StatusId).HasColumnName("status_id");
                e.HasOne(r => r.Event).WithMany(ev => ev.Registrations).HasForeignKey(r => r.EventId);
                e.HasOne(r => r.Volunteer).WithMany(u => u.Registrations).HasForeignKey(r => r.VolunteerId);
                e.HasOne(r => r.Status).WithMany(s => s.Registrations).HasForeignKey(r => r.StatusId);
            });

            mb.Entity<VolunteerSkill>(e =>
            {
                e.ToTable("volunteer_skills");
                e.Property(vs => vs.Id).HasColumnName("id");
                e.Property(vs => vs.VolunteerId).HasColumnName("volunteer_id");
                e.Property(vs => vs.SkillId).HasColumnName("skill_id");
                e.HasOne(vs => vs.Volunteer).WithMany(u => u.VolunteerSkills).HasForeignKey(vs => vs.VolunteerId);
                e.HasOne(vs => vs.Skill).WithMany(s => s.VolunteerSkills).HasForeignKey(vs => vs.SkillId);
            });

            mb.Entity<Attendance>(e =>
            {
                e.ToTable("attendance");
                e.Property(a => a.Id).HasColumnName("id");
                e.Property(a => a.RegistrationId).HasColumnName("registration_id");
                e.Property(a => a.Attended).HasColumnName("attended");
                e.Property(a => a.Notes).HasColumnName("notes");
                e.HasOne(a => a.Registration).WithOne(r => r.Attendance).HasForeignKey<Attendance>(a => a.RegistrationId);
            });
        }
    }
}
