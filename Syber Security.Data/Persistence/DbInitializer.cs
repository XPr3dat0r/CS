using SyberSecurity.Domain.Entities;
using SyberSecurity.Interfaces;
using SyberSecurity.Domain.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SyberSecurity.Data.Persistence
{
#pragma warning disable
    public class DbInitializer : IDbInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationDbContext> _logger;
        private IConfiguration _configuration { get; }

        public DbInitializer(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            ILogger<ApplicationDbContext> logger
        )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _logger = logger;  
        }

        public async void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                    _context.Database.Migrate();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
            }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(Roles.RoleType.SuperAdmin.ToString()).GetAwaiter().GetResult())
            {

                _roleManager.CreateAsync(new IdentityRole(Roles.RoleType.SuperAdmin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.RoleType.Admin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.RoleType.Employee.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.RoleType.Individual.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Roles.RoleType.Company.ToString())).GetAwaiter().GetResult();


                //Create superadmin user.
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = _configuration["UserSettings:SuperAdmin:UserName"],
                    Email = _configuration["UserSettings:SuperAdmin:UserName"],
                    FirstName = "Pratik",
                    LastName = "Pratik",
                    PhoneNumber = "+447393064680",
                    StreetAddress = "UK",
                    State = "UK",
                    PostalCode = "4111",
                    City = "UK"
                }, _configuration["UserSettings:SuperAdmin:Password"]).GetAwaiter().GetResult();

                ApplicationUser superAdmin = _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == _configuration["UserSettings:SuperAdmin:UserName"]).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(superAdmin, Roles.RoleType.SuperAdmin.ToString()).GetAwaiter().GetResult();

                //Create admin user.
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = _configuration["UserSettings:Admin:UserName"],
                    Email = _configuration["UserSettings:Admin:UserName"],
                    FirstName = "Test",
                    LastName = "Test",
                    PhoneNumber = "+201032040649",
                    StreetAddress = "Falkgatan 44C",
                    State = "Sverige",
                    PostalCode = "65421",
                    City = "Motala"
                }, _configuration["UserSettings:Admin:Password"]).GetAwaiter().GetResult();

                ApplicationUser admin = _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == _configuration["UserSettings:Admin:UserName"]).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(admin, Roles.RoleType.Admin.ToString()).GetAwaiter().GetResult();

            }

            

            // Create categories
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(new List<Category>()
                    {
                        new Category(){Name = "Hardware", DisplayOrder = 2, CreatedDateTime = DateTime.Parse("2022-01-15")},
                        new Category(){Name = "Software", DisplayOrder = 2, CreatedDateTime = DateTime.Parse("2022-01-15")},
                        new Category(){Name = "rare", DisplayOrder = 2, CreatedDateTime = DateTime.Parse("2022-01-16")},
                        
                    });
                _context.SaveChanges();
            }

          

            // Create products
            if (!_context.Products.Any())
            {
                _context.Products.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Title = "WIFI PINEAPPLE",
                            Description = "<p>The industry standard WiFi pentest platform has evolved. Equip your red team with the WiFi Pineapple® Mark VII. Newly refined. Enterprise ready.\n\nBasic edition includes antennas and USB-C power/ethernet cable.</p>",
                            Price = 119,
                            ImageUrl = "\\images\\products\\WIFI PINEAPPLE.webp",
                            CategoryId = 1,
                            InStock = 10
                        },
                        new Product()
                        {
                            Title = "WIFI COCONUT",
                            Description = "<p> There are 14 channels on the 2.4 GHz WiFi spectrum. Why packet sniff with only one radio?\n\nChannel hopping misses 93% of the airspace at any given time.\n\nWhat if you could monitor all channels at once, from a single USB-C device?\n\nNow you can. Introducing WiFi Coconut: an Open source full-spectrum WiFi sniffer that simultaneously monitors the entire 2.4 GHz airspace.\n\nWiFi Coconut captures standard PCAP files with its 14 finely tuned 802.11 WiFi radios, and integrates with popular tools like Kismet & Wireshark.</p>",
                            Price = 100,
                            ImageUrl = "\\images\\products\\WIFI COCONUT.webp",
                            CategoryId = 1,
                            InStock = 4
                        },
                        new Product()
                        {
                            Title = "MK7AC WIFI ADAPTER",
                            Description = "<p>Add dual-band 802.11ac monitor and injection capabilities to the WiFi Pineapple Mark VII with the MK7AC module.\n\nThe MK7AC is an 802.11ac Wifi adapter compatible with the WiFi Pineapple Mark VII and many Linux pentest tools for broad spectrum WiFi monitoring and auditing.\n\nStandards: IEEE 802.11 (WiFi 5) a/b/g/n/ac\nChipset: MediaTek MT7612U\nWiFi Frequency:L 2.4 GHz, 5 GHz\nData Throughput: 866 Mbit/s\nInterface: USB 3.0\nDimensions: 33 x 44 x 20 mm\nAntennas: 2x High Gain RP-SMA\nIncludes: USB-C to USB-A 3.0 adapter</p>",
                            Price = 125,
                            ImageUrl = "\\images\\products\\MK7AC WIFI ADAPTER.webp",
                            CategoryId = 1,
                            InStock = 13
                        },new Product()
                        {
                            Title = "WIFI PINEAPPLE",
                            Description = "<p>The industry standard WiFi pentest platform has evolved. Equip your red team with the WiFi Pineapple® Mark VII. Newly refined. Enterprise ready.\n\nBasic edition includes antennas and USB-C power/ethernet cable.</p>",
                            Price = 119,
                            ImageUrl = "\\images\\products\\WIFI PINEAPPLE.webp",
                            CategoryId = 1,
                            InStock = 10
                        },
                        new Product()
                        {
                            Title = "WIFI COCONUT",
                            Description = "<p> There are 14 channels on the 2.4 GHz WiFi spectrum. Why packet sniff with only one radio?\n\nChannel hopping misses 93% of the airspace at any given time.\n\nWhat if you could monitor all channels at once, from a single USB-C device?\n\nNow you can. Introducing WiFi Coconut: an Open source full-spectrum WiFi sniffer that simultaneously monitors the entire 2.4 GHz airspace.\n\nWiFi Coconut captures standard PCAP files with its 14 finely tuned 802.11 WiFi radios, and integrates with popular tools like Kismet & Wireshark.</p>",
                            Price = 100,
                            ImageUrl = "\\images\\products\\WIFI COCONUT.webp",
                            CategoryId = 1,
                            InStock = 4
                        },
                        new Product()
                        {
                            Title = "MK7AC WIFI ADAPTER",
                            Description = "<p>Add dual-band 802.11ac monitor and injection capabilities to the WiFi Pineapple Mark VII with the MK7AC module.\n\nThe MK7AC is an 802.11ac Wifi adapter compatible with the WiFi Pineapple Mark VII and many Linux pentest tools for broad spectrum WiFi monitoring and auditing.\n\nStandards: IEEE 802.11 (WiFi 5) a/b/g/n/ac\nChipset: MediaTek MT7612U\nWiFi Frequency:L 2.4 GHz, 5 GHz\nData Throughput: 866 Mbit/s\nInterface: USB 3.0\nDimensions: 33 x 44 x 20 mm\nAntennas: 2x High Gain RP-SMA\nIncludes: USB-C to USB-A 3.0 adapter</p>",
                            Price = 125,
                            ImageUrl = "\\images\\products\\MK7AC WIFI ADAPTER.webp",
                            CategoryId = 1,
                            InStock = 13
                        }
                    });
                _context.SaveChanges();
            }
            return;
        }
    }
}
