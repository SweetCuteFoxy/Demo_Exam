namespace VolunteerApp.Models
{
    public class VolunteerEvent
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int CategoryId { get; set; }
        public DateTime EventDate { get; set; }
        public int LocationId { get; set; }
        public int VolunteersNeeded { get; set; }
        public int CoordinatorId { get; set; }
        public int StatusId { get; set; }
        public EventCategory? Category { get; set; }
        public Location? Location { get; set; }
        public User? Coordinator { get; set; }
        public EventStatus? Status { get; set; }
        public List<VolunteerRegistration> Registrations { get; set; } = new();
    }
}
