using System.Threading.Tasks;
using FirstAPI.Models;
using FirstAPI.Contexts;
using FirstAPI.Repositories;
using FirstAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FirstAPI.Test;

public class Tests
{
    private ClinicContext _context;
    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ClinicContext>().UseInMemoryDatabase("TestDb").Options;

        _context = new ClinicContext(options);
    }

    // [Test]
    // public async Task Test1()
    // {
    //     //arrange
    //     var email = "test@gmail.com";
    //     var password = System.Text.Encoding.UTF8.GetBytes("test123");
    //     var key = Guid.NewGuid().ToByteArray();

    //     var user = new User
    //     {
    //         Username = email,
    //         Password = password,
    //         HashKey = key,
    //         Role = "Doctor"
    //     };

    //     _context.Add(user);

    //     await _context.SaveChangesAsync();
    //     var doctor = new Doctor
    //     {
    //         Name = "test",
    //         YearsOfExperience = 2,
    //         Email = email
    //     };
    //     IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
    //     //action
    //     var result = await _doctorRepository.Add(doctor);
    //     //assert
    //     Assert.That(result, Is.Not.Null, "Doctor IS not added");
    //     Assert.That(result.Id, Is.EqualTo(2));

    // }
    [TestCase(1)]
    public async Task GetDoctorPassTest(int id)
    {
        //arrange
        var email = "test@gmail.com";
        var password = System.Text.Encoding.UTF8.GetBytes("test123");
        var key = Guid.NewGuid().ToByteArray();

        var user = new User
        {
            Username = email,
            Password = password,
            HashKey = key,
            Role = "Doctor"
        };

        _context.Add(user);

        await _context.SaveChangesAsync();
        var doctor = new Doctor
        {
            Name = "test",
            YearsOfExperience = 2,
            Email = email
        };
        IRepository<int, Doctor> _doctorRepository = new DoctorRepository(_context);
        //action
        await _doctorRepository.Add(doctor);

        var result = _doctorRepository.Get(id);

        Assert.That(result.Id, Is.EqualTo(1));
        
    }
    [TearDown]
    public void TearDown() 
    {
        _context.Dispose();
    }
}
