using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class InspectorRepository : IInspectorRepository
{
    private readonly EnterpriseDbContext _dbContext;
    
    public InspectorRepository(EnterpriseDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProfileStatus> CreateAsync(Inspector model)
    {
        var subdivision = await _dbContext.Subdivisions.Include(s => s.Inspectors).FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
        
        if (subdivision != null)
        {
            var isInspector =
                subdivision.Inspectors.Any(i => i.FirstName == model.FirstName && i.LastName == model.LastName);

            if (isInspector)
            {
                return ProfileStatus.ProfileExist;
            }
            else
            {
                model.Subdivision = subdivision;
                await _dbContext.Inspectors.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return ProfileStatus.Available;
            }
        }
        
        else
        {
            await _dbContext.Inspectors.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }
    }

    public async Task<ProfileStatus> UpdateAsync(Inspector model)
    {
        if (model.FirstName == model.LastName)
        {
            return ProfileStatus.FailData;
        }
        
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title ==model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Inspectors.Update(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }

        return ProfileStatus.FailData;
    }

    public async Task<ProfileStatus> RemoveAsync(Inspector model)
    {
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
        
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Inspectors.Remove(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;    
        }

        return ProfileStatus.FailData;
    }

    public async Task<Inspector> FindAsync(Inspector model)
    {
        return await _dbContext.Inspectors.Include(i => i.Subdivision).FirstOrDefaultAsync(i => i.Id == model.Id);
    }

    public async Task<IEnumerable<Inspector>> GetProfilesAsync()
    {
        return await _dbContext.Inspectors.
            Include(i => i.Subdivision).ToListAsync();
    }

    public async Task<ProfileStatus> IncreaseProfileAsync(Inspector model, PositionTitle position)
    {
        Subdivision subdivision;
        switch (position)
        {
            case PositionTitle.Headmaster:
                 subdivision = await _dbContext.Subdivisions.Include(s => s.Headmaster)
                    .FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
                if (subdivision != null)
                {
                    if (subdivision.Headmaster == null)
                    {
                        model.Subdivision = subdivision;
                        _dbContext.Inspectors.Remove(model);
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
            case PositionTitle.SubdivisionMaster:
                
                 subdivision = await _dbContext.Subdivisions.Include(s => s.SubdivisionMasters)
                    .FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
                 
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
                         _dbContext.Inspectors.Remove(model);
                         
                         SubdivisionMaster subdivisionMaster = new SubdivisionMaster
                         {
                             FirstName = model.FirstName,
                             LastName = model.LastName,
                             PatronicName = model.PatronicName,
                             DateBirth = model.DateBirth,
                             Subdivision = subdivision
                         };
                         
                         await _dbContext.SubdivisionMasters.AddAsync(subdivisionMaster);
                         await _dbContext.SaveChangesAsync();
                         return ProfileStatus.Available;
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