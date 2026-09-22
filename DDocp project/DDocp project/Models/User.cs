using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace DDocp_project
{
    internal class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public double GoalCalories { get; set; } = 0;
        public List<ActivityRecord> ActivityRecords { get; set; } = new List<ActivityRecord>();

        public void SetPassword(string password)
        {
            PasswordHash = ComputeHash(password);
        }

        public bool VerifyPassword(string password)
        {
            var h = ComputeHash(password);
            return string.Equals(h, PasswordHash, StringComparison.Ordinal);
        }

        private static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}
