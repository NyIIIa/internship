using Enterprise.Model;
using Enterprise.Model.Enums;

namespace Enterprise.Services.Abstractions;

public interface ISubdivisionMasterRepository : IGenericProfileRepository<SubdivisionMaster>
{
    Task<ProfileStatus> IncreaseProfileAsync(SubdivisionMaster model, PositionTitle position);
}