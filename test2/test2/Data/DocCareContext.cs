using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace test2.Data;

public partial class DocCareContext : DbContext
{
    public DocCareContext()
    {
    }

    public DocCareContext(DbContextOptions<DocCareContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<DetailDoctor> DetailDoctors { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC073BC5F2E4");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259B1ACA961A");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<DetailDoctor>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailDo__135C316DA7459C45");

            entity.ToTable("DetailDoctor");

            entity.Property(e => e.DetailId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.DetailDoctors)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__DetailDocto__DId__300424B4");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK__Doctor__C03656509A1D7FCE");

            entity.ToTable("Doctor");

            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.DoctorImg).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Position).HasMaxLength(255);
            entity.Property(e => e.SpecialtyId).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.Did)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctor__DId__2C3393D0");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__Doctor__Specialt__2D27B809");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD60B7365E4");

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
                .HasConstraintName("FK__Feedback__DId__32E0915F");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Feedback__PId__33D4B598");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78E9EBF1AA88");

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
                .HasConstraintName("FK__HealthRecor__DId__37A5467C");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__HealthRecor__PId__36B12243");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__92C7A1FF4A20296D");

            entity.ToTable("Option");

            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.DateExam).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Option__DId__3D5E1FD2");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__Order__CB394B1904760C24");

            entity.ToTable("Order");

            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");
            entity.Property(e => e.DateOrder).HasColumnType("datetime");
            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Option).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OptionId)
                .HasConstraintName("FK__Order__OptionId__412EB0B6");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Order__PId__403A8C7D");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__Patient__C57755401AA80BBD");

            entity.ToTable("Patient");

            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PatientImg).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);

            entity.HasOne(d => d.PidNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PId__276EDEB3");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__EE8FCECF07C1E169");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasMaxLength(255);
            entity.Property(e => e.DatePay).HasColumnType("datetime");
            entity.Property(e => e.Method).HasMaxLength(255);
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");
            entity.Property(e => e.PayImg).HasMaxLength(255);

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__Payment__OId__440B1D61");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B49FF7873FF");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Schedule__DId__3A81B327");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PK__Specialt__D768F6A87E8DDBEE");

            entity.ToTable("Specialty");

            entity.Property(e => e.SpecialtyId).HasMaxLength(255);
            entity.Property(e => e.SpecialtyImg).HasMaxLength(255);
            entity.Property(e => e.SpecialtyName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
