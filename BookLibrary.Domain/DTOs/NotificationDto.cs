namespace BookLibrary.Domain.DTOs
{
    public class NotificationDto
    {
        public int BookId { get; set; }
        public string UserEmail { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsSent { get; set; }
    }
}
