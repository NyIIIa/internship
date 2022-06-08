using Enterprise.Model;
using Enterprise.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Services;

public class SubdivisionRepository : ISelectionSubdivision
{
    private readonly EnterpriseDbContext _dbContext;
    
    public SubdivisionRepository(EnterpriseDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Subdivision>> GetAllSubdivisionAsync()
    {
        return await _dbContext.Subdivisions.ToListAsync();
    }

    public async Task<Subdivision> GetProfilesSubdivisionAsync(Subdivision subdivision)
    {
        return await _dbContext.Subdivisions.Include(s => s.Headmaster)
            .Include(s => s.Inspectors)
            .Include(s => s.Workman)
            .Include(s => s.SubdivisionMasters).
            FirstOrDefaultAsync(s => s.Title == subdivision.Title);


    }
}