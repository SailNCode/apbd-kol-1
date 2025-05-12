namespace Tutorial9.Model_DTOs;
//ToRemove
public class R_VisitInfo
{
    public DateTime Date { set; get; }
    public ClientDTO Client { set; get; }
    public MechanicDTO Mechanic { set; get; }
    public List<VisitServiceDTO> VisitServices { set; get; }
}

public class ClientDTO
{
    public string FirstName { set; get; }
    public string LastName { set; get; }
    public DateTime DateOfBirth { set; get; }
}

public class MechanicDTO
{
    public int MechanicId { set; get; }
    public string LicenceNumber { set; get; }
}

public class VisitServiceDTO
{
    public string ServiceName { set; get; }
    public decimal ServiceFee { set; get; }
}