using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Interimkantoor.Data;
using Interimkantoor.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Interimkantoor.Controllers
{
    [Authorize(Roles = "Beheerder,Klant")]
    public class KlantController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public KlantController(IUnitOfWork context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        
        
        public async Task<IActionResult> Assign(int Id)
        {
            var job = await _context.JobRepository.GetByIdAsync(Id);
            var unMappedKlanten = await _context.KlantRepository.GetAllAsync();

            if (job == null)
                return NotFound();

            var vm = new AssignKlantToJobViewModel
            {
                JobId = Id,
                JobOmschrijving = job.Omschrijving,
                Klanten = unMappedKlanten
                .Select(klant => new SelectListItem
                {
                    Value = klant.Id,
                    Text = klant.Naam
                })
                .ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(AssignKlantToJobViewModel vm)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var result = await _context.KlantJobRepository.Find(x => x.JobId == vm.JobId && x.KlantId == vm.KlantId);

            if(result.Count == 0)
            {
                var assignKlant = new KlantJob
                {
                    JobId = vm.JobId,
                    KlantId = vm.KlantId,
                };

                await _context.KlantJobRepository.AddAsync(assignKlant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }else
            {
                ModelState.AddModelError("", "Deze klant is al toegewezen aan deze job.");
                
                // dropdown opnieuw vullen
                vm.Klanten = (await _context.KlantRepository.GetAllAsync())
                    .Select(k => new SelectListItem
                    {
                        Value = k.Id.ToString(),
                        Text = k.Naam
                    }).ToList();

                var job = await _context.JobRepository.GetByIdAsync(vm.JobId);
                vm.JobOmschrijving = job.Omschrijving;

                return View(vm);
            }

            
        }
        
        
        
        // GET: Klant
        public async Task<IActionResult> Index()
        {
            return View(await _context.KlantRepository.GetAllAsync());
        }

        // GET: Klant/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klant = await _context.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound();
            }

            return View(klant);
        }

        // GET: Klant/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Klant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KlantCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Er zit een fout in het model");

            Klant klant = _mapper.Map<Klant>(vm);
            await _context.KlantRepository.AddAsync(klant);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Er is een probleem opgetreden bij het wegschrijven naar de database.");
                return View(vm);
            }
        }

        // GET: Klant/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klant = await _context.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound();
            }
            return View(klant);
        }

        // POST: Klant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KlantEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound("De id's komen niet overeen");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Klant klant = _mapper.Map<Klant>(vm);
                _context.KlantRepository.Update(klant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } 
            catch (DbUpdateConcurrencyException)
            {
                if (_context.KlantRepository.GetByIdAsync(id) != null)
                {
                    ModelState.AddModelError("", "Er is een probleem opgetreden bij het wegschrijven naar de database.");
                    return View(vm);
                }
            }

            return View(vm);
                       
        }

        // GET: Klant/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound("Er is geen id meegeven?");
            }

            var klant = await _context.KlantRepository.GetByIdAsync(id);

            if (klant == null)
            {
                return NotFound("de Klant met dit id kan niet worden terug gevonden");
            }

            return View(klant);
        }

        // POST: Klant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var klant = await _context.KlantRepository.GetByIdAsync(id);
            if (klant == null)
            {
                return NotFound("de Klant met dit id kan niet worden terug gevonden");
            }

            _context.KlantRepository.Delete(klant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
