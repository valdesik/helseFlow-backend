using HelseFlow_Backend.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HelseFlow_Backend.Infrastructure.Data;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly DataFactory _dataFactory;

    public DataSeeder(AppDbContext context, DataFactory dataFactory)
    {
        _context = context;
        _dataFactory = dataFactory;
    }

    public async Task SeedAsync()
    {
        // Ensure the database is created.
        await _context.Database.MigrateAsync();

        // Seed Users if the table is empty
        if (!_context.Users.Any())
        {
            var users = _dataFactory.CreateTestUsers();
            await _context.Users.AddRangeAsync(users);
        }

        // Seed Doctors if the table is empty
        if (!_context.Doctors.Any())
        {
            var doctors = _dataFactory.CreateTestDoctors();
            await _context.Doctors.AddRangeAsync(doctors);
        }

        // Seed Guidelines if the table is empty
        if (!_context.Guidelines.Any())
        {
            var guidelines = _dataFactory.CreateTestGuidelines();
            await _context.Guidelines.AddRangeAsync(guidelines);
        }

        // Save all changes to the database
        await _context.SaveChangesAsync();
    }
}
