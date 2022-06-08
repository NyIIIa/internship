using Enterprise.Model;
using Enterprise.Model.Enums;
using Enterprise.Services;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class HeadmasterController : Controller
{
    private readonly IHeadmasterRepository _headmasterRepository;
    
    public HeadmasterController(IHeadmasterRepository headmasterRepository)
    {
        _headmasterRepository = headmasterRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var headmasterList = await _headmasterRepository.GetProfilesAsync();
        return View(headmasterList);
    }

    #region Create methods

    [HttpPost]
    public async Task<IActionResult> Create(string firstName, string lastName, string patronicName,
        DateTime dateBirth, string titleSubdivision)
    {
        
        Headmaster headmaster = new Headmaster
        {
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth.ToString("MM/dd/yyyy"),
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _headmasterRepository.CreateAsync(headmaster);
        
        switch (profileStatus)
        {
            case ProfileStatus.Available:
                return RedirectToAction("Index");
            case ProfileStatus.SubdivisionHeadMasterExist:
                return View("ErrorAdd", headmaster);
            default:
                return NotFound();
        }
        
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    #endregion

    #region Edit methods

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        Headmaster headmaster = new Headmaster {Id = id};
        var headmasterFromDb = await _headmasterRepository.FindAsync(headmaster);
        
        if (headmasterFromDb != null)
        {
            return View(headmasterFromDb);    
        }
        return NotFound();

    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string firstName, string lastName, string patronicName, 
        string dateBirth, string titleSubdivision)
    {
        Headmaster headmaster = new Headmaster
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _headmasterRepository.UpdateAsync(headmaster);
        
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
    

    #region Delete methods

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Headmaster headmaster = new Headmaster {Id = id};
        var headmasterFromDb = await _headmasterRepository.FindAsync(headmaster);
        
        if (headmasterFromDb != null)
        {
            return View(headmasterFromDb);    
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id, string firstName, string lastName, string patronicName, 
        string dateBirth, string titleSubdivision)
    {
        Headmaster headmaster = new Headmaster
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PatronicName = patronicName,
            DateBirth = dateBirth,
            Subdivision = new Subdivision {Title = titleSubdivision}
        };

        ProfileStatus profileStatus = await _headmasterRepository.RemoveAsync(headmaster);
        
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