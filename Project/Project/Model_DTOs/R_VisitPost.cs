using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Model_DTOs;

//ToRemove
public class R_VisitPost
{
    [Required]
    public int VisitId { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int ClientId { get; set; }
    [Required]
    [MaxLength(30)]
    public string MechanicLicenceNumber { get; set; }
    public List<VisitServiceDTO> Services { get; set; }
}