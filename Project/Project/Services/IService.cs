using Tutorial9.Model_DTOs;

namespace Tutorial9.Services;

public interface IService
{
    //ToRemove
    Task<int> FulfillOrderRequest(R_FulfillOrderRequest rFulfillRequest);
}