namespace VolunteerApp.Models
{
    public class EventStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<VolunteerEvent> Events { get; set; } = new();
    }
}
