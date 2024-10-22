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

    public virtual DbSet<DetailSpecialty> DetailSpecialties { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Option> Options { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC075B06C957");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(255);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259B486D5E16");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<DetailDoctor>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailDo__135C316D653D8088");

            entity.ToTable("DetailDoctor");

            entity.Property(e => e.DetailId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.DetailDoctors)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__DetailDocto__DId__02084FDA");
        });

        modelBuilder.Entity<DetailSpecialty>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailSp__135C316D57B48838");

            entity.ToTable("DetailSpecialty");

            entity.Property(e => e.DetailId).HasMaxLength(50);
            entity.Property(e => e.SpecialtyId).HasMaxLength(255);

            entity.HasOne(d => d.Specialty).WithMany(p => p.DetailSpecialties)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__DetailSpe__Speci__7B5B524B");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK__Doctor__C036565080F7B23E");

            entity.ToTable("Doctor");

            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.SpecialtyId).HasMaxLength(255);

            entity.HasOne(d => d.DidNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.Did)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctor__DId__7E37BEF6");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__Doctor__Specialt__7F2BE32F");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD679E06C7A");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasMaxLength(255);
            entity.Property(e => e.DateCmt).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Feedback__DId__04E4BC85");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Feedback__PId__05D8E0BE");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78E99DB84968");

            entity.ToTable("HealthRecord");

            entity.Property(e => e.RecordId).HasMaxLength(255);
            entity.Property(e => e.DateExam).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__HealthRecor__DId__1332DBDC");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__HealthRecor__OId__14270015");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__HealthRecor__PId__123EB7A3");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__92C7A1FF835E1D23");

            entity.ToTable("Option");

            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.ScheduleId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Option__DId__0A9D95DB");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Options)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Option__Schedule__0B91BA14");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__Order__CB394B19E189345D");

            entity.ToTable("Order");

            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");
            entity.Property(e => e.DateOrder).HasColumnType("datetime");
            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");

            entity.HasOne(d => d.Option).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OptionId)
                .HasConstraintName("FK__Order__OptionId__0F624AF8");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Order__PId__0E6E26BF");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__Patient__C5775540B1E2F37C");

            entity.ToTable("Patient");

            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);

            entity.HasOne(d => d.PidNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PId__73BA3083");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__EE8FCECF4F640E4F");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasMaxLength(255);
            entity.Property(e => e.DatePay).HasColumnType("datetime");
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__Payment__OId__17036CC0");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B4981693873");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasMaxLength(255);
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PK__Specialt__D768F6A8191EE1F9");

            entity.ToTable("Specialty");

            entity.Property(e => e.SpecialtyId).HasMaxLength(255);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB17953DB67C");

            entity.Property(e => e.StaffId).HasMaxLength(255);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);

            entity.HasOne(d => d.StaffNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__StaffId__76969D2E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
