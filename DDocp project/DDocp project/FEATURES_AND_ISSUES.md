# Fitness Tracker Pro - Features & Issues Report

## ✅ IMPLEMENTED FEATURES

### Authentication & Account Management
- **User Registration**: 
  - Username validation: letters and numbers only (A-Z, a-z, 0-9)
  - Password validation: exactly 12 characters with at least 1 uppercase and 1 lowercase letter
  - Duplicate username prevention
  - User-friendly error messages for invalid input

- **User Login**:
  - Secure password verification (SHA-256 hashing)
  - Failed login attempt counter
  - Account lockout: 3 failed attempts → 30-second lockout
  - Countdown display for remaining lockout time
  - Clear error messages indicating attempts remaining

### Fitness Activity Tracking
- **6 Supported Activities**:
  1. **Walking**: Steps, Distance (km), Duration (min)
  2. **Swimming**: Laps, Pool Length (m), Duration (min)
  3. **Running**: Distance (km), Duration (min), Avg Heart Rate (bpm)
  4. **Cycling**: Distance (km), Duration (min), Avg Speed (km/h)
  5. **Rowing**: Strokes, Distance (km), Duration (min)
  6. **Yoga**: Duration (min), Intensity (1-10), Calories/hr estimate

- **Activity Recording**:
  - 3 metrics per activity (user-defined)
  - Automatic calorie calculation (research-based formulas)
  - Timestamp for each activity
  - Input validation (positive numbers only)

### Goal Setting & Progress Tracking
- **Daily Calorie Goals**:
  - Set personalized daily calorie burn targets
  - Goal is saved per user and persistent
  - Real-time goal achievement status
  - Visual indicators (✅ achieved / ❌ not yet)
  - Remaining calories display

- **Activity History**:
  - Sortable activity log (most recent first)
  - Total calories burned calculation
  - Activity details: date/time, name, calories
  - Empty state message when no activities recorded

### Data Persistence
- **JSON-based Storage** (users.json):
  - All user data saved locally
  - User credentials (SHA-256 hashed passwords)
  - Activity history per user
  - Goal targets per user

- **Data Safety**:
  - Automatic backup creation before saving
  - Error recovery from corrupt JSON
  - Graceful fallback to empty data store
  - Error logging to fitness_tracker_errors.log

### User Interface
- **Modern, Stylish Design**:
  - Segoe UI font throughout
  - Flat design with custom colors:
	- Blue (#2196F3) for primary actions (Login)
	- Green (#4CAF50) for positive actions (Register, Goal)
	- Orange (#FF9800) for secondary actions (Save Goal)
	- Cyan (#03A9F4) for record activity
  - Button hover effects (color darkening)
  - Emoji icons for visual clarity
  - Clean separation: Login card → Dashboard

- **Login Panel** (Centered card):
  - Username/password input with placeholder text
  - Clear validation requirement tooltips
  - Success/error message display
  - Register and Login buttons

- **Dashboard**:
  - Welcome message with current user
  - Goal setting area (top)
  - Activity Card (left): record new activities
  - Activity Log (right): history and statistics
  - Total calories and goal status (bottom)
  - Logout button (top-right)

---

## ⚠️ KNOWN ISSUES & LIMITATIONS

### 1. **Calorie Calculation Formulas**
- **Issue**: Formulas are approximate and not medically validated
- **Impact**: Results may not match professional fitness trackers
- **Note**: Intended for educational purposes; formulas use 3 metrics per activity
- **Example Formulas**:
  - Walking: `0.04 * steps + 50 * distance + 0.8 * duration`
  - Swimming: `0.1 * laps + 0.02 * poolLength * laps + 8 * duration`
  - Running: `60 * distance + 0.1 * HR * (duration / 60)`
  - Cycling: `30 * distance + 0.5 * speed * (duration / 60)`
  - Rowing: `0.02 * strokes + 40 * distance + 0.6 * duration`
  - Yoga: `(cal_per_hr / 60) * duration * Max(1, intensity / 5)`

### 2. **User Weight Not Considered**
- **Issue**: Calorie calculations don't account for user weight/body composition
- **Impact**: Calorie estimates may be significantly inaccurate
- **Workaround**: User can adjust formulas or treat values as relative effort metrics

### 3. **No User Profile Data**
- **Issue**: App doesn't store age, height, weight, or fitness level
- **Impact**: Cannot provide personalized recommendations
- **Mitigation**: Can be added in future versions

### 4. **Activity Metrics Are Hard-Coded**
- **Issue**: Cannot add custom metrics or activities
- **Impact**: Limited flexibility for niche workouts
- **Workaround**: User can adapt existing activity categories

### 5. **No Data Export/Import**
- **Issue**: No CSV, Excel, or PDF export functionality
- **Impact**: Cannot backup or share data outside the app
- **Workaround**: users.json file can be manually copied

### 6. **No Graphical Charts**
- **Issue**: Activity data shown as text list only
- **Impact**: Cannot visualize trends over time
- **Note**: WinForms charting would require additional dependencies

### 7. **Single-User Session**
- **Issue**: Only one user logged in at a time
- **Impact**: Multi-user support limited to sequential login/logout
- **Note**: Sufficient for personal fitness app

### 8. **No Input Range Validation**
- **Issue**: Accepts extremely large metric values (e.g., 1 million steps)
- **Impact**: Unrealistic calorie calculations possible
- **Mitigation**: App displays results; users expected to enter reasonable values

### 9. **No Unit Conversion**
- **Issue**: All distances assumed metric (km), time in minutes
- **Impact**: Imperial users must convert manually
- **Workaround**: Add imperial option in future

### 10. **File Permission Issues**
- **Issue**: If app directory is read-only, data won't persist
- **Impact**: users.json not created or saved
- **Mitigation**: Run as administrator or save to user's Documents folder

---

## 🔧 MISSING DEPENDENCIES

**Current Status**: None. All required dependencies are built-in to .NET 10:
- `System.Text.Json` for JSON serialization
- `System.Security.Cryptography` for SHA-256 hashing
- `System.Windows.Forms` for UI
- `System.Text.RegularExpressions` for username validation

---

## 🚀 USAGE GUIDE

### Getting Started
1. **Launch the app** (double-click executable or F5 in Visual Studio)
2. **Register** a new account:
   - Enter username (letters/numbers only, e.g., "john123")
   - Enter password (exactly 12 chars, e.g., "MyPassword01")
   - Click "Register"
3. **Log In** with your credentials
4. **Set a Goal**: Enter daily calorie target and click "Save Goal"
5. **Record Activities**:
   - Select activity from dropdown
   - Enter the 3 required metrics
   - Click "Record Activity"
6. **View Progress**:
   - Check "Total Calories" at bottom
   - View "Goal Status" to see if goal is achieved
   - Browse activity history in the log

### Keyboard Shortcuts
- None currently; use mouse for all interactions

### File Locations
- **users.json**: App working directory (contains all user data)
- **users.json.backup**: Auto-created backup
- **fitness_tracker_errors.log**: Error log in app directory

---

## 🐛 ERROR HANDLING

The app handles errors gracefully:

1. **Invalid Login**: Shows "Invalid credentials" with attempt counter
2. **Account Lockout**: Shows "Too many failed attempts" with countdown
3. **Invalid Input**: Shows specific field error (e.g., "Invalid Metric 1")
4. **File Errors**: Logs to fitness_tracker_errors.log; app continues
5. **Corrupted Data**: Automatically recovers from backup or starts fresh

---

## 📋 VALIDATION RULES

### Username
- ✅ Must be 1-50 characters
- ✅ Only A-Z, a-z, 0-9
- ❌ No spaces, special characters, or unicode
- ❌ Cannot duplicate existing username

### Password
- ✅ Must be exactly 12 characters
- ✅ At least 1 uppercase letter (A-Z)
- ✅ At least 1 lowercase letter (a-z)
- ❌ No length validation for numbers or special chars (optional)

### Activity Metrics
- ✅ Must be positive numbers (> 0)
- ✅ Accepts decimal values (e.g., 2.5, 10.75)
- ❌ Cannot be zero or negative
- ❌ Cannot be empty

### Goal
- ✅ Must be positive number (> 0)
- ✅ Accepts decimal values (e.g., 300.5)
- ❌ Cannot be zero or negative

---

## 🎨 UI/UX FEATURES

- **Responsive Layout**: Activity card and log side-by-side
- **Color-Coded Messages**: Red for errors, Green for success
- **Emoji Icons**: Visual indicators for buttons and sections (💪, 🔐, ✅, ❌, 🎯, etc.)
- **Placeholder Hints**: Text boxes show requirements in placeholder
- **Tooltips**: Hover over fields for input requirements
- **Button Hover Effects**: Buttons darken on hover for interaction feedback
- **Clear Typography**: 10-14pt Segoe UI font for readability
- **Consistent Styling**: Flat design with modern color palette

---

## 📊 CALORIE FORMULAS REFERENCE

Each activity uses a unique formula combining its 3 metrics:

| Activity | Formula | Example |
|----------|---------|---------|
| Walking | 0.04×steps + 50×km + 0.8×min | 1000 steps, 1km, 30min = 60kcal |
| Swimming | 0.1×laps + 0.02×poolLen×laps + 8×min | 10 laps, 50m, 30min = 241kcal |
| Running | 60×km + 0.1×HR×(min/60) | 5km, 150bpm, 30min = 375kcal |
| Cycling | 30×km + 0.5×speed×(min/60) | 10km, 20kph, 30min = 305kcal |
| Rowing | 0.02×strokes + 40×km + 0.6×min | 1000 strokes, 2km, 20min = 60kcal |
| Yoga | (cal/hr ÷ 60) × min × Max(1, intensity÷5) | 20min, intensity=8, 240cal/hr = 104kcal |

---

## ✨ FUTURE ENHANCEMENTS

Potential improvements for v2.0:
1. ✏️ Add user profile (age, weight, height, fitness level)
2. 📊 Implement chart/graph visualization (weekly trends)
3. 📥 CSV export and import functionality
4. 🌍 Imperial/metric unit conversion
5. 🔔 Daily goal notifications/reminders
6. 🎯 Weekly and monthly summaries
7. 🥇 Achievement badges/milestones
8. 🌙 Dark mode UI theme
9. 📱 Mobile companion app
10. ☁️ Cloud sync for multi-device access

---

## 📝 TECHNICAL NOTES

- **Architecture**: Single-form WinForms with MVC-like separation
- **Database**: JSON file (users.json) in app directory
- **Security**: SHA-256 password hashing (not salted; would improve in v2)
- **Framework**: .NET 10, compiled for Windows desktop
- **Dependencies**: Zero external NuGet packages (uses built-in .NET libraries)
- **UI**: Programmatically created (no designer files used for control instances)

---

## 🔒 Privacy & Security

- ✅ Passwords hashed with SHA-256
- ✅ Data stored locally (no cloud transmission)
- ✅ No telemetry or tracking
- ⚠️ Passwords not salted (consider bcrypt/Argon2 for production)
- ⚠️ No HTTPS needed (local-only app)
- ⚠️ Backup files not encrypted

---

**Last Updated**: 2025-01-17 | **Version**: 1.0 Release
