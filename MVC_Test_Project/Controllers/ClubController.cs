using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Test_Project.Data;
using MVC_Test_Project.Interfaces;
using MVC_Test_Project.Models;
using MVC_Test_Project.ViewModels;

namespace MVC_Test_Project.Controllers;
public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photosService;

    public ClubController(IClubRepository clubRepository, IPhotoService photosService)
    {
        _clubRepository = clubRepository;
        _photosService = photosService;
    }
    public async Task <IActionResult> Index()
    {
        IEnumerable<Club> clubs = await _clubRepository.GetAll();
        return View(clubs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);
        return View(club);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]

    public async Task<IActionResult> Create(CreateClubViewModel clubVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photosService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = result.Url.ToString(),
                Address = new Address
                {
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City,
                    State = clubVM.Address.State,
                }
            };
        _clubRepository.Add(club);
        return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Photo upload failed");
        }
        return View(clubVM);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);
        if(club == null) return View("Error");
        var clubVM = new EditClubViewModel
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address,
            URL = club.Image,
            ClubCategory = club.ClubCategory
        };
        return View(clubVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit club");
            return View("Edit", clubVM);
        }
        var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

        if (userClub != null)
        {
            try
            {
                await _photosService.DeletePhotoAsync(userClub.Image);
            }
            catch (Exception) //if exception doesn't work, add ex in parentheses
            {
                ModelState.AddModelError("", "Could not delete photo");
                return View(clubVM);
            }
            var photoResult = await _photosService.AddPhotoAsync(clubVM.Image);

            var club = new Club
            {
                Id = id,
                Title = clubVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = clubVM.Address,
            };
            _clubRepository.Update(club);

            return RedirectToAction("Index");
        }
            else
            {
                return View(clubVM);
            }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var clubDetails = await _clubRepository.GetByIdAsync(id);

        if(clubDetails == null) return View("Error");
        return View(clubDetails);
    }

    [HttpPost, ActionName("Delete")]

    public async Task<IActionResult> DeleteClub(int id)
    {
        var clubDetails = await _clubRepository.GetByIdAsync(id);

        if (clubDetails == null) return View("Error");
        _clubRepository.Delete(clubDetails);
        return RedirectToAction("Index");
    }
}
