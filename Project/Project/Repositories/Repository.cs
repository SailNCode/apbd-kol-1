using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Model_DTOs;
using Tutorial9.Exceptions;
using Tutorial9.Services;
using Service = Tutorial9.Model_DTOs.Service;

namespace Tutorial9.Repositories;

public class Repository: TransactionalRepository, IRepository
{

    public Repository(IConfiguration configuration): base(configuration) {}

    public async Task<Visit> GetVisit(int visitId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [Visit] WHERE visit_id = @visitId", con);

        cmd.Parameters.AddWithValue("@visitId", visitId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Visit()
            {
                VisitId = (int) reader["visit_id"],
                ClientId = (int) reader["client_id"],
                MechanicId = (int) reader["mechanic_id"],
                Date = (DateTime) reader["date"],
            };
        }

        throw new NotFoundException("Visit with such id doesn't exist");
    }
    public async Task<Client> GetClient(int clientId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [Client] WHERE client_id = @clientId", con);

        cmd.Parameters.AddWithValue("@clientId", clientId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Client
            {
                ClientId = (int) reader["client_id"],
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                BirthDate = (DateTime) reader["date_of_birth"]
            };
        }

        throw new NotFoundException("Client with such id doesn't exist");
    }
    public async Task<Mechanic> GetMechanic(int mechanicId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [Mechanic] WHERE mechanic_id = @mechanicId", con);

        cmd.Parameters.AddWithValue("@mechanicId", mechanicId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Mechanic
            {
                MechanicID = (int) reader["mechanic_id"],
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                LicenceNumber = reader["licence_number"].ToString()
            };
        }

        throw new NotFoundException("Mechanic with such id doesn't exist");
    }

    public async Task<List<VisitService>> GetVisitServices(int visitId)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        string sql = @"SELECT v.visit_id, s.name, s.service_id, vs.service_fee
                        FROM Visit v
                        JOIN Visit_Service vs ON vs.visit_id = v.visit_id
                        JOIN [Service] s ON s.service_id = vs.service_id
                        WHERE v.visit_id = @visitId";
        await using var cmd = new SqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@visitId", visitId);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        List<VisitService> services = new List<VisitService>();
        while (await reader.ReadAsync())
        {
            VisitService service = new VisitService()
            {
                VisitId = visitId,
                ServiceId = (int) reader["service_id"],
                ServiceName = (string)reader["name"],
                ServiceFee = (decimal)reader["service_fee"]
            };
            services.Add(service);
        }
        return services;
    }

    public async Task<Mechanic> GetMechanic(string mechanicLicenceNumber)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [Mechanic] WHERE licence_number = @mechanicLicenceNumber", con);

        cmd.Parameters.AddWithValue("@mechanicLicenceNumber", mechanicLicenceNumber);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Mechanic
            {
                MechanicID = (int) reader["mechanic_id"],
                FirstName = reader["first_name"].ToString(),
                LastName = reader["last_name"].ToString(),
                LicenceNumber = reader["licence_number"].ToString()
            };
        }

        throw new NotFoundException("Mechanic with such licence number doesn't exist");
    }
    public async Task<Service> getService(string serviceServiceName)
    {
        await using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var cmd = new SqlCommand("SELECT * FROM [Service] WHERE name = @serviceServiceName", con);

        cmd.Parameters.AddWithValue("@serviceServiceName", serviceServiceName);

        await con.OpenAsync();
        SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Service()
            {
                ServiceId = (int)reader["service_id"],
                Name = (string)reader["name"],
                BaseFee = (decimal)reader["base_fee"]
            };
        }

        throw new NotFoundException("Service with name " + serviceServiceName + " does not exist");
    }

    public async Task AddVisit(int visitPostVisitId, int visitPostClientId, int mechanicMechanicId)
    {
        await using var insComm = new SqlCommand(@"INSERT INTO [Visit] VALUES (@visitId, @clientId, @mechanicId, GETDATE())", TransConnection);
        insComm.Transaction = Transaction as SqlTransaction;
        insComm.Parameters.AddWithValue("@visitId", visitPostVisitId);
        insComm.Parameters.AddWithValue("@clientId", visitPostClientId);
        insComm.Parameters.AddWithValue("@mechanicId", mechanicMechanicId);
        
        int nModified = await insComm.ExecuteNonQueryAsync();
        if (nModified != 1)
        {
            throw new InternalServerException("Visit wasn't added");
        }
    }

    public async Task AddVisitServices(List<VisitService> services)
    {
        foreach (var service in services)
        {
            await using var insComm = new SqlCommand(@"INSERT INTO [Visit_Service] VALUES (@visitId, @serviceId, @serviceFee)", TransConnection);
            insComm.Transaction = Transaction as SqlTransaction;
            insComm.Parameters.AddWithValue("@visitId", service.VisitId);
            insComm.Parameters.AddWithValue("@serviceId", service.ServiceId);
            insComm.Parameters.AddWithValue("@serviceFee", service.ServiceFee);
        
            int nModified = await insComm.ExecuteNonQueryAsync();
            if (nModified != 1)
            {
                throw new InternalServerException("Visit wasn't added");
            }
        }
        
    }
}