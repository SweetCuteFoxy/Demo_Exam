namespace VolunteerApp.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<VolunteerSkill> VolunteerSkills { get; set; } = new();
    }
}
