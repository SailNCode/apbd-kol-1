using Tutorial9.Model_DTOs;

namespace Tutorial9.Repositories;

public interface IRepository: ITransactional
{
    Task<Client> GetClient(int clientId);

    Task<Visit> GetVisit(int visitId);
    Task<Mechanic> GetMechanic(int mechanicId);
    Task<Mechanic> GetMechanic(string mechanicLicenceNumber);

    Task<List<VisitService>> GetVisitServices(int visitId);
    Task<Service> getService(string serviceServiceName);
    Task AddVisit(int visitPostVisitId, int visitPostClientId, int mechanicMechanicId);
    Task AddVisitServices(List<VisitService> services);
}