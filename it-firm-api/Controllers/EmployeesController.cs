
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using it_firm_api.Domain.Entities;
using it_firm_api.Persistence;
using it_firm_api.Models;
using Mapster;
using BCrypt.Net;

namespace it_firm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees()
        {
            return await _context.Employees.ProjectToType<EmployeeResponseDto>().ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee.Adapt<EmployeeResponseDto>();
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, EmployeeRequestDto employeeDto)
        {

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employeeDto.Password != null)
            {
                employee.HashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password, SaltRevision.Revision2A);
            }
            employee.FistName = employeeDto.FistName ?? employee.FistName;
            employee.LastName = employeeDto.LastName ?? employee.LastName;
            employee.Email = employeeDto.Email ?? employee.Email;
           
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> PostEmployee(EmployeeRequestDto employeeDto)
        {
            var employee = employeeDto.Adapt<Employee>();
            employee.HashedPassword = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password, SaltRevision.Revision2A);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee.Adapt<EmployeeResponseDto>());
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
