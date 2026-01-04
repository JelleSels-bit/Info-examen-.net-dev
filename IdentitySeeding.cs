
using Microsoft.AspNetCore.Identity;
using System.Data.Common;


namespace Interimkantoor
{
    public class IdentitySeeding
    {
        public async Task IdentitySeedingAsync(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager) {
            try
            {
                // Gebruiker aanmaken
                // Admin bestaat nog niet?
                if (userManager.FindByNameAsync("Admin").Result == null)
                {
                    // Gebruiker voorzien
                    var defaultUser = new CustomUser
                    {
                        UserName = "Admin",
                        Achternaam=" De Beheerder",
                        Voornaam="Admin",
                        Geboortedatum=DateTime.Now,
                        Email = "superteam@thomasmore.be",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                    };

                    // Gebruiker aanmaken
                    await userManager.CreateAsync(defaultUser, "t0LTHxzy.v");

                    // Rollen seeden
                    string[] roles = ["Beheerder", "Gebruiker"];

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                     await userManager.AddToRoleAsync(defaultUser, "Beheerder");
                }
            } 
             catch (DbException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}