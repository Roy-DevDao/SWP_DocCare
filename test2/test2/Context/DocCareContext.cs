
using Microsoft.EntityFrameworkCore;
using test2.Data;

namespace test2.Context
{
   public class DocCareContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DocCareContext(DbContextOptions<DocCareContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    // Các DbSet tương ứng với mỗi bảng
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<DetailSpecialty> DetailSpecialties { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DetailDoctor> DetailDoctors { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<HealthRecord> HealthRecords { get; set; }
    public DbSet<Payment> Payments { get; set; }

    // Cấu hình chuỗi kết nối từ appsettings.json
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyStore"));
        }

    // Tùy chỉnh mô hình (nếu cần) bằng Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Định nghĩa các quan hệ khóa ngoại giữa các bảng bằng Fluent API nếu cần
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Account)
            .WithMany(a => a.Doctors)
            .HasForeignKey(d => d.DId);

        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Patients)
            .HasForeignKey(p => p.PId);

        modelBuilder.Entity<DetailDoctor>()
            .HasOne(dd => dd.Doctor)
            .WithMany(d => d.DetailDoctors)
            .HasForeignKey(dd => dd.DId);

        modelBuilder.Entity<DetailSpecialty>()
            .HasOne(ds => ds.Specialty)
            .WithMany(s => s.DetailSpecialties)
            .HasForeignKey(ds => ds.SpecialtyId);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Doctor)
            .WithMany(d => d.Feedbacks)
            .HasForeignKey(f => f.DId);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Patient)
            .WithMany(p => p.Feedbacks)
            .HasForeignKey(f => f.PId);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(hr => hr.Patient)
            .WithMany(p => p.HealthRecords)
            .HasForeignKey(hr => hr.PId);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(hr => hr.Doctor)
            .WithMany(d => d.HealthRecords)
            .HasForeignKey(hr => hr.DId);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(hr => hr.Order)
            .WithMany(o => o.HealthRecords)
            .HasForeignKey(hr => hr.OId);

        base.OnModelCreating(modelBuilder);
    }
}
}
