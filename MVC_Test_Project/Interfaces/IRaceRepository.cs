using MVC_Test_Project.Models;

namespace MVC_Test_Project.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race>> GetAll();
    Task<Race> GetByIdAsync(int id);
    Task<Race> GetByIdAsyncNoTracking(int id);
    Task<IEnumerable<Race>> GetAllRacesByCity(string city);
    bool Add(Race race);
    bool Update(Race race);
    bool Delete(Race race);
    bool Save();
}
