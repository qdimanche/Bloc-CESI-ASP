namespace Bloc_CESI_ASP.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? LandlinePhone { get; set; }
    public string? MobilePhone { get; set; }
    public string? Email { get; set; }
    
    public string? Service { get; set; }
    public string? Site { get; set; }
    
}