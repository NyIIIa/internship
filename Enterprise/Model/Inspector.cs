using System.ComponentModel.DataAnnotations;

namespace Enterprise.Model;

public class Inspector: EntityBase
{   
    [MaxLength(40)]
    [Required]
    public string FirstName { get; set; }
    [MaxLength(40)]
    [Required]
    public string LastName { get; set; }
    [MaxLength(40)]
    [Required]
    public string PatronicName { get; set; }
    [MaxLength(40)]
    [Required]
    public string DateBirth { get; set; }
    public int SubdivisionId { get; set; }
    public Subdivision Subdivision { get; set; }
    
}