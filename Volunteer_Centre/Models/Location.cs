namespace VolunteerApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Address { get; set; }
        public List<VolunteerEvent> Events { get; set; } = new();
    }
}
