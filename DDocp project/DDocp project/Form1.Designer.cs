namespace DDocp_project
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls as class fields
        private Panel pnlLogin;
        private Panel pnlDashboard;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblLoginMessage;
        private Label lblWelcome;
        private TextBox txtGoal;
        private Button btnSetGoal;
        private ComboBox comboActivity;
        private Label lblM1;
        private Label lblM2;
        private Label lblM3;
        private TextBox txtM1;
        private TextBox txtM2;
        private TextBox txtM3;
        private Button btnRecord;
        private ListBox listActivities;
        private Label lblTotal;
        private Label lblGoalStatus;
        private Button btnLogout;
        private ToolTip toolTip1;
        private Label lblTitle;
        private Label lblActivityLog;
        private Panel pnlActivityCard;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "💪 Fitness Tracker Pro";
            this.ClientSize = new Size(1100, 720);
            this.BackColor = Color.FromArgb(245, 245, 245);
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            toolTip1 = new ToolTip();

            // ===== LOGIN PANEL (Centered Card) =====
            pnlLogin = new Panel()
            {
                Location = new Point(300, 120),
                Size = new Size(500, 380),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            // Shadow effect via fake border
            var pnlLoginShadow = new Panel()
            {
                Location = new Point(295, 115),
                Size = new Size(510, 390),
                BackColor = Color.FromArgb(200, 200, 200),
                BorderStyle = BorderStyle.None
            };

            // Title
            lblTitle = new Label()
            {
                Text = "💪 Welcome to Fitness Tracker Pro",
                Location = new Point(30, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243)
            };

            // Username label & textbox
            var lblUser = new Label()
            {
                Text = "Username:",
                Location = new Point(30, 70),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtUsername = new TextBox()
            {
                Location = new Point(30, 95),
                Width = 440,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                Text = "Enter username (letters & numbers only)",
                BorderStyle = BorderStyle.FixedSingle
            };
            txtUsername.GotFocus += txtUsername_Enter;
            txtUsername.LostFocus += txtUsername_Leave;
            toolTip1.SetToolTip(txtUsername, "Usernames: letters and numbers only (e.g., john123)");

            // Password label & textbox
            var lblPass = new Label()
            {
                Text = "Password:",
                Location = new Point(30, 145),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtPassword = new TextBox()
            {
                Location = new Point(30, 170),
                Width = 440,
                Height = 36,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.Gray,
                Text = "Enter password (12 chars: upper + lower)",
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = false
            };
            txtPassword.GotFocus += txtPassword_Enter;
            txtPassword.LostFocus += txtPassword_Leave;
            toolTip1.SetToolTip(txtPassword, "Exactly 12 characters with at least 1 uppercase and 1 lowercase");

            // Buttons
            btnLogin = new Button()
            {
                Text = "🔐 Log In",
                Location = new Point(30, 240),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += btnLogin_Click;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = Color.FromArgb(21, 128, 211);
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = Color.FromArgb(33, 150, 243);

            btnRegister = new Button()
            {
                Text = "✍️ Register",
                Location = new Point(270, 240),
                Size = new Size(200, 40),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += btnRegister_Click;
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = Color.FromArgb(56, 142, 60);
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = Color.FromArgb(76, 175, 80);

            // Error message
            lblLoginMessage = new Label()
            {
                Text = "",
                Location = new Point(30, 300),
                Width = 440,
                Height = 60,
                ForeColor = Color.FromArgb(244, 67, 54),
                Font = new Font("Segoe UI", 9.5F),
                AutoSize = false,
                TextAlign = ContentAlignment.TopLeft
            };

            pnlLogin.Controls.Add(lblTitle);
            pnlLogin.Controls.Add(lblUser);
            pnlLogin.Controls.Add(txtUsername);
            pnlLogin.Controls.Add(lblPass);
            pnlLogin.Controls.Add(txtPassword);
            pnlLogin.Controls.Add(btnLogin);
            pnlLogin.Controls.Add(btnRegister);
            pnlLogin.Controls.Add(lblLoginMessage);

            // ===== DASHBOARD PANEL =====
            pnlDashboard = new Panel()
            {
                Location = new Point(20, 20),
                Size = new Size(1060, 680),
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Welcome label
            lblWelcome = new Label()
            {
                Text = "Welcome",
                Location = new Point(20, 16),
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243)
            };

            // Goal setting area
            var lblGoalLabel = new Label()
            {
                Text = "Set Daily Goal (kcal):",
                Location = new Point(20, 56),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9.5F)
            };
            txtGoal = new TextBox()
            {
                Location = new Point(180, 52),
                Width = 120,
                Height = 28,
                Font = new Font("Segoe UI", 11F),
                BorderStyle = BorderStyle.FixedSingle
            };
            btnSetGoal = new Button()
            {
                Text = "💾 Save Goal",
                Location = new Point(320, 48),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(255, 152, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSetGoal.FlatAppearance.BorderSize = 0;
            btnSetGoal.Click += btnSetGoal_Click;
            btnSetGoal.MouseEnter += (s, e) => btnSetGoal.BackColor = Color.FromArgb(245, 127, 0);
            btnSetGoal.MouseLeave += (s, e) => btnSetGoal.BackColor = Color.FromArgb(255, 152, 0);

            // ===== ACTIVITY CARD (LEFT PANEL) =====
            pnlActivityCard = new Panel()
            {
                Location = new Point(20, 110),
                Size = new Size(460, 380),
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblActivityTitle = new Label()
            {
                Text = "📊 Record Activity",
                Location = new Point(14, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243)
            };

            var lblActivityLabel = new Label()
            {
                Text = "Select Activity:",
                Location = new Point(14, 48),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9.5F)
            };
            comboActivity = new ComboBox()
            {
                Location = new Point(120, 44),
                Width = 320,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboActivity.SelectedIndexChanged += comboActivity_SelectedIndexChanged;

            // Metrics inputs
            lblM1 = new Label()
            {
                Text = "Metric 1:",
                Location = new Point(14, 100),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9F)
            };
            txtM1 = new TextBox()
            {
                Location = new Point(120, 96),
                Width = 320,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblM2 = new Label()
            {
                Text = "Metric 2:",
                Location = new Point(14, 150),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9F)
            };
            txtM2 = new TextBox()
            {
                Location = new Point(120, 146),
                Width = 320,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblM3 = new Label()
            {
                Text = "Metric 3:",
                Location = new Point(14, 200),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 66, 66),
                Font = new Font("Segoe UI", 9F)
            };
            txtM3 = new TextBox()
            {
                Location = new Point(120, 196),
                Width = 320,
                Height = 28,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnRecord = new Button()
            {
                Text = "✅ Record Activity",
                Location = new Point(120, 250),
                Size = new Size(320, 40),
                BackColor = Color.FromArgb(3, 169, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRecord.FlatAppearance.BorderSize = 0;
            btnRecord.Click += btnRecord_Click;
            btnRecord.MouseEnter += (s, e) => btnRecord.BackColor = Color.FromArgb(2, 136, 209);
            btnRecord.MouseLeave += (s, e) => btnRecord.BackColor = Color.FromArgb(3, 169, 244);

            pnlActivityCard.Controls.Add(lblActivityTitle);
            pnlActivityCard.Controls.Add(lblActivityLabel);
            pnlActivityCard.Controls.Add(comboActivity);
            pnlActivityCard.Controls.Add(lblM1);
            pnlActivityCard.Controls.Add(txtM1);
            pnlActivityCard.Controls.Add(lblM2);
            pnlActivityCard.Controls.Add(txtM2);
            pnlActivityCard.Controls.Add(lblM3);
            pnlActivityCard.Controls.Add(txtM3);
            pnlActivityCard.Controls.Add(btnRecord);

            // ===== ACTIVITY LOG (RIGHT PANEL) =====
            lblActivityLog = new Label()
            {
                Text = "📋 Activity History",
                Location = new Point(500, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 150, 243)
            };

            listActivities = new ListBox()
            {
                Location = new Point(500, 140),
                Size = new Size(540, 280),
                Font = new Font("Segoe UI", 9.5F),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Stats area
            lblTotal = new Label()
            {
                Text = "📈 Total Calories: 0.0 kcal",
                Location = new Point(500, 430),
                AutoSize = true,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80)
            };

            lblGoalStatus = new Label()
            {
                Text = "🎯 Goal Status: No goal set",
                Location = new Point(500, 465),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(244, 67, 54)
            };

            // Logout button
            btnLogout = new Button()
            {
                Text = "🚪 Log Out",
                Location = new Point(965, 12),
                Size = new Size(85, 36),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += btnLogout_Click;
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(150, 150, 150);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.FromArgb(200, 200, 200);

            // Add to dashboard
            pnlDashboard.Controls.Add(lblWelcome);
            pnlDashboard.Controls.Add(lblGoalLabel);
            pnlDashboard.Controls.Add(txtGoal);
            pnlDashboard.Controls.Add(btnSetGoal);
            pnlDashboard.Controls.Add(pnlActivityCard);
            pnlDashboard.Controls.Add(lblActivityLog);
            pnlDashboard.Controls.Add(listActivities);
            pnlDashboard.Controls.Add(lblTotal);
            pnlDashboard.Controls.Add(lblGoalStatus);
            pnlDashboard.Controls.Add(btnLogout);

            // Add to form
            this.Controls.Add(pnlLogin);
            this.Controls.Add(pnlDashboard);
        }
    }
}
