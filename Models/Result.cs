namespace RaceDayAPI.Models
{
    public class Result
    {
        public int Id { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int FinishingPosition { get; set; }

        public int EventEnrollmentId { get; set; }
        public EventEnrollment? EventEnrollment { get; set; }
    }
}
