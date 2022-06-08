using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class PositionController : Controller
{
    private readonly IHeadmasterRepository _headmasterRepository;
    private readonly ISubdivisionMasterRepository _subdivisionMasterRepository;
    private readonly IInspectorRepository _inspectorRepository;
    private readonly IWorkmanRepository _workmanRepository;
    
    
    public PositionController(
        IHeadmasterRepository headmasterRepository,
        ISubdivisionMasterRepository subdivisionMasterRepository,
        IInspectorRepository inspectorRepository,
        IWorkmanRepository workmanRepository)
    {
        _headmasterRepository = headmasterRepository;
        _subdivisionMasterRepository = subdivisionMasterRepository;
        _inspectorRepository = inspectorRepository;
        _workmanRepository = workmanRepository;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(PositionTitle position)
    {
        
        switch (position)
        {
            case PositionTitle.Headmaster:
                var positionsHeadmasters = await _headmasterRepository.GetProfilesAsync();
                return View("TableHeadmaster", positionsHeadmasters);
            case PositionTitle.SubdivisionMaster:
               var positionsSubdivisionMasters = await _subdivisionMasterRepository.GetProfilesAsync();
               return View("TableSubdivisionMaster", positionsSubdivisionMasters);
            case PositionTitle.Inspector:
               var positionsInspectors = await _inspectorRepository.GetProfilesAsync();
               return View("TableInspector", positionsInspectors);
            case PositionTitle.Workman:
                var positionsWorkman = await _workmanRepository.GetProfilesAsync();
                return View("TableWorkman", positionsWorkman);
            default:
                return NotFound();
        }
        
    }

    #region IncreaseSubdivisionMaster

    [HttpGet]
    public async Task<IActionResult> IncreaseSubdivisionMaster(int id)
    {
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster {Id = id};
        
        var subdivisionMasterFromDb = await _subdivisionMasterRepository.FindAsync(subdivisionMaster);
        
        if (subdivisionMasterFromDb != null)
        {
            return View(subdivisionMasterFromDb);    
        }
        return NotFound();

    }
    
    [HttpPost]
    public async Task<IActionResult> IncreaseSubdivisionMaster(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision, PositionTitle position)
    {
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _subdivisionMasterRepository.IncreaseProfileAsync(subdivisionMaster, position);

        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.SubdivisionHeadMasterExist:
                return Content($"Профиль директора подразделения {titleSubdivision} уже существует");
            default:
                return NotFound();
        }
    }

    #endregion

    #region IncreaseInspector

    [HttpGet]
    public async Task<IActionResult> IncreaseInspector(int id)
    {
        Inspector inspector = new Inspector {Id = id};
        
        var inspectorFromDb = await _inspectorRepository.FindAsync(inspector);
        
        if (inspectorFromDb != null)
        {
            return View(inspectorFromDb);    
        }
        return NotFound();

    }
    
    [HttpPost]
    public async Task<IActionResult> IncreaseInspector(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision, PositionTitle position)
    {
        Inspector inspector = new Inspector
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _inspectorRepository.IncreaseProfileAsync(inspector, position);

        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.SubdivisionHeadMasterExist:
                return Content($"Профиль директора подразделения {titleSubdivision} уже существует");
            case ProfileStatus.ProfileExist:
                return Content($"Профиль руководителя подразделения {titleSubdivision} c именем ${inspector.FirstName} и фамилией {inspector.LastName} уже существует");
            default:
                return NotFound();
        }
    }

    #endregion

    #region IncreaseWorkman

    [HttpGet]
    public async Task<IActionResult> IncreaseWorkman(int id)
    {
        Workman workman = new Workman {Id = id};
        
        var workmanFromDb = await _workmanRepository.FindAsync(workman);
        
        if (workmanFromDb != null)
        {
            return View(workmanFromDb);    
        }
        return NotFound();

    }
    
    [HttpPost]
    public async Task<IActionResult> IncreaseWorkman(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision, PositionTitle position)
    {
        Workman workman = new Workman
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _workmanRepository.IncreaseProfileAsync(workman, position);

        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.SubdivisionHeadMasterExist:
                return Content($"Профиль директора подразделения {titleSubdivision} уже существует");
            case ProfileStatus.ProfileExist:
                return Content($"Профиль руководителя/инспектора подразделения {titleSubdivision} c именем ${workman.FirstName} и фамилией {workman.LastName} уже существует");
            default:
                return NotFound();
        }
    }

    #endregion
    
}