using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class WorkmanRepository : IWorkmanRepository
{
    private readonly EnterpriseDbContext _dbContext;

    public WorkmanRepository(EnterpriseDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ProfileStatus> CreateAsync(Workman model)
    {
        var FullNameMaster =  model.FullNameSubdivisionMaster.Split(" ");
        if (FullNameMaster.Length != 3)
        {
            return ProfileStatus.FailData;
        }
        else
        {
            var LastNameMaster = FullNameMaster[0];
            var FirstNameMaster = FullNameMaster[1];
            var PatronicNameMaster = FullNameMaster[2];
            var subdivisionMaster = await _dbContext.SubdivisionMasters.Include(s => s.Subdivision).
                FirstOrDefaultAsync(
                    s =>
                        s.FirstName == FirstNameMaster && s.LastName == LastNameMaster && s.PatronicName == PatronicNameMaster && 
                        s.Subdivision.Title == model.Subdivision.Title);
            
            if (subdivisionMaster == null)
            {
                return ProfileStatus.SubdivisionMasterDoesNotExist;
            }
            else
            {
                model.Subdivision = subdivisionMaster.Subdivision;
                await _dbContext.Workman.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return ProfileStatus.Available;
            }
        }
        
    }

    public async Task<ProfileStatus> UpdateAsync(Workman model)
    {
        
        if (model.FirstName == model.LastName)
        {
            return ProfileStatus.FailData;
        }
        
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title ==model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Workman.Update(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;
        }

        return ProfileStatus.FailData;

    }

    public async Task<ProfileStatus> RemoveAsync(Workman model)
    {
        var subdivision = await _dbContext.Subdivisions.FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
        if (subdivision != null)
        {
            model.Subdivision = subdivision;
            _dbContext.Workman.Remove(model);
            await _dbContext.SaveChangesAsync();
            return ProfileStatus.Available;    
        }

        return ProfileStatus.FailData;
    }

    public async Task<Workman> FindAsync(Workman model)
    {
        return await _dbContext.Workman.Include(w => w.Subdivision).FirstOrDefaultAsync(w => w.Id == model.Id);
    }

    public async Task<IEnumerable<Workman>> GetProfilesAsync()
    {
       return await _dbContext.Workman.Include(w => w.Subdivision).ToListAsync();
    }

    public async Task<ProfileStatus> IncreaseProfileAsync(Workman model, PositionTitle position)
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
                        _dbContext.Workman.Remove(model);
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
                         _dbContext.Workman.Remove(model);
                         
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
            case PositionTitle.Inspector:
                subdivision = await _dbContext.Subdivisions.Include(s => s.Inspectors)
                    .FirstOrDefaultAsync(s => s.Title == model.Subdivision.Title);
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
                        _dbContext.Workman.Remove(model);
                         
                        Inspector inspector = new Inspector
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PatronicName = model.PatronicName,
                            DateBirth = model.DateBirth,
                            Subdivision = subdivision
                        };
                        
                        await _dbContext.Inspectors.AddAsync(inspector);
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