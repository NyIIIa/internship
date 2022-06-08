using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class InspectorController : Controller
{
    private readonly IInspectorRepository _inspectorRepository;
    
    public InspectorController(IInspectorRepository inspectorRepository)
    {
        _inspectorRepository = inspectorRepository;
    }

    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var inspectorList = await _inspectorRepository.GetProfilesAsync();
        return View(inspectorList);
    }
    
    #region Create method

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string firstName, string lastName, string patronicName, DateTime dateBirth,
        string titleSubdivision)
    {
        Inspector inspector = new Inspector
        {
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth.ToString("MM/dd/yyyy"),
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _inspectorRepository.CreateAsync(inspector);
        
        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.ProfileExist:
                return View("ErrorAdd", inspector);
            default:
                return NotFound();
        }
    }
    
    #endregion

    #region Edit method

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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
    public async Task<IActionResult> Edit(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision)
    {
        Inspector inspector = new Inspector
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision{Title = titleSubdivision}
        };
        ProfileStatus profileStatus = await _inspectorRepository.UpdateAsync(inspector);
        
        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.FailData:
                return View("ErrorEdit");
            default:
                return NotFound();
        }
    }

    #endregion
    
    #region Delete method

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
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
    public async Task<IActionResult> Delete(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision)
    {
        Inspector inspector = new Inspector
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision{Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _inspectorRepository.RemoveAsync(inspector);
        
        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            default:
                return NotFound();
        }
    }

    #endregion
}