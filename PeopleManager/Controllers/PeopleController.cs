using Microsoft.AspNetCore.Mvc;
using PeopleManager.Models;
using PeopleManager.Services;

namespace PeopleManager.Controllers;

public class PeopleController : Controller
{
    private readonly PersonRepository _repository;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(PersonRepository repository, ILogger<PeopleController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: /People
    public IActionResult Index()
    {
        _logger.PersonListViewed();
        var people = _repository.GetAll();
        return View(people);
    }

    // GET: /People/Details/5
    public IActionResult Details(int id)
    {
        var person = _repository.GetById(id);
        if (person == null)
        {
            _logger.PersonNotFound(id);
            return NotFound();
        }

        _logger.PersonDetailsViewed(id, person.FullName);
        return View(person);
    }

    // GET: /People/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /People/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Person person)
    {
        if (!ModelState.IsValid)
            return View(person);

        _repository.Add(person);
        _logger.PersonCreated(person.Id, person.FullName);
        return RedirectToAction(nameof(Index));
    }

    // GET: /People/Edit/5
    public IActionResult Edit(int id)
    {
        var person = _repository.GetById(id);
        if (person == null)
        {
            _logger.PersonNotFound(id);
            return NotFound();
        }

        return View(person);
    }

    // POST: /People/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Person person)
    {
        if (id != person.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(person);

        if (!_repository.Update(person))
        {
            _logger.PersonNotFound(id);
            return NotFound();
        }

        _logger.PersonUpdated(person.Id, person.FullName);
        return RedirectToAction(nameof(Index));
    }

    // GET: /People/Delete/5
    public IActionResult Delete(int id)
    {
        var person = _repository.GetById(id);
        if (person == null)
        {
            _logger.PersonNotFound(id);
            return NotFound();
        }

        return View(person);
    }

    // POST: /People/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var person = _repository.GetById(id);
        if (!_repository.Delete(id))
        {
            _logger.PersonNotFound(id);
            return NotFound();
        }

        _logger.PersonDeleted(id, person?.FullName ?? string.Empty);
        return RedirectToAction(nameof(Index));
    }
}

internal static partial class PeopleLoggerExtensions
{
    [LoggerMessage(LogLevel.Information, "Viewing list of all people.")]
    public static partial void PersonListViewed(this ILogger logger);

    [LoggerMessage(LogLevel.Information, "Viewing details for person with id `{id}` (`{fullName}`).")]
    public static partial void PersonDetailsViewed(this ILogger logger, int id, string fullName);

    [LoggerMessage(LogLevel.Warning, "Person with id `{id}` was not found.")]
    public static partial void PersonNotFound(this ILogger logger, int id);

    [LoggerMessage(LogLevel.Information, "Person created with id `{id}` and name `{fullName}`.")]
    public static partial void PersonCreated(this ILogger logger, int id, string fullName);

    [LoggerMessage(LogLevel.Information, "Person with id `{id}` (`{fullName}`) was updated.")]
    public static partial void PersonUpdated(this ILogger logger, int id, string fullName);

    [LoggerMessage(LogLevel.Information, "Person with id `{id}` (`{fullName}`) was deleted.")]
    public static partial void PersonDeleted(this ILogger logger, int id, string fullName);
}
