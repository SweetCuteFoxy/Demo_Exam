namespace VolunteerApp.Models
{
    public class RegistrationStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<VolunteerRegistration> Registrations { get; set; } = new();
    }
}
