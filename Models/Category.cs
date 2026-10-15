namespace RaceDayAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // 

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public ICollection<EventEnrollment> Enrollments { get; set; } = new List<EventEnrollment>();
    }
}
