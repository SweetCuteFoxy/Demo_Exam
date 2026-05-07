namespace VolunteerApp.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int RegistrationId { get; set; }
        public bool Attended { get; set; }
        public string? Notes { get; set; }
        public VolunteerRegistration? Registration { get; set; }
    }
}
