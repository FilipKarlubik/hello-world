namespace Tribes.Tests.SeedData
{
    public class UserSeedData
    {

        public static void PopulateTestData(ApplicationContext _db, string dbFillCode)
        {

            var user1 = new User()
            {
                Name = "Matilda",
                PasswordHash = "m",
                Email = "matilda@gmail.com",
                VerificationToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJmaWxpcC5rYXJsdWJpa0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NjI2NzE3OTN9.2OADR17kq4nLfKNGY8mMZF92LyL205Gh9eV7HykVF-o",
                ForgottenPasswordToken = String.Empty,
                VerificationTokenExpiresAt = DateTime.UtcNow.AddDays(1)

            };

            var user2 = new User()
            {
                Name = "Klotilda",
                PasswordHash = "k",
                Email = "klotilda@gmail.com",
                VerificationToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjExIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJmaWxpcC5rYXJsdWJpa0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NjI2NzE3OTN9.2OADR17kq4nLfKNGY8mMZF92LyL205Gh9eV7HykVF-o",
                ForgottenPasswordToken = String.Empty,
                VerificationTokenExpiresAt = DateTime.UtcNow.AddDays(1)
            };
            user1.Role = "Player";
            user2.Role = "Player";

            try
            {
                int worlds = Int32.Parse(dbFillCode.Substring(dbFillCode.Length - 1));
                if (worlds > 0)
                    for (int i = 0; i < worlds; i++)
                        _db.Worlds.Add(new World() { Name = $"world{i}"});
              
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{dbFillCode}'");
            }

            _db.Users.Add(user1);
            _db.Users.Add(user2);
            _db.SaveChanges();
        }
    }
}