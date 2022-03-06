using Lady_Lollipop.Data.Static;
using Lady_Lollipop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lady_Lollipop.Data
{
    public class ApplicationSeeds
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!context.Sweets.Any())
                {
                    context.Sweets.AddRange(new List<Sweet>()
                    {
                        new Sweet()
                        {
                           Picture="https://onepoundsweets.com/wp-content/uploads/2019/07/dob-bonfire.jpg",
                            Name="Joseph Dobson Bonfire / Treacle Mega Lollies",
                            Price=1.51,
                            Stock=132,
                            Description="Joseph Dobson Bonfire / Treacle Mega Lollies.These classic hard boiled lollies are flavoured with treacle and have a dark black appearance.Classic lollies loved by all on bonfire night and around halloween.You get 4 mega lollies for just £1 which are individually wrapped",
                            Ingridients="Sugar, Glucose Syrup (Sulphates), Treacle, Sunflower Oil, Salt. May also contain milk"
                        },
                        new Sweet()
                        {
                            Picture="https://onepoundsweets.com/wp-content/uploads/2021/04/starbeams-1.jpg",
                            Name="Haribo Starbeams 160g",
                            Price=1.29,
                            Stock=12,
                             Description="NEW Haribo Starbeams. These are a new fruity mixture from Haribo which has stars, lightbulbs and brain shaped sweets inside. Big foamy sweets are full of apple, cherry and lemon flavour.",
                            Ingridients="Sugar; glucose syrup; modified potato starch; potato protein; water; acids: citric acid; malic acid; flavouring; fruit juice from concentrate: apple, lemon, raspberry; fruit and plant concentrates: apple, aronia, beetroot, blackcurrant, elderberry, grape, lemon, orange, radish, safflower, spirulina, sweet potato; caramelised sugar syrup; glazing agents: beeswax, carnauba wax; elderberry extract."
                        },
                        new Sweet()
                        {
                            Picture="https://onepoundsweets.com/wp-content/uploads/2020/11/w-salted.jpg",
                            Name="Walkers Salted Caramel Toffee 150g",
                            Price=1.60,
                            Stock=0,
                             Description="Walkers Salted Caramel Toffee 150g bag. These delicious salted caramel toffees are a real treat any time of the day. Made with fresh sea salt these salted caramel toffees have a super hit of salt and are one of our most popular lines. Big 150g bag and ideal for presents or Father’s Day!",
                            Ingridients="Ingredients: Glucose Syrup, Sugar, Sweetened Condensed Milk (from whole milk) 21%, Vegetable Oil (sustainable palm oil), Butter 4%, Anglesey Sea Salt 1%, Salt, Natural Caramel Flavour, Molasses, Emulsifier (E471), Vanillin.  May contain nut traces. For allergens see ingredients in boldWarnings: May contain nut traces"
                        },
                        new Sweet()
                        {
                            Picture="https://onepoundsweets.com/wp-content/uploads/2019/07/bebeto-trunks-single.jpg",
                            Name="Bebeto Strawberry Trunks 200g",
                            Price=0.78,
                            Stock=12,
                             Description="Strawberry Trunks in a bumper 220g bag. This bag of strawberry trunks contains around 20 trunks and they are around 20cm in length and very thick. Bigger than a normal pencil these are thick and have a creamy centre.Supersize bag which will keep you going for a long time.",
                            Ingridients="Sugar, glucose syrup, wheat flour, modified corn starch, vegetable fat (palm, coconut). apple juice concentrate, invert sugar, acidity regulator (citric acid), flavouring,  beef gelatine (halal), colour (anthocyanins), emulsifier (mono and diglyerides of vegetables fatty acids), humectant (sorbitol)"
                        }
                    });
                    context.SaveChanges();
                }
            }

        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                string adminUserEmail = "admin@lollipop.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }


                string appUserEmail = "user@lollipop.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new ApplicationUser()
                    {
                        FullName = "Application User",
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }

    }
}
