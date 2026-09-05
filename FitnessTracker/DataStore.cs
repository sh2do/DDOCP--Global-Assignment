using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FitnessTracker
{
    public static class DataStore
    {
        private static readonly string FilePath = "users.json";
        public static List<User> Users { get; private set; } = new List<User>();

        static DataStore()
        {
            Load();
        }

        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                Users = new List<User>();
                Save();
                return;
            }
            var txt = File.ReadAllText(FilePath);
            try
            {
                Users = JsonSerializer.Deserialize<List<User>>(txt) ?? new List<User>();
            }
            catch
            {
                Users = new List<User>();
            }
        }

        public static void Save()
        {
            var txt = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, txt);
        }

        public static User? Find(string username)
        {
            return Users.Find(u => u.Username == username);
        }
    }
}
