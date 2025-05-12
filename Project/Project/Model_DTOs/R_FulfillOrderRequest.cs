using System.ComponentModel.DataAnnotations;

namespace Tutorial9.Model_DTOs;

//ToRemove
public class R_FulfillOrderRequest
{
    [Required]
    public int IdProduct { set; get; }
    [Required]
    public int IdWarehouse { set; get; }
    [Required]
    public int Amount { set; get; }
    [Required]
    public DateTime CreatedAt { set; get; }
}