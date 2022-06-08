using Enterprise.Model;
using Enterprise.Model.Enums;

namespace Enterprise.Services.Abstractions;

public interface IInspectorRepository : IGenericProfileRepository<Inspector>
{
    Task<ProfileStatus> IncreaseProfileAsync(Inspector model, PositionTitle position);
}