using CyberCareServices.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CyberCareServices.Data
{
    public class DbInitializer
    {
        public static void Initialize(CyberCareServicesContext db)
        {
            // проверка существования базы данных
            db.Database.EnsureCreated();

            if (db.ComponentTypes.Any())
            {
                db.ComponentTypes.RemoveRange(db.ComponentTypes);
                db.SaveChanges();
            }
            if (db.Customers.Any())
            {
                db.Customers.RemoveRange(db.Customers);
                db.SaveChanges();
            }
            if (db.Employees.Any())
            {
                db.Employees.RemoveRange(db.Employees);
                db.SaveChanges();
            }
            if (db.Services.Any())
            {
                db.Services.RemoveRange(db.Services);
                db.SaveChanges();
            }
            if (db.Components.Any())
            {
                db.Components.RemoveRange(db.Components);
                db.SaveChanges();
            }
            if (db.Orders.Any())
            {
                db.Orders.RemoveRange(db.Orders);
                db.SaveChanges();
            }

            ResetIdentitySeed(db, "ComponentTypes");
            ResetIdentitySeed(db, "Customers");
            ResetIdentitySeed(db, "Employees");
            ResetIdentitySeed(db, "Services");
            ResetIdentitySeed(db, "Components");
            ResetIdentitySeed(db, "Orders");

            ResetIdentitySeed(db, "Orders");
            ResetIdentitySeed(db, "Orders");

            // проверка инициализированны ли таблицы
            if (db.Orders.Any()) { return; }

            // quontity
            int compQ = 250;
            int cstmQ = 100;
            int emplQ = 20;
            int srvcQ = 20;
            int cmpTQ = 27;
            int ordrQ = 250;

            Random random = new Random();

            #region Component Types Initialization
            string[] componentTypes = {
                "CPU (Central Processing Unit)",           // The main processor of the system
                "GPU (Graphics Processing Unit)",          // The graphics card for rendering images
                "Motherboard",                              // The main circuit board that connects all components
                "RAM (Random Access Memory)",               // Memory used for temporary data storage
                "Storage Drive (HDD/SSD)",                  // Hard disk drives or solid-state drives for data storage
                "Power Supply Unit (PSU)",                  // Supplies power to the system
                "Cooling System",                            // Fans or liquid cooling systems to dissipate heat
                "Case/Chassis",                             // The enclosure that houses all components
                "Optical Drive",                            // CD/DVD/Blu-ray drives (less common in modern systems)
                "Network Interface Card (NIC)",             // For wired or wireless network connectivity
                "Sound Card",                               // For audio processing (often integrated into the motherboard)
                "Input Devices",                            // Keyboards, mice, controllers
                "Output Devices",                           // Monitors, TVs, speakers
                "Game Controller",                          // Specific to consoles for gameplay
                "Capture Card",                             // For recording or streaming gameplay
                "Cooling Pads",                             // For laptops or consoles to enhance cooling
                "Expansion Cards",                          // Additional cards for extra functionality (e.g., USB, Thunderbolt)
                "Webcam",                                   // For video streaming or conferencing
                "Microphone",                               // For voice chat or recording
                "VR Headset",                               // Virtual reality headsets for immersive gaming
                "Power Cable",                              // Cables for power supply connections
                "Data Cables",                              // SATA, USB, HDMI, DisplayPort cables, etc.
                "Storage Enclosure",                        // External enclosures for HDDs/SSDs
                "Game Cartridge/Disc",                      // Physical media for games (specific to consoles)
                "Cooling Fan",                              // Specific fans for additional cooling
                "Thermal Paste",                           // For heat transfer between CPU/GPU and their coolers
                "Game Console Dock",                        // For docking handheld consoles (like the Nintendo Switch)
            };
            for (int id = 1; id <= cmpTQ; id++)
            {
                db.ComponentTypes.Add(new ComponentType
                {
                    //ComponentTypeId = id,
                    Name = componentTypes[id - 1],
                    Description = "Description for " + componentTypes[id - 1],
                });
            }

            #endregion

            #region Customer Initialization
            for (int id = 1; id <= cstmQ; id++)
            {
                var discountAvailable = random.Next(0, 2) == 0;
                db.Customers.Add(new Customer
                {
                    FullName = "Name of custoer #" + id.ToString(),
                    Address = "Address of cusstomer #" + id.ToString(),
                    Phone = "+375" + random.Next(100000000, 999999999),
                    DiscountAvailable = discountAvailable,
                    DiscountAmount = discountAvailable ? 0 : random.Next(1, 21),
                });
            }
            #endregion

            #region Employees Initialization
            string[] employeePositions = {
                "Service Technician",
                "Customer Support Representative",
                "Field Service Engineer",
                "Technical Support Specialist",
                "IT Support Technician",
                "Network Administrator",
                "System Administrator",
                "Help Desk Technician",
                "Repair Technician",
                "Quality Assurance Specialist",
                "Sales Representative",
                "Store Manager",
                "Inventory Specialist",
                "Software Developer",
                "Project Manager",
                "Data Analyst",
                "Cybersecurity Specialist",
                "Hardware Engineer",
                "Technical Trainer",
                "Procurement Specialist"
            };
            for (int id = 1; id <= emplQ; id++)
            {
                db.Employees.Add(new Employee
                {
                    FullName = "Name of Employee #" + id.ToString(),
                    Position = employeePositions[random.Next(employeePositions.Length)],
                    DateOfHire = new DateOnly(random.Next(2020, 2025),
                                                    random.Next(1, 13),
                                                    1),
                    DateOfBirth = new DateOnly(random.Next(1980, 2005),
                                                    random.Next(1, 13),
                                                    1),
                    Phone = "+375" + random.Next(100000000, 999999999),
                    Email = "mailOfEmployee#" + id.ToString() + "@gmail.com",
                });
            }
            #endregion

            #region Services Initialization
            string[] commonServices = {
                "Virus and Malware Removal",
                "Computer Repair and Maintenance",
                "Data Recovery Services",
                "Hardware Upgrades",
                "Software Installation and Configuration",
                "Network Setup and Configuration",
                "Operating System Installation",
                "Troubleshooting and Diagnostics",
                "Remote Technical Support",
                "On-Site Technical Support",
                "Custom PC Building",
                "Backup Solutions and Data Management",
                "Printer Setup and Repair",
                "Website Development and Hosting",
                "Cloud Services and Solutions",
                "IT Consulting and Strategy",
                "Computer Cleaning and Optimization",
                "Training and Workshops",
                "Security Audits and Vulnerability Assessments",
                "Mobile Device Repair"
            };
            for (int id = 1; id <= srvcQ; id++)
            {
                db.Services.Add(new Service
                {
                    Name = commonServices[id - 1],
                    Description = commonServices[id - 1],
                    Cost = random.Next(10, 301),
                });
            }
            #endregion

            db.SaveChanges();

            #region Components Initialization

            string[] brands = {
                "Intel",
                "AMD",
                "NVIDIA",
                "Asus",
                "MSI",
                "Gigabyte",
                "Corsair",
                "Seagate",
                "Western Digital",
                "Samsung"
            };

            string[] manufacturers = {
                "Intel Corporation",
                "Advanced Micro Devices, Inc.",
                "NVIDIA Corporation",
                "ASUSTeK Computer Inc.",
                "Micro-Star International Co., Ltd.",
                "Gigabyte Technology Co., Ltd.",
                "Corsair Components, Inc.",
                "Seagate Technology Holdings PLC",
                "Western Digital Corporation",
                "Samsung Electronics Co., Ltd."
            };

            string[] countriesOfOrigin = {
                "United States",
                "United States",
                "United States",
                "Taiwan",
                "Taiwan",
                "Taiwan",
                "United States",
                "United States",
                "United States",
                "South Korea"
            };

            for (int id = 1; id <= compQ; id++)
            {
                int bmcId = random.Next(brands.Length);
                int ctID = random.Next(1, cmpTQ + 1);
                var component = new Model.Component
                {
                    //ComponentId = id,
                    ComponentTypeId = ctID,
                    Brand = brands[bmcId],
                    Manufacturer = manufacturers[bmcId],
                    CountryOfOrigin = countriesOfOrigin[bmcId],
                    ReleaseDate = new DateOnly(random.Next(2000, 2025),
                                                    random.Next(1, 13),
                                                    1),
                    Specifications = "Specifications for component #" + id.ToString(),
                    WarrantyPeriod = random.Next(1, 5),
                    Description = "Description for component #" + id.ToString(),
                    Price = random.Next(100, 5000),
                    ComponentType = db.ComponentTypes.FirstOrDefault(ct => ct.ComponentTypeId == ctID),
                };
                db.Components.Add(component);

                db.ComponentTypes.FirstOrDefault(ct => ct.ComponentTypeId == ctID).Components.Add(component);
            }
            #endregion

            db.SaveChanges();

            #region Orders Initialization
            for (int id = 1; id <= ordrQ; id++)
            {
                var od = new DateOnly(random.Next(2020, 2025),
                                                    random.Next(1, 13),
                                                    1);
                var tc = random.Next(10, 301);
                int cId = random.Next(1, cstmQ + 1);
                int eId = random.Next(1, emplQ + 1);
                Tuple<int, int, int> components = new Tuple<int, int, int>(random.Next(1, compQ + 1), random.Next(1, compQ + 1), random.Next(1, compQ + 1));
                Tuple<int, int, int> services = new Tuple<int, int, int>(random.Next(1, srvcQ + 1), random.Next(1, srvcQ + 1), random.Next(1, srvcQ + 1));
                var order = new Order
                {
                    OrderDate = od,
                    CompletionDate = new DateOnly(od.Year, od.Month, od.Day + random.Next(1, 15)),
                    CustomerId = cId,
                    Prepayment = tc / random.Next(2, 10),
                    PaymentStatus = random.Next(0, 2) == 0,
                    CompletionStatus = random.Next(0, 2) == 0,
                    TotalCost = tc,
                    WarrantyPeriod = random.Next(1, 25),
                    EmployeeId = eId,
                    Customer = db.Customers.FirstOrDefault(c => c.CustomerId == cId),
                    Employee = db.Employees.FirstOrDefault(e => e.EmployeeId == eId),
                    Components = new List<Component>
                    {
                        db.Components.FirstOrDefault(c => c.ComponentId == components.Item1),
                        db.Components.FirstOrDefault(c => c.ComponentId == components.Item2),
                        db.Components.FirstOrDefault(c => c.ComponentId == components.Item3),
                    },
                    Services = new List<Service>
                    {
                        db.Services.FirstOrDefault(s => s.ServiceId == services.Item1),
                        db.Services.FirstOrDefault(s => s.ServiceId == services.Item2),
                        db.Services.FirstOrDefault(s => s.ServiceId == services.Item3),
                    },
                };
                db.Orders.Add(order);

                db.Components.FirstOrDefault(c => c.ComponentId == components.Item1).Orders.Add(order);
                db.Components.FirstOrDefault(c => c.ComponentId == components.Item2).Orders.Add(order);
                db.Components.FirstOrDefault(c => c.ComponentId == components.Item3).Orders.Add(order);

                db.Services.FirstOrDefault(s => s.ServiceId == services.Item1).Orders.Add(order);
                db.Services.FirstOrDefault(s => s.ServiceId == services.Item2).Orders.Add(order);
                db.Services.FirstOrDefault(s => s.ServiceId == services.Item3).Orders.Add(order);

                db.Customers.FirstOrDefault(c => c.CustomerId == cId).Orders.Add(order);
                db.Employees.FirstOrDefault(e => e.EmployeeId == eId).Orders.Add(order);
            }
            #endregion

            db.SaveChanges();

        }

        private static void ResetIdentitySeed(CyberCareServicesContext db, string tableName)
        {
            // Execute the SQL command to reset the identity seed
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('{tableName}', RESEED, 0);");
        }
    }
}
