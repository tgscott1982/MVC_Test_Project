using MVC_Test_Project.Data.Enum;
using MVC_Test_Project.Models;

namespace MVC_Test_Project.ViewModels;

public class CreateRaceViewModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Address Address { get; set; }
    public IFormFile Image { get; set; }
    public RaceCategory RaceCategory { get; set; }
}
