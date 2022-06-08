using System.ComponentModel.DataAnnotations;

namespace Enterprise.Model;

public class Subdivision
{
    [Key]
    public int Id { get; set; }
    [MaxLength(50)]
    [Required]
    public string Title { get; set; }
    public Headmaster Headmaster { get; set; }
    public IEnumerable<SubdivisionMaster> SubdivisionMasters { get; set; }
    public IEnumerable<Inspector> Inspectors { get; set; }
    public IEnumerable<Workman> Workman { get; set; }
    
}