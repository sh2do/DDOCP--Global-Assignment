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

        // Minimalist Palette
        private static readonly Color BgColor = Color.FromArgb(245, 247, 250);
        private static readonly Color CardBg = Color.White;
        private static readonly Color PrimaryColor = Color.FromArgb(41, 128, 185);
        private static readonly Color PrimaryText = Color.FromArgb(44, 62, 80);
        private static readonly Color SecondaryBtnBg = Color.FromArgb(236, 240, 241);

        public MainForm()
        {
            Text = "Fitness Tracker - Task 1 (Minimal)";
            Size = new Size(840, 680);
            MinimumSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = BgColor;
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            InitializeLogin();
            InitializeRegister();
            InitializeMain();

            ShowLogin();
        }

        private void InitializeLogin()
        {
            loginPanel = new Panel { Dock = DockStyle.Fill, BackColor = BgColor };

            var card = new Panel
            {
                Size = new Size(380, 320),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point((Width - 380) / 2, 100)
            };
            loginPanel.SizeChanged += (s, e) =>
            {
                card.Location = new Point((loginPanel.Width - card.Width) / 2, Math.Max(40, (loginPanel.Height - card.Height) / 2));
            };

            var lblTitle = new Label
            {
                Text = "Fitness Tracker Sign In",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = PrimaryText,
                Location = new Point(30, 25),
                AutoSize = true
            };

            var lblUser = new Label { Text = "Username", Location = new Point(30, 75), AutoSize = true, ForeColor = PrimaryText };
            txtLoginUser = new TextBox { Location = new Point(30, 95), Width = 318, Font = new Font("Segoe UI", 10f) };

            var lblPass = new Label { Text = "Password", Location = new Point(30, 135), AutoSize = true, ForeColor = PrimaryText };
            txtLoginPass = new TextBox { Location = new Point(30, 155), Width = 318, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 10f) };

            btnLogin = new Button
            {
                Text = "Sign In",
                Location = new Point(30, 205),
                Width = 150,
                Height = 36,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnShowRegister = new Button
            {
                Text = "Register Account",
                Location = new Point(195, 205),
                Width = 153,
                Height = 36,
                BackColor = SecondaryBtnBg,
                ForeColor = PrimaryText,
                FlatStyle = FlatStyle.Flat
            };
            btnShowRegister.FlatAppearance.BorderSize = 0;
            btnShowRegister.Click += (s, e) => ShowRegister();

            lblLoginMsg = new Label { Location = new Point(30, 255), ForeColor = Color.Crimson, AutoSize = true };

            card.Controls.AddRange(new Control[] { lblTitle, lblUser, txtLoginUser, lblPass, txtLoginPass, btnLogin, btnShowRegister, lblLoginMsg });
            loginPanel.Controls.Add(card);
            Controls.Add(loginPanel);
        }

        private void InitializeRegister()
        {
            registerPanel = new Panel { Dock = DockStyle.Fill, Visible = false, BackColor = BgColor };

            var card = new Panel
            {
                Size = new Size(460, 340),
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point((Width - 460) / 2, 90)
            };
            registerPanel.SizeChanged += (s, e) =>
            {
                card.Location = new Point((registerPanel.Width - card.Width) / 2, Math.Max(40, (registerPanel.Height - card.Height) / 2));
            };

            var lblTitle = new Label
            {
                Text = "Create New Account",
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = PrimaryText,
                Location = new Point(30, 25),
                AutoSize = true
            };

            var lblUser = new Label { Text = "Username", Location = new Point(30, 75), AutoSize = true, ForeColor = PrimaryText };
            txtRegUser = new TextBox { Location = new Point(30, 95), Width = 398, Font = new Font("Segoe UI", 10f) };

            var lblPass = new Label { Text = "Password (12 chars, 1 uppercase, 1 lowercase)", Location = new Point(30, 135), AutoSize = true, ForeColor = PrimaryText };
            txtRegPass = new TextBox { Location = new Point(30, 155), Width = 398, UseSystemPasswordChar = true, Font = new Font("Segoe UI", 10f) };

            btnRegister = new Button
            {
                Text = "Create Account",
                Location = new Point(30, 210),
                Width = 190,
                Height = 36,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;

            btnBackToLogin = new Button
            {
                Text = "Back to Sign In",
                Location = new Point(235, 210),
                Width = 193,
                Height = 36,
                BackColor = SecondaryBtnBg,
                ForeColor = PrimaryText,
                FlatStyle = FlatStyle.Flat
            };
            btnBackToLogin.FlatAppearance.BorderSize = 0;
            btnBackToLogin.Click += (s, e) => ShowLogin();

            lblRegMsg = new Label { Location = new Point(30, 265), ForeColor = Color.Crimson, AutoSize = true };

            card.Controls.AddRange(new Control[] { lblTitle, lblUser, txtRegUser, lblPass, txtRegPass, btnRegister, btnBackToLogin, lblRegMsg });
            registerPanel.Controls.Add(card);
            Controls.Add(registerPanel);
        }

        private void InitializeMain()
        {
            mainPanel = new Panel { Dock = DockStyle.Fill, Visible = false, AutoScroll = true, BackColor = BgColor };

            var topCard = new Panel
            {
                Location = new Point(20, 20),
                Width = 780,
                Height = 85,
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblWelcome = new Label { Text = "", Location = new Point(20, 15), AutoSize = true, Font = new Font("Segoe UI", 12f, FontStyle.Bold), ForeColor = PrimaryText };
            var lblGoal = new Label { Text = "Calorie Goal:", Location = new Point(20, 47), AutoSize = true, ForeColor = PrimaryText };
            numGoal = new NumericUpDown { Location = new Point(110, 44), Width = 110, Minimum = 0, Maximum = 100000, Font = new Font("Segoe UI", 9.5f) };
            btnSaveGoal = new Button
            {
                Text = "Save Goal",
                Location = new Point(235, 42),
                Width = 100,
                Height = 28,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSaveGoal.FlatAppearance.BorderSize = 0;
            btnSaveGoal.Click += BtnSaveGoal_Click;

            topCard.Controls.AddRange(new Control[] { lblWelcome, lblGoal, numGoal, btnSaveGoal });
            mainPanel.Controls.Add(topCard);

            int y = 115;
            foreach (var act in activities)
            {
                var grp = new GroupBox
                {
                    Text = "  " + act + "  ",
                    Location = new Point(20, y),
                    Width = 780,
                    Height = 80,
                    BackColor = CardBg,
                    Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                    ForeColor = PrimaryText
                };

                var labels = metricLabels.ContainsKey(act) ? metricLabels[act] : (m1: "Metric1", m2: "Metric2", m3: "Metric3");

                var lblA = new Label { Text = labels.m1 + ":", Location = new Point(15, 32), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = PrimaryText };
                var tA = new TextBox { Location = new Point(110, 28), Width = 120, Font = new Font("Segoe UI", 9.5f, FontStyle.Regular) };

                var lblB = new Label { Text = labels.m2 + ":", Location = new Point(265, 32), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = PrimaryText };
                var tB = new TextBox { Location = new Point(365, 28), Width = 120, Font = new Font("Segoe UI", 9.5f, FontStyle.Regular) };

                var lblC = new Label { Text = labels.m3 + ":", Location = new Point(520, 32), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = PrimaryText };
                var tC = new TextBox { Location = new Point(625, 28), Width = 120, Font = new Font("Segoe UI", 9.5f, FontStyle.Regular) };

                grp.Controls.AddRange(new Control[] { lblA, tA, lblB, tB, lblC, tC });
                mainPanel.Controls.Add(grp);
                activityInputs[act] = (tA, tB, tC);
                y += 90;
            }

            var bottomCard = new Panel
            {
                Location = new Point(20, y),
                Width = 780,
                Height = 65,
                BackColor = CardBg,
                BorderStyle = BorderStyle.FixedSingle
            };

            btnCalculate = new Button
            {
                Text = "Calculate Calories",
                Location = new Point(15, 14),
                Width = 160,
                Height = 36,
                BackColor = PrimaryColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            btnCalculate.FlatAppearance.BorderSize = 0;
            btnCalculate.Click += BtnCalculate_Click;

            lblTotal = new Label { Text = "Total: 0", Location = new Point(190, 23), AutoSize = true, Font = new Font("Segoe UI", 10.5f, FontStyle.Bold), ForeColor = PrimaryText };
            lblGoalStatus = new Label { Text = "", Location = new Point(410, 23), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold) };

            bottomCard.Controls.AddRange(new Control[] { btnCalculate, lblTotal, lblGoalStatus });
            mainPanel.Controls.Add(bottomCard);

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
            lblRegMsg.ForeColor = Color.Crimson;
            if (!Regex.IsMatch(user, "^[a-zA-Z0-9]+$"))
            {
                lblRegMsg.Text = "Username must contain only letters and numbers.";
                return;
            }
            if (pass.Length != 12 || !pass.Any(char.IsUpper) || !pass.Any(char.IsLower))
            {
                lblRegMsg.Text = "Password must be 12 chars (1 upper, 1 lower).";
                return;
            }
            if (DataStore.Find(user) != null)
            {
                lblRegMsg.Text = "That username is already taken.";
                return;
            }
            DataStore.Users.Add(new User { Username = user, Password = pass });
            DataStore.Save();
            lblRegMsg.ForeColor = Color.DarkGreen;
            lblRegMsg.Text = "Account created. You can log in now.";
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (DateTime.Now < lockoutUntil)
            {
                lblLoginMsg.Text = $"Too many failed attempts. Try again at {lockoutUntil:T}.";
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
                    lockoutUntil = DateTime.Now.AddSeconds(30);
                    lblLoginMsg.Text = "Too many failed attempts. Disabled for 30 seconds.";
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
            MessageBox.Show("Goal saved successfully.", "Goal Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    lblGoalStatus.ForeColor = Color.DarkGreen;
                    lblGoalStatus.Text = "Goal achieved!";
                }
                else
                {
                    lblGoalStatus.ForeColor = PrimaryText;
                    lblGoalStatus.Text = $"Goal in progress: {u.GoalCalories - (int)total} cal remaining.";
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

        private static double CalculateForActivity(string activity, double m1, double m2, double m3)
        {
            return activity switch
            {
                "Walking" => m2 * 50 + m1 * 0.03 + m3 * 0.1,
                "Swimming" => m1 * 8 + m2 * 5 + m3 * 0.1,
                "Running" => m1 * 70 + m2 * 5 + m3 * 0.1,
                "Cycling" => m1 * 35 + m2 * 6 + m3 * 0.1,
                "Rowing" => m2 * 9 + m1 * 0.02 + m3 * 0.1,
                "Yoga" => m1 * 3 + m2 * 20 + m3 * 0.05,
                _ => 0
            };
        }
    }
}

