namespace Bloc_CESI_ASP.Models;

public class UpdateEmployeeViewModel
{
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? LandlinePhone { get; set; }
    public string? MobilePhone { get; set; }
    public string? Email { get; set; }
    
    public List<Service>? Services { get; set; }
    public string? SelectedService { get; set; }
    public List<Site>? Sites { get; set; }
}