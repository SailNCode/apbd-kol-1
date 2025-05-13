using Microsoft.AspNetCore.Components.Sections;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Model_DTOs.post;
using Tutorial9.Repositories;

namespace Tutorial9.Services;


public class Service : IService
{
    private IRepository _repository;

    public Service(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<VisitInfo> GetVisitInfo(int visitId)
    {
        Visit visit = await _repository.GetVisit(visitId);
        Client client = await _repository.GetClient(visit.ClientId);
        Mechanic mechanic = await _repository.GetMechanic(visit.MechanicId);
        List<VisitService> visitServices = await _repository.GetVisitServices(visitId);
        Console.WriteLine(visitServices.Count);
        return new VisitInfo()
        {
            Date = visit.Date,
            Client = new ClientDto()
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                DateOfBirth = client.BirthDate
            },
            Mechanic = new MechanicDto()
            {
                MechanicID = mechanic.MechanicID,
                LicenceNumber = mechanic.LicenceNumber,
            },
            VisitServices = visitServices.Select(visitService => new VisitServiceDto()
            {
                Name = visitService.ServiceName,
                ServiceFee = visitService.ServiceFee,
            }).ToList()
        };
    }
    
    public async Task AddVisit(VisitPost visitPost)
    {
        Visit visit = null;
        try
        {
            visit = await _repository.GetVisit(visitPost.VisitId);
        }
        catch (NotFoundException e) { }
        if (visit != null)
        {
            throw new BadRequestException("Visit with such id exists");
        }

        Client client = await _repository.GetClient(visitPost.ClientId);;
        Mechanic mechanic = await _repository.GetMechanic(visitPost.MechanicLicenceNumber);
        List<VisitService> services = new List<VisitService>();
        foreach (var service in visitPost.Services)
        {
            var serviceDb = await _repository.getService(service.ServiceName);
            services.Add(new VisitService()
            {
                VisitId = visitPost.VisitId,
                ServiceId = serviceDb.ServiceId,
                ServiceFee = service.ServiceFee
            });
        }
        Console.WriteLine(services.Count);
        
        //Initiating transaction
        try
        {
            await _repository.StartTransactionAsync();
            await _repository.AddVisit(visitPost.VisitId, visitPost.ClientId, mechanic.MechanicID);
            await _repository.AddVisitServices(services);

        }
        catch (ConflictException e)
        {
            await _repository.RollbackTransactionAsync();
            throw e;
        }
        catch (InternalServerException e)
        {
            await _repository.RollbackTransactionAsync();
            throw e;
        }
        catch (Exception e)
        {
            await _repository.RollbackTransactionAsync();
            throw new InternalServerException(e.Message);
        }
        await _repository.CommitTransactionAsync();
        
        
    }
}