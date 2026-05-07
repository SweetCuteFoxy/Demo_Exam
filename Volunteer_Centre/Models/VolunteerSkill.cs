namespace VolunteerApp.Models
{
    public class VolunteerSkill
    {
        public int Id { get; set; }
        public int VolunteerId { get; set; }
        public int SkillId { get; set; }
        public User? Volunteer { get; set; }
        public Skill? Skill { get; set; }
    }
}
