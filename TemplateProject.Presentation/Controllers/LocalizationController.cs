using Microsoft.AspNetCore.Mvc;
using TemplateProject.Application.Interfaces; // Sadece Application'ı biliyor!

namespace TemplateProject.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class LocalizationController : ControllerBase
{
    private readonly ILocalizationService _localizationService;

    // JsonLocalizationManager yerine ILocalizationService enjekte ediyoruz
    public LocalizationController(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    [HttpGet("{lang}")]
    public IActionResult GetTranslations(string lang)
    {
        var translations = _localizationService.GetAll(lang);
        return Ok(translations);
    }
}