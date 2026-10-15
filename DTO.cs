namespace RaceDayAPI.DTOs
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Participant";
    }

    public class UserLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class EventDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public double Distance { get; set; }
        public string EventType { get; set; } = string.Empty;
    }

    public class EnrollmentDto
    {
        public int EventId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ResultDto
    {
        public int EventEnrollmentId { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int FinishingPosition { get; set; }
    }
}