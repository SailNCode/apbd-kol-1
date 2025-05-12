using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Services;

namespace Tutorial9.Repositories;

public class Repository: TransactionalRepository, IRepository
{

    public Repository(IConfiguration configuration): base(configuration) {}
    //Is present
    //ToRemove
    public async Task<bool> IsVisitPresent(int visitId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("Select 1 FROM [Visit] WHERE visit_id = @visitId", con);

        cmd.Parameters.AddWithValue("@visitId", visitId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync();
    }
    //Get object
    //ToRemove
    public async Task<ClientDTO> GetClient(int clientId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [ClientA] WHERE client_id = @clientId", con);

        cmd.Parameters.AddWithValue("@clientId", clientId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new ClientDTO
            {
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                DateOfBirth = (DateTime) reader["date_of_birth"]
            };
        }

        throw new NotFoundException("Client with such id doesn't exist");
    }
    //Get list
    //ToRemove
    public async Task<List<VisitServiceDTO>> GetVisits(int visitId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        string sql = @"SELECT s.name, s.base_fee, vs.service_fee
                        FROM Visit v
                        JOIN Visit_Service vs ON vs.visit_id = vs.visit_id
                        JOIN [Service] s ON s.service_id = vs.service_id
                        WHERE v.visit_id = @visitId";
        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@visitId", visitId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<VisitServiceDTO> services = new List<VisitServiceDTO>();
        while (await reader.ReadAsync())
        {
            VisitServiceDTO service = new VisitServiceDTO()
            {
                ServiceName = (string)reader["name"],
                ServiceFee = (decimal)reader["base_fee"] + (decimal)reader["service_fee"]
            };
            services.Add(service);
        }
        return services;
    }
    //Transactional method
    //ToRemove
    public async Task FulfillOrderAtCurrentDate(int orderId)
    {
        await using var cmd = new SqlCommand("UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = @orderId", TransConnection);
        cmd.Transaction = Transaction as SqlTransaction;
        cmd.Parameters.AddWithValue("@orderId", orderId);

        int nModified = await cmd.ExecuteNonQueryAsync();
        if (nModified != 1)
        {
            throw new InternalServerException("'FulfilledAt' date hasn't been added");
        }

    }
    //Scalar
    //ToRemove
    public async Task<int> AddProduct_WarehouseRecord(int warehouseId, int productId, int orderId, int amount, decimal price)
    {
        await using var insComm = new SqlCommand(@"INSERT INTO [Product_Warehouse] VALUES (@warehouseId, @productId, @orderId, @amount, @price, GETDATE()); SELECT SCOPE_IDENTITY()", TransConnection);
        insComm.Transaction = Transaction as SqlTransaction;
        insComm.Parameters.AddWithValue("@warehouseId", warehouseId);
        insComm.Parameters.AddWithValue("@productId", productId);
        insComm.Parameters.AddWithValue("@orderId", orderId);
        insComm.Parameters.AddWithValue("@amount", amount);
        insComm.Parameters.AddWithValue("@price", price);
        
        var result = await insComm.ExecuteScalarAsync();

        if (result == DBNull.Value)
            throw new InternalServerException("'Product_Warehouse' identity not returned");

        return Convert.ToInt32(result);
    }
}