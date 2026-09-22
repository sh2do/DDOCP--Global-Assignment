using System.Text.Json;

namespace DDocp_project
{
    internal class DataStore
    {
        private const string FileName = "users.json";
        private const string BackupFileName = "users.json.backup";
        public List<User> Users { get; set; } = new List<User>();

        public static DataStore Load()
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    return new DataStore();
                }

                var json = File.ReadAllText(FileName);

                // Validate JSON is not empty
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new DataStore();
                }

                var ds = JsonSerializer.Deserialize<DataStore>(json);

                // Validate deserialized object
                if (ds == null || ds.Users == null)
                {
                    return new DataStore();
                }

                // Validate users list integrity
                foreach (var user in ds.Users)
                {
                    if (user == null || string.IsNullOrWhiteSpace(user.Username))
                    {
                        return new DataStore(); // Corrupted data, start fresh
                    }
                    if (user.ActivityRecords == null)
                    {
                        user.ActivityRecords = new List<ActivityRecord>();
                    }
                }

                return ds;
            }
            catch (JsonException ex)
            {
                // JSON parse error - attempt recovery from backup
                LogError($"JSON parse error: {ex.Message}. Attempting backup recovery...");
                return TryRecoverFromBackup();
            }
            catch (IOException ex)
            {
                LogError($"IO error reading users.json: {ex.Message}");
                return new DataStore();
            }
            catch (Exception ex)
            {
                LogError($"Unexpected error loading users.json: {ex.Message}");
                return new DataStore();
            }
        }

        public void Save()
        {
            try
            {
                // Create backup before saving
                if (File.Exists(FileName))
                {
                    File.Copy(FileName, BackupFileName, true);
                }

                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

                // Minimal validation
                if (string.IsNullOrEmpty(json))
                {
                    LogError("Serialization produced empty JSON");
                    return;
                }

                File.WriteAllText(FileName, json);
            }
            catch (IOException ex)
            {
                LogError($"IO error saving users.json: {ex.Message}");
            }
            catch (Exception ex)
            {
                LogError($"Unexpected error saving users.json: {ex.Message}");
            }
        }

        private static DataStore TryRecoverFromBackup()
        {
            try
            {
                if (File.Exists(BackupFileName))
                {
                    var backupJson = File.ReadAllText(BackupFileName);
                    var ds = JsonSerializer.Deserialize<DataStore>(backupJson);
                    LogError("Successfully recovered from backup");
                    return ds ?? new DataStore();
                }
            }
            catch (Exception ex)
            {
                LogError($"Failed to recover from backup: {ex.Message}");
            }
            return new DataStore();
        }

        private static void LogError(string message)
        {
            try
            {
                var logFile = "fitness_tracker_errors.log";
                File.AppendAllText(logFile, $"[{DateTime.Now:G}] {message}\n");
            }
            catch
            {
                // Silently ignore logging errors
            }
        }
    }
}
