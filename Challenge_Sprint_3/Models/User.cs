namespace Challenge_Sprint_3.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[32];

        public byte[] PasswordSalt { get; set; } = new byte[32];

        public string VerficationToken { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? PasswordResetToken { get; set; } 

        public DateTime? ResetTokenExpires { get; set; }


    }
}
