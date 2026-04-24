using Business.Managers.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies_web_app.Models;
using System.Diagnostics;

namespace Movies_web_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMovieManager _movieManager;


    public HomeController(ILogger<HomeController> logger, IMovieManager movieManager)
    {
        _logger = logger;
        _movieManager = movieManager;
    }
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var activeMovires=await _movieManager.GetAllCatalogMoviesAsync();
        return View(activeMovires);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
