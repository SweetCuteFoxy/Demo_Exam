namespace VolunteerApp.Models
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<VolunteerEvent> Events { get; set; } = new();
    }
}
