using Enterprise.Model;
using Enterprise.Model.Enums;

namespace Enterprise.Services.Abstractions;

public interface IGenericProfileRepository<Tkey> where Tkey : EntityBase
{
    Task<ProfileStatus> CreateAsync(Tkey model);
    Task<ProfileStatus> UpdateAsync(Tkey model);
    Task<ProfileStatus> RemoveAsync(Tkey model);
    Task<Tkey> FindAsync(Tkey model);
    Task<IEnumerable<Tkey>> GetProfilesAsync();
}