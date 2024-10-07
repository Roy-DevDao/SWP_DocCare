using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test2.Data;

public partial class BookingCareContext : DbContext
{
    public BookingCareContext()
    {
    }

    public BookingCareContext(DbContextOptions<BookingCareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<DetailDoctor> DetailDoctors { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:EnterTheWorldDB"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC07E6BC3C97");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<DetailDoctor>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailDo__135C316D28183276");

            entity.ToTable("DetailDoctor");

            entity.Property(e => e.DetailId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.DetailDoctors)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__DetailDocto__DId__30F848ED");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK__Doctor__C036565004AD6463");

            entity.ToTable("Doctor");

            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.DoctorImg).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.Did)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctor__DId__286302EC");

            entity.HasMany(d => d.Specialties).WithMany(p => p.Dids)
                .UsingEntity<Dictionary<string, object>>(
                    "DoctorSpecialty",
                    r => r.HasOne<Specialty>().WithMany()
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DoctorSpe__Speci__2E1BDC42"),
                    l => l.HasOne<Doctor>().WithMany()
                        .HasForeignKey("Did")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DoctorSpeci__DId__2D27B809"),
                    j =>
                    {
                        j.HasKey("Did", "SpecialtyId").HasName("PK__DoctorSp__5D40D93A386709D0");
                        j.ToTable("DoctorSpecialty");
                        j.IndexerProperty<string>("Did")
                            .HasMaxLength(255)
                            .HasColumnName("DId");
                        j.IndexerProperty<string>("SpecialtyId").HasMaxLength(255);
                    });
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD6E466F152");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasMaxLength(255);
            entity.Property(e => e.DateCmt).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Feedback__DId__33D4B598");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Feedback__PId__34C8D9D1");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78E910743711");

            entity.ToTable("HealthRecord");

            entity.Property(e => e.RecordId).HasMaxLength(255);
            entity.Property(e => e.DateExam).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__HealthRecor__DId__38996AB5");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__HealthRecor__PId__37A5467C");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__History__4D7B4ABDC9508559");

            entity.ToTable("History");

            entity.Property(e => e.HistoryId).HasMaxLength(255);
            entity.Property(e => e.DateExam).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.RecordId).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Histories)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__History__DId__3C69FB99");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Histories)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__History__PId__3B75D760");

            entity.HasOne(d => d.Record).WithMany(p => p.Histories)
                .HasForeignKey(d => d.RecordId)
                .HasConstraintName("FK__History__RecordI__3D5E1FD2");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__92C7A1FF3BBE1817");

            entity.ToTable("Option");

            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.DateExam).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Option__DId__44FF419A");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__Order__CB394B19396AE250");

            entity.ToTable("Order");

            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");
            entity.Property(e => e.DateOrder).HasColumnType("datetime");
            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.ScheduleId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Option).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OptionId)
                .HasConstraintName("FK__Order__OptionId__48CFD27E");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Order__PId__47DBAE45");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Order__ScheduleI__49C3F6B7");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__Patient__C57755403DA54D87");

            entity.ToTable("Patient");

            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.PidNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PId__25869641");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__EE8FCECF5890D7F1");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasMaxLength(255);
            entity.Property(e => e.DatePay).HasColumnType("datetime");
            entity.Property(e => e.Method).HasMaxLength(255);
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__Payment__OId__4CA06362");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B4931426657");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Schedule__DId__403A8C7D");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service__C51BB00A35AD4041");

            entity.ToTable("Service");

            entity.Property(e => e.ServiceId).HasMaxLength(255);
            entity.Property(e => e.ServiceImg).HasMaxLength(255);
            entity.Property(e => e.ServiceName).HasMaxLength(255);
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PK__Specialt__D768F6A8339DF18E");

            entity.ToTable("Specialty");

            entity.Property(e => e.SpecialtyId).HasMaxLength(255);
            entity.Property(e => e.ServiceId).HasMaxLength(255);
            entity.Property(e => e.SpecialtyImg).HasMaxLength(255);
            entity.Property(e => e.SpecialtyName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
