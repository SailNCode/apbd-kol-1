using Microsoft.AspNetCore.Components.Sections;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Repositories;

namespace Tutorial9.Services;


public class Service : IService
{
    private IRepository _repository;

    public Service(IRepository repository)
    {
        _repository = repository;
    }
    
    //ToRemove
    public async Task<int> FulfillOrderRequest(R_FulfillOrderRequest rFulfillRequest)
    {
        if (rFulfillRequest.Amount <= 0)
        {
            throw new BadRequestException("Amount should be greater than 0");
        }
        // if (! await _repository.IsProductPresent(fulfillRequest.IdProduct))
        // {
        //     throw new NotFoundException("IdProduct not found");
        // }

        int orderId = 1; //await _repository.GetOrderId(fulfillRequest.IdProduct, fulfillRequest.Amount);
        
        int foreignKey = -1;
        //Initiating transaction
        try
        {
            await _repository.StartTransactionAsync();

            await _repository.FulfillOrderAtCurrentDate(orderId);
            
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
        return foreignKey;
    }
    
}