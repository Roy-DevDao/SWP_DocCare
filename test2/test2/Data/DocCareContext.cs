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

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-PG;Initial Catalog=DocCare;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3214EC070B2CF95F");

            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(255);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK__Contact__5C66259BBDDF81C9");

            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<DetailDoctor>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailDo__135C316DFE091D68");

            entity.ToTable("DetailDoctor");

            entity.Property(e => e.DetailId).HasMaxLength(255);
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.DetailDoctors)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__DetailDocto__DId__59063A47");
        });

        modelBuilder.Entity<DetailSpecialty>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__DetailSp__135C316D6CBE22F5");

            entity.ToTable("DetailSpecialty");

            entity.Property(e => e.DetailId).HasMaxLength(50);
            entity.Property(e => e.SpecialtyId).HasMaxLength(255);

            entity.HasOne(d => d.Specialty).WithMany(p => p.DetailSpecialties)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__DetailSpe__Speci__52593CB8");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PK__Doctor__C036565036269B63");

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
                .HasConstraintName("FK__Doctor__DId__5535A963");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialtyId)
                .HasConstraintName("FK__Doctor__Specialt__5629CD9C");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDD6E2917BE4");

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
                .HasConstraintName("FK__Feedback__DId__5BE2A6F2");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Feedback__PId__5CD6CB2B");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__HealthRe__FBDF78E99D2C4EED");

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
                .HasConstraintName("FK__HealthRecor__DId__6754599E");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__HealthRecor__OId__68487DD7");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__HealthRecor__PId__66603565");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__Option__92C7A1FF342ECC2F");

            entity.ToTable("Option");

            entity.Property(e => e.OptionId).HasMaxLength(255);
            entity.Property(e => e.DateWork).HasColumnType("datetime");
            entity.Property(e => e.Did)
                .HasMaxLength(255)
                .HasColumnName("DId");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.Options)
                .HasForeignKey(d => d.Did)
                .HasConstraintName("FK__Option__DId__5FB337D6");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Oid).HasName("PK__Order__CB394B19C2F71B0F");

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
                .HasConstraintName("FK__Order__OptionId__6383C8BA");

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Pid)
                .HasConstraintName("FK__Order__PId__628FA481");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Pid).HasName("PK__Patient__C5775540F653161D");

            entity.ToTable("Patient");

            entity.Property(e => e.Pid)
                .HasMaxLength(255)
                .HasColumnName("PId");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);

            entity.HasOne(d => d.PidNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__PId__4D94879B");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PayId).HasName("PK__Payment__EE8FCECF47983189");

            entity.ToTable("Payment");

            entity.Property(e => e.PayId).HasMaxLength(255);
            entity.Property(e => e.DatePay).HasColumnType("datetime");
            entity.Property(e => e.Oid)
                .HasMaxLength(255)
                .HasColumnName("OId");

            entity.HasOne(d => d.OidNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Oid)
                .HasConstraintName("FK__Payment__OId__6B24EA82");
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PK__Specialt__D768F6A8DAF8297B");

            entity.ToTable("Specialty");

            entity.Property(e => e.SpecialtyId).HasMaxLength(255);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staff__96D4AB17C970B980");

            entity.Property(e => e.StaffId).HasMaxLength(255);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Gender).HasMaxLength(50);

            entity.HasOne(d => d.StaffNavigation).WithOne(p => p.Staff)
                .HasForeignKey<Staff>(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__StaffId__6E01572D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
