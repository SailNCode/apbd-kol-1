using Tutorial9.Model_DTOs;
using Tutorial9.Model_DTOs.post;

namespace Tutorial9.Services;

public interface IService
{
    Task<VisitInfo> GetVisitInfo(int visitId);
    Task AddVisit(VisitPost visitPost);
}