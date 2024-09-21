namespace BookLibrary.Domain.DTOs
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string UserEmail { get; set; }
        public DateTime? NotifiedDate { get; set; }
        public bool IsSent { get; set; }
    }
}
