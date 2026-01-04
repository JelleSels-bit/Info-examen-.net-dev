using Interimkantoor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Interimkantoor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;
            
        public HomeController(ILogger<HomeController> logger, IMapper mapper, IUnitOfWork context)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index(IndexPageViewModel vm)
        {
            var jobs = await _context.JobRepository.GetAllAsync();
            
            foreach (Job job in jobs)
            {
                if(!job.IsAvailable)
                {
                    continue;
                }
                var klantJobs = await _context.KlantJobRepository.GetAllAsync();
                int bezet = klantJobs.Count(kj => kj.JobId == job.Id);

                JobItemViewModel JobItem = _mapper.Map<JobItemViewModel>(job);
                JobItem.VrijePlaatsen = job.AantalPlaatsen - bezet;
                vm.AvailableJobs.Add(JobItem);

            }


            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
