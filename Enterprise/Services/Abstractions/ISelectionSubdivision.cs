using Enterprise.Model;

namespace Enterprise.Services.Abstractions;

public interface ISelectionSubdivision
{
    Task<IEnumerable<Subdivision>> GetAllSubdivisionAsync();
    Task<Subdivision> GetProfilesSubdivisionAsync(Subdivision subdivision);
}