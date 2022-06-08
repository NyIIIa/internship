using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class SubdivisionMasterController : Controller
{
    private readonly ISubdivisionMasterRepository _subdivisionMasterRepository;
    
    public SubdivisionMasterController(ISubdivisionMasterRepository subdivisionMasterRepository)
    {
        _subdivisionMasterRepository = subdivisionMasterRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var subdivisionMasterList = await _subdivisionMasterRepository.GetProfilesAsync();
        return View(subdivisionMasterList);
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
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster
        {
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth.ToString("MM/dd/yyyy"),
            Subdivision = new Subdivision {Title = titleSubdivision}
        };
        
        ProfileStatus profileStatus = await _subdivisionMasterRepository.CreateAsync(subdivisionMaster);
        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.ProfileExist:
                return View("ErrorAdd", subdivisionMaster);
            default:
                return NotFound();
        }
        
    }

    #endregion


    #region Edit method

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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
    public async Task<IActionResult> Edit(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision)
    {
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision{Title = titleSubdivision}
        };
        ProfileStatus profileStatus = await _subdivisionMasterRepository.UpdateAsync(subdivisionMaster);
        
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
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster {Id = id};

        var subdivisionMasterFromDb = await _subdivisionMasterRepository.FindAsync(subdivisionMaster);

        if (subdivisionMasterFromDb != null)
        {
            return View(subdivisionMasterFromDb);   
        }

        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id, string firstName, string lastName, string patronicName, string dateBirth, string titleSubdivision)
    {
        SubdivisionMaster subdivisionMaster = new SubdivisionMaster
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision{Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _subdivisionMasterRepository.RemoveAsync(subdivisionMaster);
        
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