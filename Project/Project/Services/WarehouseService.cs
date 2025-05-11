using Microsoft.AspNetCore.Components.Sections;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Repositories;

namespace Tutorial9.Services;


public class WarehouseService : IWarehouseService
{
    private IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<int> FulfillOrderRequest(FulfillOrderRequest fulfillRequest)
    {
        if (fulfillRequest.Amount <= 0)
        {
            throw new BadRequestException("Amount should be greater than 0");
        }
        if (! await _warehouseRepository.IsProductPresent(fulfillRequest.IdProduct))
        {
            throw new NotFoundException("IdProduct not found");
        }
        
        int orderId = await _warehouseRepository.GetOrderId(fulfillRequest.IdProduct, fulfillRequest.Amount);
        
        int foreignKey = -1;
        //Initiating transaction
        try
        {
            await _warehouseRepository.StartTransactionAsync();

            await _warehouseRepository.FulfillOrderAtCurrentDate(orderId);
            
        }
        catch (ConflictException e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw e;
        }
        catch (InternalServerException e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw e;
        }
        catch (Exception e)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            throw new InternalServerException(e.Message);
        }
        await _warehouseRepository.CommitTransactionAsync();
        return foreignKey;
    }
    
}