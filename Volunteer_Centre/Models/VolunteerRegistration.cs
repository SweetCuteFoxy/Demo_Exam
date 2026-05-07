namespace VolunteerApp.Models
{
    public class VolunteerRegistration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int VolunteerId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int StatusId { get; set; }
        public VolunteerEvent? Event { get; set; }
        public User? Volunteer { get; set; }
        public RegistrationStatus? Status { get; set; }
        public Attendance? Attendance { get; set; }
    }
}
