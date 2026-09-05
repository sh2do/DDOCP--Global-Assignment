using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FitnessTracker
{
    public class MainForm : Form
    {
        // UI controls
        private Panel loginPanel;
        private TextBox txtLoginUser;
        private TextBox txtLoginPass;
        private Button btnLogin;
        private Label lblLoginMsg;
        private Button btnShowRegister;

        private Panel registerPanel;
        private TextBox txtRegUser;
        private TextBox txtRegPass;
        private Button btnRegister;
        private Label lblRegMsg;
        private Button btnBackToLogin;

        private Panel mainPanel;
        private Label lblWelcome;
        private NumericUpDown numGoal;
        private Button btnSaveGoal;
        private Button btnCalculate;
        private Label lblTotal;
        private Label lblGoalStatus;

        private string currentUser = string.Empty;
        private int failedAttempts = 0;
        private DateTime lockoutUntil = DateTime.MinValue;

        private readonly List<string> activities = new List<string> { "Walking", "Swimming", "Running", "Cycling", "Rowing", "Yoga" };
        private readonly Dictionary<string, (TextBox a, TextBox b, TextBox c)> activityInputs = new();

        // Human-friendly metric labels per activity
        private readonly Dictionary<string, (string m1, string m2, string m3)> metricLabels = new()
        {
            { "Walking", ("Steps", "Distance (km)", "Avg heart rate") },
            { "Swimming", ("Time (min)", "Laps", "Avg heart rate") },
            { "Running", ("Distance (km)", "Time (min)", "Avg heart rate") },
            { "Cycling", ("Distance (km)", "Time (min)", "Avg heart rate") },
            { "Rowing", ("Strokes", "Time (min)", "Avg heart rate") },
            { "Yoga", ("Duration (min)", "Difficulty (1-5)", "Avg heart rate") }
        };

        public MainForm()
        {
            Text = "Fitness Tracker - Task 1 (Minimal)";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            InitializeLogin();
            InitializeRegister();
            InitializeMain();

            ShowLogin();
        }

        private void InitializeLogin()
        {
            loginPanel = new Panel { Dock = DockStyle.Fill };

            var lblUser = new Label { Text = "Username:", Location = new Point(200, 150), AutoSize = true };
            txtLoginUser = new TextBox { Location = new Point(300, 145), Width = 200 };
            var lblPass = new Label { Text = "Password:", Location = new Point(200, 190), AutoSize = true };
            txtLoginPass = new TextBox { Location = new Point(300, 185), Width = 200, UseSystemPasswordChar = true };
            btnLogin = new Button { Text = "Login", Location = new Point(300, 230) };
            btnLogin.Click += BtnLogin_Click;
            btnShowRegister = new Button { Text = "Register", Location = new Point(380, 230) };
            btnShowRegister.Click += (s, e) => ShowRegister();
            lblLoginMsg = new Label { Location = new Point(300, 270), ForeColor = Color.Red, AutoSize = true };

            loginPanel.Controls.AddRange(new Control[] { lblUser, txtLoginUser, lblPass, txtLoginPass, btnLogin, btnShowRegister, lblLoginMsg });
            Controls.Add(loginPanel);
        }

        private void InitializeRegister()
        {
            registerPanel = new Panel { Dock = DockStyle.Fill, Visible = false };
            var lblUser = new Label { Text = "New username:", Location = new Point(200, 150), AutoSize = true };
            txtRegUser = new TextBox { Location = new Point(320, 145), Width = 200 };
            var lblPass = new Label { Text = "Password (12 chars, 1 upper, 1 lower):", Location = new Point(200, 190), AutoSize = true };
            txtRegPass = new TextBox { Location = new Point(440, 185), Width = 200, UseSystemPasswordChar = true };
            btnRegister = new Button { Text = "Create Account", Location = new Point(320, 230) };
            btnRegister.Click += BtnRegister_Click;
            btnBackToLogin = new Button { Text = "Back", Location = new Point(440, 230) };
            btnBackToLogin.Click += (s, e) => ShowLogin();
            lblRegMsg = new Label { Location = new Point(320, 270), ForeColor = Color.Red, AutoSize = true };

            registerPanel.Controls.AddRange(new Control[] { lblUser, txtRegUser, lblPass, txtRegPass, btnRegister, btnBackToLogin, lblRegMsg });
            Controls.Add(registerPanel);
        }

        private void InitializeMain()
        {
            mainPanel = new Panel { Dock = DockStyle.Fill, Visible = false, AutoScroll = true };
            lblWelcome = new Label { Text = "", Location = new Point(20, 20), AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold) };
            var lblGoal = new Label { Text = "Set calorie goal:", Location = new Point(20, 60), AutoSize = true };
            numGoal = new NumericUpDown { Location = new Point(140, 58), Width = 100, Minimum = 0, Maximum = 100000 };
            btnSaveGoal = new Button { Text = "Save Goal", Location = new Point(260, 55) };
            btnSaveGoal.Click += BtnSaveGoal_Click;

            int y = 100;
            foreach (var act in activities)
            {
                var grp = new GroupBox { Text = act, Location = new Point(20, y), Width = 740, Height = 80 };
                // use friendly labels when available
                var labels = metricLabels.ContainsKey(act) ? metricLabels[act] : ("Metric1", "Metric2", "Metric3");
                var lblA = new Label { Text = labels.m1 + ":", Location = new Point(10, 25), AutoSize = true };
                var tA = new TextBox { Location = new Point(70, 22), Width = 120 };
                var lblB = new Label { Text = labels.m2 + ":", Location = new Point(210, 25), AutoSize = true };
                var tB = new TextBox { Location = new Point(270, 22), Width = 120 };
                var lblC = new Label { Text = labels.m3 + ":", Location = new Point(410, 25), AutoSize = true };
                var tC = new TextBox { Location = new Point(470, 22), Width = 120 };
                grp.Controls.AddRange(new Control[] { lblA, tA, lblB, tB, lblC, tC });
                mainPanel.Controls.Add(grp);
                activityInputs[act] = (tA, tB, tC);
                y += 90;
            }

            btnCalculate = new Button { Text = "Calculate Calories", Location = new Point(20, y) };
            btnCalculate.Click += BtnCalculate_Click;
            lblTotal = new Label { Text = "Total: 0", Location = new Point(160, y+5), AutoSize = true };
            lblGoalStatus = new Label { Text = "", Location = new Point(260, y+5), AutoSize = true, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold) };

            mainPanel.Controls.AddRange(new Control[] { lblWelcome, lblGoal, numGoal, btnSaveGoal, btnCalculate, lblTotal, lblGoalStatus });
            Controls.Add(mainPanel);
        }

        private void ShowLogin()
        {
            loginPanel.Visible = true;
            registerPanel.Visible = false;
            mainPanel.Visible = false;
            lblLoginMsg.Text = string.Empty;
        }

        private void ShowRegister()
        {
            loginPanel.Visible = false;
            registerPanel.Visible = true;
            mainPanel.Visible = false;
            lblRegMsg.Text = string.Empty;
        }

        private void ShowMain()
        {
            loginPanel.Visible = false;
            registerPanel.Visible = false;
            mainPanel.Visible = true;
            lblWelcome.Text = $"Welcome, {currentUser}";
            var user = DataStore.Find(currentUser);
            if (user != null)
                numGoal.Value = user.GoalCalories;
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            var user = txtRegUser.Text.Trim();
            var pass = txtRegPass.Text;
            lblRegMsg.ForeColor = Color.Red;
            if (!Regex.IsMatch(user, "^[a-zA-Z0-9]+$"))
            {
                lblRegMsg.Text = "Username must contain only letters and numbers.";
                return;
            }
            if (pass.Length != 12 || !pass.Any(char.IsUpper) || !pass.Any(char.IsLower))
            {
                lblRegMsg.Text = "Password must be exactly 12 characters and include at least one uppercase and one lowercase letter.";
                return;
            }
            if (DataStore.Find(user) != null)
            {
                lblRegMsg.Text = "That username is already taken.";
                return;
            }
            DataStore.Users.Add(new User { Username = user, Password = pass });
            DataStore.Save();
            lblRegMsg.ForeColor = Color.Green;
            lblRegMsg.Text = "Account created. You can log in now.";
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (DateTime.Now < lockoutUntil)
            {
                lblLoginMsg.Text = $"Too many failed attempts. Please try again at {lockoutUntil:T}.";
                return;
            }

            var user = txtLoginUser.Text.Trim();
            var pass = txtLoginPass.Text;
            var u = DataStore.Find(user);
            if (u != null && u.Password == pass)
            {
                currentUser = user;
                failedAttempts = 0;
                lockoutUntil = DateTime.MinValue;
                ShowMain();
            }
            else
            {
                failedAttempts++;
                if (failedAttempts >= 5)
                {
                    lockoutUntil = DateTime.Now.AddSeconds(30); // temporary lockout
                    lblLoginMsg.Text = "Too many failed attempts. Login disabled for 30 seconds.";
                }
                else
                {
                    lblLoginMsg.Text = $"Invalid credentials ({failedAttempts}/5).";
                }
            }
        }

        private void BtnSaveGoal_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentUser)) return;
            var u = DataStore.Find(currentUser);
            if (u == null) return;
            u.GoalCalories = (int)numGoal.Value;
            DataStore.Save();
            MessageBox.Show("Goal saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCalculate_Click(object? sender, EventArgs e)
        {
            double total = 0;
            foreach (var act in activities)
            {
                var inputs = activityInputs[act];
                double a = ParseDouble(inputs.a.Text);
                double b = ParseDouble(inputs.b.Text);
                double c = ParseDouble(inputs.c.Text);
                double calories = CalculateForActivity(act, a, b, c);
                total += calories;
            }
            lblTotal.Text = $"Total calories burned: {total:F1} cal";
            var u = DataStore.Find(currentUser);
            if (u != null && u.GoalCalories > 0)
            {
                if (total >= u.GoalCalories)
                {
                    lblGoalStatus.ForeColor = Color.Green;
                    lblGoalStatus.Text = "Congratulations — goal achieved!";
                }
                else
                {
                    lblGoalStatus.ForeColor = Color.Black;
                    lblGoalStatus.Text = $"Goal not yet achieved: {u.GoalCalories - (int)total} cal remaining.";
                }
            }
            else
            {
                lblGoalStatus.Text = string.Empty;
            }
        }

        private static double ParseDouble(string s)
        {
            if (double.TryParse(s, out var v)) return v;
            return 0;
        }

        // Minimal, research-inspired simple formulas using three metrics
        private static double CalculateForActivity(string activity, double m1, double m2, double m3)
        {
            // m1,m2,m3 are the three metrics for each activity (interpretation depends on activity)
            return activity switch
            {
                "Walking" => m2 * 50 + m1 * 0.03 + m3 * 0.1, // distance(km)*50 + steps*0.03 + avgHR*0.1
                "Swimming" => m1 * 8 + m2 * 5 + m3 * 0.1, // time(min)*8 + laps*5 + avgHR*0.1
                "Running" => m1 * 70 + m2 * 5 + m3 * 0.1, // distance(km)*70 + time(min)*5 + avgHR*0.1
                "Cycling" => m1 * 35 + m2 * 6 + m3 * 0.1, // distance(km)*35 + time(min)*6 + avgHR*0.1
                "Rowing" => m2 * 9 + m1 * 0.02 + m3 * 0.1, // time(min)*9 + strokes*0.02 + avgHR*0.1
                "Yoga" => m1 * 3 + m2 * 20 + m3 * 0.05, // duration(min)*3 + difficulty(1-5)*20 + avgHR*0.05
                _ => 0
            };
        }
    }
}
