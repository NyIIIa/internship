using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class SubdivisionMasterRepository : ISubdivisionMasterRepository
{
    private readonly EnterpriseDbContext _dbContext;

    public SubdivisionMasterRepository(EnterpriseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProfileStatus> CreateAsync(SubdivisionMaster model)
    {
        var subdivision = await _dbContext.Subdivisions.Include(s => s.SubdivisionMasters).FirstOrDefaultAsync(s => s.Title ==model.Subdivision.Title);
        if (subdivision != null)
        {
            var isSubdivisionMaster =
                subdivision.SubdivisionMasters.Any(s => s.FirstName == model.FirstName && s.LastName == model.LastName);

            if (isSubdivisionMaster)
            {
                return ProfileStatus.ProfileExist;
            }
            else
            {
                model.Subdivision = subdivision;
                await _dbContext.SubdivisionMasters.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return ProfileStatus.Available;
            }
        }
        
        else
        {
           await _dbContext.SubdivisionMasters.AddAsync(model);
           await _dbContext.SaveChangesAsync();
           return ProfileStatus.Available;
        }
        

    }

    public async Task<ProfileStatus> UpdateAsync(SubdivisionMaster model)
    {
        if (model.FirstName == model.LastName)
        {
            return ProfileStatus.FailData;
        }
        
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title ==model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.SubdivisionMasters.Update(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }

        return ProfileStatus.FailData;
    }

    public async Task<ProfileStatus> RemoveAsync(SubdivisionMaster model)
    {
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.SubdivisionMasters.Remove(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;    
        }

        return ProfileStatus.FailData;
    }

    public async Task<SubdivisionMaster> FindAsync(SubdivisionMaster model)
    {
        return await _dbContext.SubdivisionMasters.Include(s => s.Subdivision).FirstOrDefaultAsync(s => s.Id == model.Id);
    }

    public async Task<IEnumerable<SubdivisionMaster>> GetProfilesAsync()
    {
        return await _dbContext.SubdivisionMasters.Include(s => s.Subdivision).ToListAsync();
    }

    public async Task<ProfileStatus> IncreaseProfileAsync(SubdivisionMaster model, PositionTitle position)
    {
        switch (position)
        {
            case PositionTitle.Headmaster:
                var subdivision = await _dbContext.Subdivisions.Include(s => s.Headmaster)
                    .FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
                if (subdivision != null)
                {
                    if (subdivision.Headmaster == null)
                    {
                        model.Subdivision = subdivision;
                        _dbContext.SubdivisionMasters.Remove(model);
                        Headmaster headmaster = new Headmaster
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PatronicName = model.PatronicName,
                            DateBirth = model.DateBirth,
                            Subdivision = subdivision
                        };
                        await _dbContext.Headmasters.AddAsync(headmaster);
                        await _dbContext.SaveChangesAsync();
                        return ProfileStatus.Available;
                    }
                    else
                    {
                        return ProfileStatus.SubdivisionHeadMasterExist;
                    }
                }
                else
                {
                    return ProfileStatus.FailData;
                }
            default:
                return ProfileStatus.NotFoundProfile;
                
        }
    }
}