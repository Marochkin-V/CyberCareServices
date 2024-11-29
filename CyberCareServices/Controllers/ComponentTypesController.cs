using CyberCareServices.Data;
using CyberCareServices.Model;
using CyberCareServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyberCareServices.Controllers
{
    [Authorize]
    public class ComponentTypesController : Controller
    {
        private readonly CyberCareServicesContext _context;

        public ComponentTypesController(CyberCareServicesContext context)
        {
            _context = context;
        }

        // GET: ComponentTypes
        [ResponseCache(Duration = 2 * 5 + 240, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Index(string sortField, string sortOrder)
        {
            sortField = string.IsNullOrEmpty(sortField) ? "ComponentTypeId" : sortField;
            sortOrder = string.IsNullOrEmpty(sortOrder) ? "asc" : sortOrder;

            var componentTypesQuery = _context.ComponentTypes
                .Select(ct => new ComponentType
                {
                    ComponentTypeId = ct.ComponentTypeId,
                    Name = ct.Name,
                    Description = ct.Description
                });

            // Сортировка по выбранному полю и направлению
            if (sortOrder == "desc")
            {
                componentTypesQuery = sortField switch
                {
                    "ComponentTypeId" => componentTypesQuery.OrderByDescending(ct => ct.ComponentTypeId),
                    "Name" => componentTypesQuery.OrderByDescending(ct => ct.Name),
                    "Description" => componentTypesQuery.OrderByDescending(ct => ct.Description),
                    _ => componentTypesQuery.OrderByDescending(ct => ct.ComponentTypeId),
                };
            }
            else
            {
                componentTypesQuery = sortField switch
                {
                    "ComponentTypeId" => componentTypesQuery.OrderBy(ct => ct.ComponentTypeId),
                    "Name" => componentTypesQuery.OrderBy(ct => ct.Name),
                    "Description" => componentTypesQuery.OrderBy(ct => ct.Description),
                    _ => componentTypesQuery.OrderBy(ct => ct.ComponentTypeId),
                };
            }

            var componentTypes = await componentTypesQuery.ToListAsync();

            var viewModel = new ComponentTypesViewModel
            {
                ComponentTypes = componentTypes,
                SortField = sortField,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }


        // GET: ComponentTypes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var componentType = await _context.ComponentTypes
                .FirstOrDefaultAsync(m => m.ComponentTypeId == id);
            if (componentType == null)
            {
                return NotFound();
            }

            return View(componentType);
        }

        // GET: ComponentTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComponentTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComponentTypeId,Name,Description")] ComponentType componentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(componentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(componentType);
        }

        // GET: ComponentTypes/Edit/
        public async Task<IActionResult> Edit(int id)
        {
            var componentType = await _context.ComponentTypes.FindAsync(id);
            if (componentType == null)
            {
                return NotFound();
            }
            return View(componentType);
        }

        // POST: ComponentTypes/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComponentTypeId,Name,Description")] ComponentType componentType)
        {
            if (id != componentType.ComponentTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentTypeExists(componentType.ComponentTypeId))
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
            return View(componentType);
        }

        // GET: ComponentTypes/Delete/
        public async Task<IActionResult> Delete(int id)
        {
            var componentType = await _context.ComponentTypes
                .FirstOrDefaultAsync(m => m.ComponentTypeId == id);
            if (componentType == null)
            {
                return NotFound();
            }

            return View(componentType);
        }

        // POST: ComponentTypes/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componentType = await _context.ComponentTypes.FindAsync(id);
            if (componentType != null)
            {
                _context.ComponentTypes.Remove(componentType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }


        private bool ComponentTypeExists(int id)
        {
            return _context.ComponentTypes.Any(e => e.ComponentTypeId == id);
        }
    }
}
