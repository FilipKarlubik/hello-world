namespace Eucyon_Tribes.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string VerificationToken { get; set; } = null!;
        public DateTime VerificationTokenExpiresAt { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ForgottenPasswordToken { get; set; } = null!;
        public DateTime ForgottenPasswordTokenExpiresAt { get; set; }

        public Kingdom Kingdom { get; set; } = null!;

        public User()
        {
            VerificationTokenExpiresAt = DateTime.Now.AddDays(5);
            CreatedDate = DateTime.Now;
        }
    }
}