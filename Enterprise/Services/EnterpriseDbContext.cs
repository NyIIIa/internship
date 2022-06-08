using Enterprise.Model;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class EnterpriseDbContext : DbContext
{
    //Table Database
    public DbSet<Headmaster> Headmasters { get; set; }
    public DbSet<SubdivisionMaster> SubdivisionMasters { get; set; }
    public DbSet<Inspector> Inspectors { get; set; }
    public DbSet<Workman> Workman { get; set; }
    public DbSet<Subdivision> Subdivisions { get; set; }


    public EnterpriseDbContext(DbContextOptions<EnterpriseDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       //Configure One-to-One Relationships
       
       //Headmaster - Subdivision
       modelBuilder.Entity<Headmaster>().HasOne<Subdivision>(h => h.Subdivision).WithOne(s => s.Headmaster)
           .HasForeignKey<Headmaster>(h => h.SubdivisionId);
       
       
       
       //Configure One-to-Many Relationships
       
       //SubdivisionMaster - Subdivision
       modelBuilder.Entity<SubdivisionMaster>().
           HasOne<Subdivision>(m => m.Subdivision).
           WithMany(
           s => s.SubdivisionMasters).
           HasForeignKey(m => m.SubdivisionId);

       //Inspector - Subdivision
       modelBuilder.Entity<Inspector>().
           HasOne<Subdivision>(i => i.Subdivision).
           WithMany(
               s => s.Inspectors).
           HasForeignKey(i => i.SubdivisionId);
       
       //Workman - Subdivision
       modelBuilder.Entity<Workman>().
           HasOne<Subdivision>(w => w.Subdivision).
           WithMany(
               s => s.Workman).
           HasForeignKey(w => w.SubdivisionId);
       
    }
}