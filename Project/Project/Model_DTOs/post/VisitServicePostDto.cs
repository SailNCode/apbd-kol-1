using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Model_DTOs.post;

public class VisitServicePostDto
{
    [Required] 
    public string ServiceName { get; set; }
    [Required]

    public decimal ServiceFee { get; set; }
}
