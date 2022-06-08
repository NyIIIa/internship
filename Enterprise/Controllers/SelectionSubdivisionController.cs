using Enterprise.Model;
using Enterprise.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Controllers;

public class SelectionSubdivisionController : Controller
{
    private readonly ISelectionSubdivision _selectionSubdivision;
    
    public SelectionSubdivisionController(ISelectionSubdivision selectionSubdivision)
    {
        _selectionSubdivision = selectionSubdivision;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var subdivisionList = await _selectionSubdivision.GetAllSubdivisionAsync();
        return View(subdivisionList);
    }
    [HttpPost]
    public async Task<IActionResult> Index(string titleSubdivision)
    {
        Subdivision subdivision = new Subdivision {Title = titleSubdivision};
        var subdivisionProfiles = await _selectionSubdivision.GetProfilesSubdivisionAsync(subdivision);
        if (subdivisionProfiles != null)
        {
            return View("Profiles", subdivisionProfiles);
        }

        return View("Error", subdivision);

    }
}