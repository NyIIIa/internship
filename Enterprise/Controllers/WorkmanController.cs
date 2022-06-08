using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class WorkmanController : Controller
{
    private readonly IWorkmanRepository _workmanRepository;
    
    public WorkmanController(IWorkmanRepository workmanRepository)
    {
        _workmanRepository = workmanRepository;
    }
    
   [HttpGet]
    public async Task<IActionResult> Index()
    {
        var workmanList = await _workmanRepository.GetProfilesAsync();
        return View(workmanList);
    }

    #region Create method

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(string firstName, string lastName, string patronicName, DateTime dateBirth,
        string titleSubdivision, string fullNameMaster)
    {
        Workman workman = new Workman
        {
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth.ToString("MM/dd/yyyy"),
            Subdivision = new Subdivision {Title = titleSubdivision},
            FullNameSubdivisionMaster = fullNameMaster

        };
        ProfileStatus profileStatus = await _workmanRepository.CreateAsync(workman);

        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.SubdivisionMasterDoesNotExist:
                return View("ErrorAdd", workman);
            default:
                return NotFound();
        }
        
        
    }

    #endregion

    #region Edit method

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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
    public async Task<IActionResult> Edit(int id,string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision, string fullNameMaster)
    {
        Workman workman = new Workman
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision},
            FullNameSubdivisionMaster = fullNameMaster
        };

        ProfileStatus profileStatus = await _workmanRepository.UpdateAsync(workman);
        
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
        Workman workman = new Workman {Id = id};
        var workmanFromDb = await _workmanRepository.FindAsync(workman);
        if (workmanFromDb != null)
        {
            return View(workmanFromDb);
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision, string fullNameMaster)
    {
        Workman workman = new Workman
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision},
            FullNameSubdivisionMaster = fullNameMaster
        };

        ProfileStatus profileStatus = await _workmanRepository.RemoveAsync(workman);

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