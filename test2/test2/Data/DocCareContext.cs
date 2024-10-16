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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TIS;Initial Catalog=DocCare;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC076D3CCF75");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(255);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259BE93FA067");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<DetailDoctor>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailDo__135C316D4D12D7E2");

            entity.ToTable("DetailDoctor");

            entity.Property(e => e.DetailId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.DetailDoctors)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__DetailDocto__DId__46E78A0C");
        });

        modelBuilder.Entity<DetailSpecialty>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailSp__135C316D84CF49F7");

            entity.ToTable("DetailSpecialty");

            entity.Property(e => e.DetailId).HasMaxLength(50);
            entity.Property(e => e.SpecialtyId).HasMaxLength(255);

            entity.HasOne(d => d.Specialty).WithMany(p => p.DetailSpecialties)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__DetailSpe__Speci__403A8C7D");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK__Doctor__C03656506862BF8A");

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
                .HasConstraintName("FK__Doctor__DId__4316F928");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__Doctor__Specialt__440B1D61");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD670CD57F8");

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
                .HasConstraintName("FK__Feedback__DId__49C3F6B7");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Feedback__PId__4AB81AF0");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78E9992BA84F");

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
                .HasConstraintName("FK__HealthRecor__DId__5812160E");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__HealthRecor__OId__59063A47");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__HealthRecor__PId__571DF1D5");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__92C7A1FFDB19ED25");

            entity.ToTable("Option");

            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.ScheduleId).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Option__DId__4F7CD00D");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Options)
                .HasForeignKey(d => d.ScheduleId)
                .HasConstraintName("FK__Option__Schedule__5070F446");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__Order__CB394B1969DDABE1");

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
                .HasConstraintName("FK__Order__OptionId__5441852A");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Order__PId__534D60F1");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__Patient__C577554000CC7889");

            entity.ToTable("Patient");

            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);

            entity.HasOne(d => d.PidNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PId__3B75D760");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__EE8FCECF07E95411");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasMaxLength(255);
            entity.Property(e => e.DatePay).HasColumnType("datetime");
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__Payment__OId__5BE2A6F2");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B49BFC0212C");

            entity.ToTable("Schedule");

            entity.Property(e => e.ScheduleId).HasMaxLength(255);
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PK__Specialt__D768F6A8F602EB58");

            entity.ToTable("Specialty");

            entity.Property(e => e.SpecialtyId).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
