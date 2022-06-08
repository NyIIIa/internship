using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class HeadmasterRepository : IHeadmasterRepository
{
    private readonly EnterpriseDbContext _dbContext;
   

    public HeadmasterRepository(EnterpriseDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProfileStatus> CreateAsync(Headmaster model)
    {

        var subdivision = await _dbContext.Subdivisions.Include(s => s.Headmaster).FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);

        if (subdivision != null)
        {
            if (subdivision.Headmaster == null)
            {
                model.Subdivision = subdivision;
                await _dbContext.Headmasters.AddAsync(model);
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
            await _dbContext.Headmasters.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }
        
    }

    public async Task<ProfileStatus> UpdateAsync(Headmaster model)
    {
        if (model.FirstName == model.LastName)
        {
            return ProfileStatus.FailData;
        }

        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);

        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Headmasters.Update(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }

        return ProfileStatus.FailData;
    }
    
    public async Task<ProfileStatus> RemoveAsync(Headmaster model)
    {
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Headmasters.Remove(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;    
        }

        return ProfileStatus.FailData;

    }
    
    public async Task<Headmaster> FindAsync(Headmaster model)
    {
        return await _dbContext.Headmasters.Include(h => h.Subdivision).FirstOrDefaultAsync(h => h.Id == model.Id);
    }
    
    public async Task<IEnumerable<Headmaster>> GetProfilesAsync()
    {
        return await _dbContext.Headmasters.
            Include(h => h.Subdivision).ToListAsync();
    }
    
}