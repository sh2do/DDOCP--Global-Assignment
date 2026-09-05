using System.Text.Json.Serialization;

namespace FitnessTracker
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // stored plainly for simplicity
        public int GoalCalories { get; set; } = 0;
    }
}
