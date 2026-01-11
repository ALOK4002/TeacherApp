using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Data;

public static class SchoolSeedData
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (context.Schools.Any()) return;

        var schools = new List<School>
        {
            new School
            {
                SchoolName = "Rajkiya Ucch Vidyalaya Patna",
                SchoolCode = "BR001",
                District = "Patna",
                Block = "Patna Sadar",
                Village = "Patna City",
                SchoolType = "Senior Secondary",
                ManagementType = "Government",
                TotalStudents = 1200,
                TotalTeachers = 45,
                PrincipalName = "Dr. Rajesh Kumar",
                ContactNumber = "9876543210",
                Email = "principal.patna@education.bihar.gov.in",
                EstablishedDate = new DateTime(1965, 4, 15),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Madhya Vidyalaya Gaya",
                SchoolCode = "BR002",
                District = "Gaya",
                Block = "Gaya Town",
                Village = "Gaya",
                SchoolType = "High",
                ManagementType = "Government",
                TotalStudents = 800,
                TotalTeachers = 32,
                PrincipalName = "Mrs. Sunita Devi",
                ContactNumber = "9876543211",
                Email = "principal.gaya@education.bihar.gov.in",
                EstablishedDate = new DateTime(1970, 8, 20),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Prathamik Vidyalaya Muzaffarpur",
                SchoolCode = "BR003",
                District = "Muzaffarpur",
                Block = "Muzaffarpur East",
                Village = "Muzaffarpur",
                SchoolType = "Primary",
                ManagementType = "Government",
                TotalStudents = 350,
                TotalTeachers = 15,
                PrincipalName = "Mr. Anil Singh",
                ContactNumber = "9876543212",
                Email = "principal.muzaffarpur@education.bihar.gov.in",
                EstablishedDate = new DateTime(1980, 1, 10),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Rajkiya Madhya Vidyalaya Darbhanga",
                SchoolCode = "BR004",
                District = "Darbhanga",
                Block = "Darbhanga Sadar",
                Village = "Darbhanga",
                SchoolType = "Middle",
                ManagementType = "Government",
                TotalStudents = 600,
                TotalTeachers = 25,
                PrincipalName = "Dr. Kavita Sharma",
                ContactNumber = "9876543213",
                Email = "principal.darbhanga@education.bihar.gov.in",
                EstablishedDate = new DateTime(1975, 6, 5),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Ucch Vidyalaya Bhagalpur",
                SchoolCode = "BR005",
                District = "Bhagalpur",
                Block = "Bhagalpur Town",
                Village = "Bhagalpur",
                SchoolType = "Senior Secondary",
                ManagementType = "Government",
                TotalStudents = 1000,
                TotalTeachers = 38,
                PrincipalName = "Mr. Ramesh Prasad",
                ContactNumber = "9876543214",
                Email = "principal.bhagalpur@education.bihar.gov.in",
                EstablishedDate = new DateTime(1968, 3, 12),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Madhya Vidyalaya Purnia",
                SchoolCode = "BR006",
                District = "Purnia",
                Block = "Purnia East",
                Village = "Purnia",
                SchoolType = "High",
                ManagementType = "Government",
                TotalStudents = 750,
                TotalTeachers = 30,
                PrincipalName = "Mrs. Meera Kumari",
                ContactNumber = "9876543215",
                Email = "principal.purnia@education.bihar.gov.in",
                EstablishedDate = new DateTime(1972, 9, 18),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Prathamik Vidyalaya Chapra",
                SchoolCode = "BR007",
                District = "Saran",
                Block = "Chapra",
                Village = "Chapra",
                SchoolType = "Primary",
                ManagementType = "Government",
                TotalStudents = 400,
                TotalTeachers = 18,
                PrincipalName = "Mr. Suresh Kumar",
                ContactNumber = "9876543216",
                Email = "principal.chapra@education.bihar.gov.in",
                EstablishedDate = new DateTime(1978, 11, 25),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Rajkiya Ucch Vidyalaya Begusarai",
                SchoolCode = "BR008",
                District = "Begusarai",
                Block = "Begusarai",
                Village = "Begusarai",
                SchoolType = "Senior Secondary",
                ManagementType = "Government",
                TotalStudents = 900,
                TotalTeachers = 35,
                PrincipalName = "Dr. Priya Singh",
                ContactNumber = "9876543217",
                Email = "principal.begusarai@education.bihar.gov.in",
                EstablishedDate = new DateTime(1969, 7, 8),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Madhya Vidyalaya Katihar",
                SchoolCode = "BR009",
                District = "Katihar",
                Block = "Katihar",
                Village = "Katihar",
                SchoolType = "Middle",
                ManagementType = "Government",
                TotalStudents = 550,
                TotalTeachers = 22,
                PrincipalName = "Mrs. Rekha Devi",
                ContactNumber = "9876543218",
                Email = "principal.katihar@education.bihar.gov.in",
                EstablishedDate = new DateTime(1976, 2, 14),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            },
            new School
            {
                SchoolName = "Ucch Vidyalaya Arrah",
                SchoolCode = "BR010",
                District = "Bhojpur",
                Block = "Arrah",
                Village = "Arrah",
                SchoolType = "High",
                ManagementType = "Government",
                TotalStudents = 850,
                TotalTeachers = 33,
                PrincipalName = "Mr. Vinod Kumar",
                ContactNumber = "9876543219",
                Email = "principal.arrah@education.bihar.gov.in",
                EstablishedDate = new DateTime(1971, 5, 30),
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }
        };

        await context.Schools.AddRangeAsync(schools);
        await context.SaveChangesAsync();
    }
}