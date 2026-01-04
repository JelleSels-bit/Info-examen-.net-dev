


using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Interimkantoor.Controllers
{
    [Authorize(Roles = "Beheerder")]
    public class GebruikerController : Controller
    {
        private UserManager<CustomUser> _userManager;
        private SignInManager<CustomUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public GebruikerController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<CustomUser> signInManager, IUnitOfWork context, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(GebruikerLoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);

            if (user != null && !user.EmailConfirmed )
            {
                ModelState.AddModelError("","Email is nog niet bevestigd");
                return View(vm);
            }

            if (!await _userManager.CheckPasswordAsync(user, vm.Password))
            {
                ModelState.AddModelError("","Verkeerde logincombinatie!");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, vm.Password, false, false);

            if (result.IsLockedOut)
                ModelState.AddModelError("","Account geblokeerd!!");

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Ongeldige loginpoging");
            return View(vm);

        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var vm = new GebruikerListViewModel
            {
                Gebruikers = await _userManager.Users.ToListAsync(),    
            };
            

            return View(vm);
        }

        public IActionResult Create()
        {
            var vm = new GebruikerCreateViewModel
            {
                Rollen = new SelectList(_roleManager.Roles, "Id", "Name")
            };

            return View(vm);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GebruikerCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            
            CustomUser existingEmail = await _userManager.FindByEmailAsync(vm.Email);
            
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email al in gebruik");
                return View(vm);
            }

            CustomUser CreateUser = _mapper.Map<CustomUser>(vm);
            CreateUser.EmailConfirmed = true;

            IdentityResult result = await _userManager.CreateAsync(CreateUser, vm.Password);

            if (result.Succeeded)
            {
                CustomUser Getuser = await _userManager.FindByEmailAsync(vm.Email);
                IdentityRole role = await _roleManager.FindByIdAsync(vm.RolId);

                IdentityResult Resultaat = await _userManager.AddToRoleAsync(Getuser, role.Name);
                return RedirectToAction("Index");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
            }

            vm.Rollen = new SelectList(_roleManager.Roles, "Id", "Name");


            return View(vm);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                var vm = _mapper.Map<GebruikerEditViewModel>(user);
                return View(vm);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GebruikerEditViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Er zijn enkele fouten in het formulier. Controleer a.u.b. de velden.");
                return View(vm);
            }

            CustomUser user = await _userManager.FindByIdAsync(vm.GebruikerId);

            _mapper.Map(vm, user);

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }





        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<GebruikerDeleteViewModel>(user);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users.ToList());
        }


    }
}
