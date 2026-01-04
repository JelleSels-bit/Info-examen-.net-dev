using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Interimkantoor.Data;
using Interimkantoor.Models;
using Interimkantoor.ViewModels.Job;

namespace Interimkantoor.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class JobController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public JobController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Job
        public async Task<IActionResult> Index()
        {
            var unMappedVacatures = await _context.JobRepository.GetAllAsync();

            var vm = new JobIndexViewModel
            {
                Vacatures = unMappedVacatures
                .Select(job => _mapper.Map<JobItemViewModel>(job))
                .ToList()
            };

            return View(vm);
        }

        // GET: Job/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.JobRepository.GetJobAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Er zit een fout in het model");

            var Job = _mapper.Map<Job>(vm);
            await _context.JobRepository.AddAsync(Job);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Er is een probleem opgetreden bij het wegschrijven naar de database.");
                return View();
            }
        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var job = await _context.JobRepository.GetJobAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            var klanten = await _context.KlantRepository.GetAllAsync();

            EditJobViewModel viewModel = new EditJobViewModel
            {
                Id = id,
                Omschrijving = job.Omschrijving,
                StartDatum = job.StartDatum,
                EindDatum = job.EindDatum,
                Locatie = job.Locatie,
                IsBadge = job.IsBadge,
                IsKleding = job.IsKleding,
                IsWerkschoenen = job.IsWerkschoenen,
                AantalPlaatsen = job.AantalPlaatsen,

                KlantJobs = job.KlantJobs.Select(kj => new KlantJobListViewModel
                {
                    KlantId = kj.KlantId,
                }).ToList(),

                Klanten = klanten.Select(c => new SelectListItem
                {
                    Value = c.Id,
                    Text = c.Naam + " " + c.Voornaam
                })
                .ToList()
            };

            return View(viewModel);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] // Zorgt ervoor dat het formulier een geldig anti-forgery token heeft, om CSRF-aanvallen te voorkomen
        public async Task<IActionResult> Edit(int id, EditJobViewModel viewModel)
        {
            // Controleer of het id in de URL overeenkomt met het id in het ViewModel
            if (id != viewModel.Id)
            {
                return NotFound(); // Zo niet, geef een 404 terug
            }

            // Haal de Job op uit de repository/database
            Job job = await _context.JobRepository.GetJobAsync(id);

            // Controleer of de Job bestaat
            if (job == null)
            {
                return NotFound(); // Zo niet, geef een 404 terug
            }

            // Controleer of het model geldig is (DataAnnotations, required velden, etc.)
            if (ModelState.IsValid)
            {
                // Update de velden van de Job met de waarden uit het ViewModel
                job.Id = viewModel.Id;
                job.Omschrijving = viewModel.Omschrijving;
                job.StartDatum = viewModel.StartDatum;
                job.EindDatum = viewModel.EindDatum;
                job.Locatie = viewModel.Locatie;
                job.IsBadge = viewModel.IsBadge;
                job.IsKleding = viewModel.IsKleding;
                job.IsWerkschoenen = viewModel.IsWerkschoenen;
                job.AantalPlaatsen = viewModel.AantalPlaatsen;

                // Bepaal welke klant IDs verwijderd moeten worden omdat ze niet langer in het ViewModel staan
                var removedKlantIDs = job.KlantJobs
                   .Where(existingKlantJob => !viewModel.KlantJobs.Any(newKlantJob => newKlantJob.KlantId == existingKlantJob.KlantId))
                   .Select(existingKlantJob => existingKlantJob.KlantId)
                   .ToList();

                // Een set om duplicaten te detecteren
                var existingKlantIds = new HashSet<string>();

                // Loop door alle klantjobs in het ViewModel
                foreach (var viewModelKlantJob in viewModel.KlantJobs)
                {
                    // Controleer op duplicaten
                    if (existingKlantIds.Contains(viewModelKlantJob.KlantId))
                    {
                        // Voeg een model error toe die weergegeven wordt in de view
                        ModelState.AddModelError(string.Empty, "Er zijn duplicaten in de klantjobs.");

                        // Vul het ViewModel opnieuw met de bestaande klantjobs uit de Job
                        viewModel.KlantJobs = job.KlantJobs.Select(kj => new KlantJobListViewModel
                        {
                            KlantId = kj.KlantId,
                        }).ToList();

                        // Vul de lijst van alle klanten opnieuw voor de dropdowns
                        var klanten = await _context.KlantRepository.GetAllAsync();
                        viewModel.Klanten = klanten.Select(k => new SelectListItem
                        {
                            Value = k.Id.ToString(),
                            Text = k.Naam + " " + k.Voornaam
                        }).ToList();

                        // Return naar de view met foutmelding en ViewModel
                        return View(viewModel);
                    }

                    // Voeg de klantId toe aan de set om duplicaten verderop te voorkomen
                    existingKlantIds.Add(viewModelKlantJob.KlantId);

                    // Zoek of deze klant al aan de Job gekoppeld is
                    var klantjob = job.KlantJobs
                        .FirstOrDefault(kj => kj.KlantId == viewModelKlantJob.KlantId);

                    if (klantjob != null)
                    {
                        // Als de klant al bestaat, update eventueel properties (hier alleen KlantId, voor volledigheid)
                        klantjob.KlantId = viewModelKlantJob.KlantId;
                    }
                    else
                    {
                        // Anders maak een nieuwe KlantJob aan en voeg toe aan de Job
                        KlantJob klantjobnieuw = new()
                        {
                            JobId = job.Id,
                            KlantId = viewModelKlantJob.KlantId
                        };
                        job.KlantJobs.Add(klantjobnieuw);
                    }
                }

                // Verwijder klantJobs die niet langer aanwezig zijn in het ViewModel
                foreach (var removedKlantID in removedKlantIDs)
                {
                    var klantjobToRemove = job.KlantJobs.FirstOrDefault(kj => kj.KlantId == removedKlantID);
                    if (klantjobToRemove != null)
                    {
                        _context.KlantJobRepository.Delete(klantjobToRemove);
                    }
                }

                try
                {
                    // Update de Job in de repository en sla de wijzigingen op in de database
                    _context.JobRepository.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Als er een concurrency issue is (bijv. iemand anders heeft de Job ondertussen aangepast)
                    if (await _context.JobRepository.GetByIdAsync(id) != null)
                    {
                        return NotFound(); // Geef een 404 als de Job ineens verdwenen is
                    }
                    else
                    {
                        throw; // Anders gooi de uitzondering verder
                    }
                }

                // Redirect naar de Index van de Job controller als alles gelukt is
                return RedirectToAction(nameof(Index));
            }

            // Als ModelState invalid is, terug naar de view met het ViewModel zodat de gebruiker fouten kan corrigeren
            return View(viewModel);
        }


        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.JobRepository.GetByIdAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.JobRepository.GetByIdAsync(id);
            if (job != null)
            {
                _context.JobRepository.Delete(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
