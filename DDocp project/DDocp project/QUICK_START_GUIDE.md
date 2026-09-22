# Fitness Tracker Pro - Quick Start Guide

## 🚀 Installation & Running

### From Visual Studio
1. Open **DDocp project.slnx** in Visual Studio 2026
2. Press **F5** or click **Debug > Start Debugging**
3. App launches immediately (first run takes ~2 seconds to initialize)

### From Command Line
```powershell
cd "C:\Users\Administrator\source\repos\sh2do\DDocp project"
dotnet run
```

### Data Files
- Users stored in: `users.json` (created in app's working directory on first save)
- Errors logged to: `fitness_tracker_errors.log`
- Backup created: `users.json.backup` (auto-generated before each save)

---

## 🔐 First-Time Setup

### Step 1: Create an Account
1. Click **"✍️ Register"** button
2. Enter a username:
   - Example: `john123` or `FitnessJoe`
   - ✅ Allowed: A-Z, a-z, 0-9
   - ❌ Not allowed: spaces, @, #, special chars
3. Enter a password:
   - Must be exactly 12 characters
   - Must have at least 1 UPPERCASE letter
   - Must have at least 1 lowercase letter
   - Example: `MyPassword01` ✅ or `FitLife2025!` ✅
4. Click **"✍️ Register"**
5. See success message: "✅ Registration successful!"

### Step 2: Log In
1. Enter your username and password again
2. Click **"🔐 Log In"**
3. You're now in the Dashboard!

---

## 📊 Dashboard Overview

Once logged in, you see:

```
[LEFT SIDE]                          [RIGHT SIDE]
┌─────────────────────┐              ┌──────────────────────┐
│ 📊 Record Activity  │              │ 📋 Activity History  │
│ ─────────────────── │              │ ────────────────── │
│ Activity: [Walking] │   ════════   │ 2025-01-17 12:30   │
│ Metric 1: [1000]    │              │ Walking: 60 kcal   │
│ Metric 2: [1.5]     │              │ 2025-01-16 18:45   │
│ Metric 3: [30]      │              │ Running: 300 kcal  │
│ ✅ Record Activity  │              │ 2025-01-16 08:00   │
└─────────────────────┘              │ Yoga: 100 kcal     │
									 └──────────────────────┘
		 STATS (Bottom)
	  📈 Total Calories: 460.0 kcal
	  🎯 Goal Status: 240 kcal remaining (460 / 700)
```

---

## 🏃 Recording Your First Activity

### Example 1: Walking
1. **Select Activity**: Click dropdown → choose "Walking"
2. **Enter Metrics**:
   - **Metric 1 (Steps)**: `1000`
   - **Metric 2 (Distance km)**: `0.8`
   - **Metric 3 (Duration min)**: `15`
3. Click **"✅ Record Activity"**
4. See popup: "✅ Activity recorded! Walking: 45.2 calories burned"
5. Activity appears in history with timestamp

### Example 2: Swimming
1. **Select Activity**: "Swimming"
2. **Enter Metrics**:
   - **Metric 1 (Laps)**: `10`
   - **Metric 2 (Pool Length m)**: `50`
   - **Metric 3 (Duration min)**: `25`
3. Click **"✅ Record Activity"**
4. Calories calculated and logged

### Example 3: Running
1. **Select Activity**: "Running"
2. **Enter Metrics**:
   - **Metric 1 (Distance km)**: `5`
   - **Metric 2 (Duration min)**: `30`
   - **Metric 3 (Avg Heart Rate)**: `150`
3. Click **"✅ Record Activity"**
4. View in log; total updates

---

## 🎯 Setting & Achieving Goals

### Set a Daily Goal
1. At top of dashboard, see **"Set Daily Goal (kcal):"**
2. Enter target: `500` (for 500 calories/day)
3. Click **"💾 Save Goal"**
4. See confirmation: "✅ Daily goal set to 500 kcal!"

### Track Goal Progress
- **Status at bottom shows**:
  - 🎯 Goal Status: ✅ ACHIEVED! (600 / 500)  ← Green if met
  - 🎯 Goal Status: 200 kcal remaining (300 / 500)  ← Red if not met

---

## ⚠️ Error Handling

### Wrong Username/Password
- **Message**: "❌ Invalid credentials. (3 attempts remaining)"
- **Solution**: Check spelling, case. If locked out, wait 30 seconds.

### Account Lockout
- **Trigger**: 3 failed login attempts
- **Message**: "🔒 Too many failed attempts. Try again in 25 seconds."
- **Solution**: Wait for timer, then try again

### Invalid Metric Input
- **Message**: "❌ Invalid Metric 1 value. Must be a positive number."
- **Solution**: Enter a number > 0 (e.g., `100`, `2.5`, not `0` or `-5`)

### Username Already Exists
- **Message**: "❌ Username already taken. Try a different username."
- **Solution**: Append a number (e.g., `john123` → `john1234`)

---

## 🔄 Daily Workflow

### Morning
1. ☀️ Log in with your credentials
2. 📋 Check yesterday's total at bottom
3. 🎯 Confirm your daily goal

### During Day
1. 🏃 After each workout, click **"Record Activity"**
2. 📊 See calories added to total
3. 📈 Watch progress toward your goal

### Evening
1. 📊 Review activity history
2. ✅ Celebrate if you hit your goal!
3. 🚪 Click **"🚪 Log Out"** to safely exit
4. 💾 Data automatically saved to users.json

---

## 📱 Activity Metrics Reference

### Walking
- **Metric 1**: Steps (e.g., `5000`, `10000`)
- **Metric 2**: Distance in km (e.g., `1.5`, `5.0`)
- **Metric 3**: Duration in minutes (e.g., `30`, `60`)

### Swimming
- **Metric 1**: Laps (e.g., `10`, `20`)
- **Metric 2**: Pool length in meters (e.g., `25`, `50`)
- **Metric 3**: Duration in minutes (e.g., `20`, `45`)

### Running
- **Metric 1**: Distance in km (e.g., `5`, `10`)
- **Metric 2**: Duration in minutes (e.g., `30`, `50`)
- **Metric 3**: Avg heart rate in bpm (e.g., `140`, `160`)

### Cycling
- **Metric 1**: Distance in km (e.g., `10`, `50`)
- **Metric 2**: Duration in minutes (e.g., `30`, `120`)
- **Metric 3**: Avg speed in km/h (e.g., `15`, `25`)

### Rowing
- **Metric 1**: Strokes (e.g., `500`, `2000`)
- **Metric 2**: Distance in km (e.g., `2`, `5`)
- **Metric 3**: Duration in minutes (e.g., `15`, `30`)

### Yoga
- **Metric 1**: Duration in minutes (e.g., `30`, `60`)
- **Metric 2**: Intensity 1-10 (e.g., `5`, `8`)
- **Metric 3**: Avg calories/hour (e.g., `150`, `240`)

---

## 🔐 Account Security Tips

- ✅ Use a **unique password** you don't use elsewhere
- ✅ Passwords are **hashed** (encrypted) before storage
- ✅ Data stays **local** (no internet upload)
- ✅ Backup file created **automatically** before saves
- ❌ Don't share users.json file with others (contains account info)

---

## 🆘 Troubleshooting

| Problem | Solution |
|---------|----------|
| App won't launch | Run as Administrator; check .NET 10 installed |
| Can't register username | Username already taken or has invalid characters |
| Forgot password | No recovery; create new username & password |
| Data not saving | Check directory write permissions; check error log |
| Metrics seem wrong | Calorie formulas are approximate; see documentation |
| Can't log back in | Check spelling of username/password; case-sensitive password |
| Activity log is empty | You haven't recorded any activities yet; start tracking! |

---

## 📁 File Structure

```
DDocp project/
├── DDocp project.slnx          (Solution file)
├── DDocp project.csproj        (Project file)
├── Form1.cs                    (Main logic)
├── Form1.Designer.cs           (UI controls)
├── Program.cs                  (Entry point)
├── DataStore.cs                (Persistence)
├── Models/
│   ├── User.cs                 (User model)
│   └── ActivityRecord.cs        (Activity model)
├── users.json                  (User data — created on first save)
├── users.json.backup           (Backup — auto-created)
└── fitness_tracker_errors.log  (Error log — auto-created)
```

---

## 🎯 Tips for Best Results

1. **Be Consistent**: Log activities daily for best tracking
2. **Be Accurate**: Enter realistic metrics (not extreme numbers)
3. **Set Achievable Goals**: Start with 300-500 kcal/day
4. **Vary Activities**: Mix cardio, strength, flexibility for balanced fitness
5. **Monitor Trends**: Keep app open for a week+ to see patterns
6. **Have Fun**: Use emojis and celebrate milestone achievements!

---

## 📞 Support

If you encounter issues:
1. Check **fitness_tracker_errors.log** for error details
2. Ensure **users.json** is not corrupted (can restore from backup)
3. Run Visual Studio as **Administrator** if permission errors occur
4. See **FEATURES_AND_ISSUES.md** for comprehensive documentation

---

**Version 1.0 | Last Updated: 2025-01-17**
