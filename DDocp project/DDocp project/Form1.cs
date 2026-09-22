using System.Text;
using System.Text.RegularExpressions;

namespace DDocp_project
{
    public partial class Form1 : Form
    {
        private DataStore _store;
        private User _currentUser;
        private int _failedAttempts = 0;
        private DateTime _lockoutUntil = DateTime.MinValue;
        private const int LOCKOUT_SECONDS = 30;
        private const int MAX_FAILED_ATTEMPTS = 3;

        public Form1()
        {
            InitializeComponent();
            try
            {
                _store = DataStore.Load();
                SetupActivityList();
                ShowLoginPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize app: {ex.Message}", "Init Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void SetupActivityList()
        {
            comboActivity.Items.Clear();
            comboActivity.Items.AddRange(new string[] { "Walking", "Swimming", "Running", "Cycling", "Rowing", "Yoga" });
            comboActivity.SelectedIndex = 0;
            UpdateMetricLabels();
        }

        private void ShowLoginPanel()
        {
            pnlLogin.Visible = true;
            pnlDashboard.Visible = false;
            lblLoginMessage.Text = "";
        }

        private void ShowDashboard()
        {
            pnlLogin.Visible = false;
            pnlDashboard.Visible = true;
            lblWelcome.Text = $"Welcome, {_currentUser.Username}";
            txtGoal.Text = _currentUser.GoalCalories > 0 ? _currentUser.GoalCalories.ToString() : "";
            RefreshActivityLog();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                // Clear previous message
                lblLoginMessage.Text = "";

                // Validate username
                if (string.IsNullOrWhiteSpace(username))
                {
                    ShowError("❌ Username cannot be empty.");
                    return;
                }
                if (!ValidateUsername(username))
                {
                    ShowError("❌ Username must contain only letters (A-Z) and numbers (0-9). No spaces or special characters.");
                    return;
                }

                // Validate password
                if (string.IsNullOrEmpty(password))
                {
                    ShowError("❌ Password cannot be empty.");
                    return;
                }
                if (!ValidatePassword(password))
                {
                    ShowError("❌ Password must be EXACTLY 12 characters with at least 1 uppercase and 1 lowercase letter.\nExample: MyPassword01");
                    return;
                }

                // Check if username already exists
                if (_store.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    ShowError("❌ Username already taken. Try a different username.");
                    return;
                }

                // Create new user
                var user = new User { Username = username };
                user.SetPassword(password);
                _store.Users.Add(user);
                _store.Save();

                ShowSuccess("✅ Registration successful! You can now log in with your credentials.");
                txtUsername.Text = "Enter username (letters & numbers only)";
                txtPassword.Text = "Enter password (12 chars: upper + lower)";
                txtUsername.ForeColor = Color.Gray;
                txtPassword.ForeColor = Color.Gray;
                txtPassword.UseSystemPasswordChar = false;
            }
            catch (Exception ex)
            {
                ShowError($"❌ Registration failed: {ex.Message}");
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Check lockout status
                if (DateTime.Now < _lockoutUntil)
                {
                    var remainingSeconds = (int)(_lockoutUntil - DateTime.Now).TotalSeconds;
                    ShowError($"🔒 Too many failed attempts. Try again in {remainingSeconds} seconds.");
                    return;
                }

                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                // Validate inputs not empty
                if (string.IsNullOrWhiteSpace(username) || username == "Enter username (letters & numbers only)")
                {
                    ShowError("❌ Please enter your username.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(password) || password == "Enter password (12 chars: upper + lower)")
                {
                    ShowError("❌ Please enter your password.");
                    return;
                }

                // Find user
                var user = _store.Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user == null || !user.VerifyPassword(password))
                {
                    _failedAttempts++;
                    if (_failedAttempts >= MAX_FAILED_ATTEMPTS)
                    {
                        _lockoutUntil = DateTime.Now.AddSeconds(LOCKOUT_SECONDS);
                        ShowError($"🔒 Too many failed attempts. Account locked for {LOCKOUT_SECONDS} seconds.");
                        _failedAttempts = 0;
                    }
                    else
                    {
                        int remaining = MAX_FAILED_ATTEMPTS - _failedAttempts;
                        ShowError($"❌ Invalid credentials. ({remaining} attempts remaining)");
                    }
                    return;
                }

                // Login successful
                _currentUser = user;
                _failedAttempts = 0;
                lblLoginMessage.Text = "";
                ShowDashboard();
            }
            catch (Exception ex)
            {
                ShowError($"❌ Login failed: {ex.Message}");
            }
        }

        private bool ValidateUsername(string username)
        {
            return !string.IsNullOrEmpty(username) && Regex.IsMatch(username, "^[a-zA-Z0-9]+$");
        }

        private bool ValidatePassword(string password)
        {
            if (password == null) return false;
            if (password.Length != 12) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsUpper)) return false;
            return true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            _currentUser = null;
            txtUsername.Text = "";
            txtPassword.Text = "";
            ShowLoginPanel();
        }

        // Helper methods for consistent error/success display
        private void ShowError(string message)
        {
            lblLoginMessage.Text = message;
            lblLoginMessage.ForeColor = Color.FromArgb(244, 67, 54); // Red
        }

        private void ShowSuccess(string message)
        {
            lblLoginMessage.Text = message;
            lblLoginMessage.ForeColor = Color.FromArgb(76, 175, 80); // Green
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Enter username (letters & numbers only)")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.Black;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                txtUsername.Text = "Enter username (letters & numbers only)";
                txtUsername.ForeColor = Color.Gray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Enter password (12 chars: upper + lower)")
            {
                txtPassword.Text = "";
                txtPassword.UseSystemPasswordChar = true;
                txtPassword.ForeColor = Color.Black;
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.UseSystemPasswordChar = false;
                txtPassword.Text = "Enter password (12 chars: upper + lower)";
                txtPassword.ForeColor = Color.Gray;
            }
        }

        private void comboActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMetricLabels();
        }

        private void UpdateMetricLabels()
        {
            var activity = comboActivity.SelectedItem?.ToString() ?? "Walking";
            switch (activity)
            {
                case "Walking":
                    lblM1.Text = "Steps"; lblM2.Text = "Distance (km)"; lblM3.Text = "Duration (min)"; break;
                case "Swimming":
                    lblM1.Text = "Laps"; lblM2.Text = "Pool Length (m)"; lblM3.Text = "Duration (min)"; break;
                case "Running":
                    lblM1.Text = "Distance (km)"; lblM2.Text = "Duration (min)"; lblM3.Text = "Avg Heart Rate"; break;
                case "Cycling":
                    lblM1.Text = "Distance (km)"; lblM2.Text = "Duration (min)"; lblM3.Text = "Avg Speed (km/h)"; break;
                case "Rowing":
                    lblM1.Text = "Strokes"; lblM2.Text = "Distance (km)"; lblM3.Text = "Duration (min)"; break;
                case "Yoga":
                    lblM1.Text = "Duration (min)"; lblM2.Text = "Intensity (1-10)"; lblM3.Text = "Calories/hr estimate"; break;
                default:
                    lblM1.Text = "M1"; lblM2.Text = "M2"; lblM3.Text = "M3"; break;
            }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentUser == null)
                {
                    MessageBox.Show("User session expired. Please log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var activity = comboActivity.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(activity))
                {
                    MessageBox.Show("❌ Please select an activity.", "Missing Activity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate metrics
                if (!double.TryParse(txtM1.Text, out double m1) || m1 <= 0)
                {
                    MessageBox.Show($"❌ Invalid Metric 1 value. Must be a positive number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!double.TryParse(txtM2.Text, out double m2) || m2 <= 0)
                {
                    MessageBox.Show($"❌ Invalid Metric 2 value. Must be a positive number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!double.TryParse(txtM3.Text, out double m3) || m3 <= 0)
                {
                    MessageBox.Show($"❌ Invalid Metric 3 value. Must be a positive number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var calories = CalculateCalories(activity, m1, m2, m3);
                var record = new ActivityRecord
                {
                    ActivityName = activity,
                    Metric1 = m1,
                    Metric2 = m2,
                    Metric3 = m3,
                    Calories = calories,
                    Timestamp = DateTime.Now,
                };
                _currentUser.ActivityRecords.Add(record);
                _store.Save();

                // Clear inputs
                txtM1.Text = "";
                txtM2.Text = "";
                txtM3.Text = "";

                RefreshActivityLog();
                MessageBox.Show($"✅ Activity recorded!\n\n{activity}: {calories:F1} calories burned", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error recording activity: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double CalculateCalories(string activity, double m1, double m2, double m3)
        {
            // Simple approximate formulas using the three metrics; not medical grade.
            switch (activity)
            {
                case "Walking":
                    // m1=steps, m2=distance_km, m3=duration_min
                    return 0.04 * m1 + 50 * m2 + 0.8 * m3;
                case "Swimming":
                    // m1=laps, m2=poolLength_m, m3=duration_min
                    return 0.1 * m1 + 0.02 * m2 * m1 + 8 * m3;
                case "Running":
                    // m1=distance_km, m2=duration_min, m3=avgHR
                    return 60 * m1 + 0.1 * m3 * (m2 / 60.0);
                case "Cycling":
                    // m1=distance_km, m2=duration_min, m3=avgSpeed
                    return 30 * m1 + 0.5 * m3 * (m2 / 60.0);
                case "Rowing":
                    // m1=strokes, m2=distance_km, m3=duration_min
                    return 0.02 * m1 + 40 * m2 + 0.6 * m3;
                case "Yoga":
                    // m1=duration_min, m2=intensity(1-10), m3=cal/hr estimate
                    return (m3 / 60.0) * m1 * Math.Max(1, m2 / 5.0);
                default:
                    return 0;
            }
        }

        private void RefreshActivityLog()
        {
            try
            {
                listActivities.Items.Clear();
                if (_currentUser == null) return;

                if (_currentUser.ActivityRecords.Count == 0)
                {
                    listActivities.Items.Add("No activities recorded yet. Start tracking!");
                    lblTotal.Text = "📈 Total Calories: 0.0 kcal";
                    lblGoalStatus.Text = "🎯 Goal Status: No goal set";
                    return;
                }

                // Display activities in reverse chronological order
                foreach (var r in _currentUser.ActivityRecords.OrderByDescending(x => x.Timestamp))
                {
                    listActivities.Items.Add($"{r.Timestamp:g} | {r.ActivityName} | {r.Calories:F1} kcal");
                }

                // Calculate totals
                var total = _currentUser.ActivityRecords.Sum(x => x.Calories);
                lblTotal.Text = $"📈 Total Calories: {total:F1} kcal";

                // Update goal status
                if (_currentUser.GoalCalories > 0)
                {
                    if (total >= _currentUser.GoalCalories)
                    {
                        lblGoalStatus.Text = $"🎯 Goal Status: ✅ ACHIEVED! ({total:F1} / {_currentUser.GoalCalories:F1})";
                        lblGoalStatus.ForeColor = Color.FromArgb(76, 175, 80); // Green
                    }
                    else
                    {
                        var remaining = _currentUser.GoalCalories - total;
                        lblGoalStatus.Text = $"🎯 Goal Status: {remaining:F1} kcal remaining ({total:F1} / {_currentUser.GoalCalories:F1})";
                        lblGoalStatus.ForeColor = Color.FromArgb(244, 67, 54); // Red
                    }
                }
                else
                {
                    lblGoalStatus.Text = "🎯 Goal Status: No goal set";
                    lblGoalStatus.ForeColor = Color.FromArgb(66, 66, 66);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error refreshing activity log: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSetGoal_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentUser == null) return;

                if (!double.TryParse(txtGoal.Text, out double goal) || goal <= 0)
                {
                    MessageBox.Show("❌ Please enter a valid positive number for your daily goal.", "Invalid Goal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _currentUser.GoalCalories = goal;
                _store.Save();
                RefreshActivityLog();
                MessageBox.Show($"✅ Daily goal set to {goal:F1} kcal!", "Goal Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error setting goal: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }
    }
}
