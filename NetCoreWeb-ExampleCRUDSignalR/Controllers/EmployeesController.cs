using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetCoreWeb_ExampleCRUDSignalR.Data;
using NetCoreWeb_ExampleCRUDSignalR.Hubs;
using NetCoreWeb_ExampleCRUDSignalR.Models;

namespace NetCoreWeb_ExampleCRUDSignalR.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        private readonly EmailSender.EmailSender _emailSender;

        public EmployeesController(ApplicationDbContext context, IHubContext<BroadcastHub, IHubClient> hubContext, EmailSender.EmailConfiguration emailConfig)
        {
            _context = context;
            _hubContext = hubContext;
            _emailSender = new EmailSender.EmailSender(emailConfig);
        }

        public async Task<IActionResult> GetAll()
        {
            return PartialView("_GetAllEmployees", await GetEmployeesAsync());
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await GetEmployeesAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.BroadcastMessage();
                try
                {
                    var message = new EmailSender.Message(new string[] { "Pedro.MartinezCastro@tds.fujitsu.com" }, "NetCore Application", $"The Employee ${employee.FirstName} was added.");
                    _emailSender.SendEmail(message);
                }
                catch (Exception ex)
                {
                    int x = 12;
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Employee employee)
        {
            if (id != employee.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.BroadcastMessage();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.BroadcastMessage();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        private async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.OrderBy(x => x.FirstName).ToListAsync();
        }
    }
}
