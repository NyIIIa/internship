using Enterprise.Model;
using Enterprise.Model.Enums;

namespace Enterprise.Services.Abstractions;

public interface IWorkmanRepository : IGenericProfileRepository<Workman>
{
    Task<ProfileStatus> IncreaseProfileAsync(Workman model, PositionTitle position);
}