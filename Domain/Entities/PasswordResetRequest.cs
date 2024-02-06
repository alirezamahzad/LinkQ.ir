
namespace Domain.Entities
{
    public class PasswordResetRequest
    {   
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ResetCode { get; private set; }
        public DateTime? Expiration { get; private set; }

        public PasswordResetRequest(int userId,int resetCode, DateTime? expiration)
        {
            UserId = userId;
            ResetCode = resetCode;
            Expiration = expiration;
        }

        public bool IsExpired() => Expiration.HasValue && Expiration.Value < DateTime.UtcNow;
    }
}
