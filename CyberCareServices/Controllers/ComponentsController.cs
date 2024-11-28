using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using CyberCareServices.Data;
using Microsoft.AspNetCore.Authorization;

namespace CyberCareServices.Controllers
{
    [Authorize]
    public class ComponentsController : Controller
    {
        private readonly CyberCareServicesContext _context;
        private readonly int PageSize = 20;

        public ComponentsController(CyberCareServicesContext context)
        {
            _context = context;
        }

        // GET: Components
        [ResponseCache(Duration = 2 * 5 + 240, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index(int page = 1)
        {
            var components = await _context.Components
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Include(c => c.ComponentType)
                .Select(c => new ComponentViewModel
                {
                    ComponentId = c.ComponentId,
                    ComponentType = c.ComponentType.Name,
                    Brand = c.Brand,
                    Manufacturer = c.Manufacturer,
                    CountryOfOrigin = c.CountryOfOrigin,
                    ReleaseDate = c.ReleaseDate,
                    Specifications = c.Specifications,
                    WarrantyPeriod = c.WarrantyPeriod,
                    Description = c.Description,
                    Price = c.Price
                })
                .ToListAsync();

            var componentsvm = new ComponentsViewModel()
            {
                Components = components,
                PageViewModel = new PageViewModel(_context.Orders.Count(), page, PageSize)
            };
            return View(componentsvm);
        }

        // GET: Components/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var component = await _context.Components
                .Include(c => c.ComponentType)
                .Where(c => c.ComponentId == id)
                .Select(c => new ComponentViewModel
                {
                    ComponentId = c.ComponentId,
                    ComponentType = c.ComponentType.Name,
                    Brand = c.Brand,
                    Manufacturer = c.Manufacturer,
                    CountryOfOrigin = c.CountryOfOrigin,
                    ReleaseDate = c.ReleaseDate,
                    Specifications = c.Specifications,
                    WarrantyPeriod = c.WarrantyPeriod,
                    Description = c.Description,
                    Price = c.Price
                })
                .FirstOrDefaultAsync();

            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        // GET: Components/Create
        public async Task<IActionResult> Create()
        {
            var componentTypes = await _context.ComponentTypes.ToListAsync();

            var editModel = new ComponentEditViewModel
            {
                ComponentTypes = componentTypes
            };

            return View(editModel);
        }

        // POST: Components/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComponentType,Brand,Manufacturer,CountryOfOrigin,ReleaseDate,Specifications,WarrantyPeriod,Description,Price")] ComponentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var componentType = await _context.ComponentTypes
                    .FirstOrDefaultAsync(ct => ct.Name == model.ComponentType);

                if (componentType == null)
                {
                    ModelState.AddModelError("ComponentType", "Неверный тип компонента.");
                    return View(model);
                }

                var component = new Component
                {
                    ComponentTypeId = componentType.ComponentTypeId,
                    Brand = model.Brand,
                    Manufacturer = model.Manufacturer,
                    CountryOfOrigin = model.CountryOfOrigin,
                    ReleaseDate = model.ReleaseDate,
                    Specifications = model.Specifications,
                    WarrantyPeriod = model.WarrantyPeriod,
                    Description = model.Description,
                    Price = model.Price
                };

                _context.Add(component);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Components/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var component = await _context.Components
                .Include(c => c.ComponentType)
                .Select(c => new ComponentEditViewModel
                {
                    ComponentId = c.ComponentId,
                    ComponentType = c.ComponentType.Name,
                    Brand = c.Brand,
                    Manufacturer = c.Manufacturer,
                    CountryOfOrigin = c.CountryOfOrigin,
                    ReleaseDate = c.ReleaseDate,
                    Specifications = c.Specifications,
                    WarrantyPeriod = c.WarrantyPeriod,
                    Description = c.Description,
                    Price = c.Price
                })
                .FirstOrDefaultAsync(c => c.ComponentId == id);

            if (component == null)
            {
                return NotFound();
            }

            component.ComponentTypes = await _context.ComponentTypes.ToListAsync();

            return View(component);
        }



        // POST: Components/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComponentId,ComponentType,Brand,Manufacturer,CountryOfOrigin,ReleaseDate,Specifications,WarrantyPeriod,Description,Price")] ComponentViewModel model)
        {
            if (id != model.ComponentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var component = await _context.Components
                        .Include(c => c.ComponentType)
                        .FirstOrDefaultAsync(c => c.ComponentId == id);

                    if (component == null)
                    {
                        return NotFound();
                    }

                    var componentType = await _context.ComponentTypes
                        .FirstOrDefaultAsync(ct => ct.Name == model.ComponentType);

                    if (componentType == null)
                    {
                        ModelState.AddModelError("ComponentType", "Invalid component type.");
                        return View(model);
                    }

                    // Update the component properties
                    component.ComponentTypeId = componentType.ComponentTypeId;
                    component.Brand = model.Brand;
                    component.Manufacturer = model.Manufacturer;
                    component.CountryOfOrigin = model.CountryOfOrigin;
                    component.ReleaseDate = model.ReleaseDate;
                    component.Specifications = model.Specifications;
                    component.WarrantyPeriod = model.WarrantyPeriod;
                    component.Description = model.Description;
                    component.Price = model.Price;

                    _context.Update(component);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(model.ComponentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Components/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var component = await _context.Components
                .Include(c => c.ComponentType)
                .Where(c => c.ComponentId == id)
                .Select(c => new ComponentViewModel
                {
                    ComponentId = c.ComponentId,
                    ComponentType = c.ComponentType.Name,
                    Brand = c.Brand,
                    Manufacturer = c.Manufacturer,
                    CountryOfOrigin = c.CountryOfOrigin,
                    ReleaseDate = c.ReleaseDate,
                    Specifications = c.Specifications,
                    WarrantyPeriod = c.WarrantyPeriod,
                    Description = c.Description,
                    Price = c.Price
                })
                .FirstOrDefaultAsync();

            if (component == null)
            {
                return NotFound();
            }

            return View(component);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var component = await _context.Components.FindAsync(id);
            if (component != null)
            {
                _context.Components.Remove(component);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentExists(int id)
        {
            return _context.Components.Any(e => e.ComponentId == id);
        }
    }
}
