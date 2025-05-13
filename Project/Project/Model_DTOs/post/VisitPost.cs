using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Model_DTOs.post;

public class VisitPost
{
    [Required]
    public int VisitId { get; set; }
    [Required]
    public int ClientId { get; set; }
    [Required]
    public string MechanicLicenceNumber { get; set; }
    [Required]
    public List<VisitServicePostDto> Services { get; set; }

}