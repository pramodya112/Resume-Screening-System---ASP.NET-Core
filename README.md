# Resume Screening System

An AI-powered resume screening application built with ASP.NET Core MVC that helps recruiters efficiently evaluate and rank resumes against job postings with role-based authentication.

## Features

### Core Features
- **Role-Based Authentication**: Separate login for Admin and Recruiters
- **Job Posting Management**: Create and manage job postings with detailed descriptions
- **Resume Upload**: Upload and store resumes (PDF, DOC, DOCX formats)
- **Automated Screening**: AI-powered keyword matching to score resumes against job descriptions
- **Resume Ranking**: Automatically rank resumes by compatibility score
- **Recruiter Dashboard**: Track and manage job postings and candidates with restricted access
- **Score Visualization**: Visual progress bars showing resume match percentages

### Security & Access Control
- **Admin Access**: Full system control including recruiter management and all job postings
- **Recruiter Access**: Limited to viewing and managing only their own job postings and resumes
- **Session-Based Authentication**: Secure login system with session management
- **Access Denied Protection**: Prevents unauthorized access to admin-only features

## Technology Stack

- **Framework**: ASP.NET Core 9.0 MVC
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: Custom Session-Based Authentication
- **Frontend**: Bootstrap 5, jQuery
- **File Processing**: PDF/DOCX text extraction
- **Architecture**: MVC Pattern with Custom Authorization Filters

## Project Structure

```
ResumeScreeningSystem/
├── Controllers/          # MVC Controllers
│   ├── AccountController.cs      # Login/Logout & Authentication
│   ├── HomeController.cs
│   ├── JobPostingController.cs   # Admin only
│   ├── RecruiterController.cs    # Admin only
│   ├── ResumeController.cs       # Role-based access
│   └── ResumeScoreController.cs  # Role-based access
├── Models/              # Data models and ViewModels
│   ├── Recruiter.cs
│   ├── JobPosting.cs
│   ├── Resume.cs
│   ├── ResumeScore.cs
│   └── LoginViewModel.cs
├── Filters/             # Custom Authorization Filters
│   ├── AuthorizeSessionAttribute.cs
│   └── AuthorizeAdminAttribute.cs
├── Views/               # Razor views
│   ├── Account/         # Login & Access Denied pages
│   ├── Home/
│   ├── JobPosting/
│   ├── Recruiter/
│   ├── Resume/
│   ├── ResumeScore/
│   └── Shared/          # Layout with role-based navigation
├── Data/                # Database context
├── Services/            # Business logic services
├── Migrations/          # EF Core migrations
├── wwwroot/            # Static files (CSS, JS, uploads)
└── Properties/         # Application properties
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ResumeScreeningSystem
   ```

2. **Update connection string**
   
   Edit `appsettings.json` to configure your database connection:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ResumeScreeningDB;Trusted_Connection=true;"
     }
   }
   ```

3. **Apply migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   
   Navigate to `https://localhost:5005` in your browser

## User Roles & Access

### Admin
- **Username**: `admin`
- **Password**: `Admin@123`
- **Permissions**:
  - Manage all recruiters (Create, Edit, Delete)
  - Manage all job postings
  - View all resumes across all recruiters
  - Analyze and score all resumes
  - Full system access

### Recruiter
- **Login**: Use email assigned by admin
- **Password**: Set by admin during recruiter creation
- **Permissions**:
  - View only their own job postings
  - Upload resumes only to their job postings
  - View and download only resumes for their jobs
  - Analyze and score only their own resumes
  - Cannot access admin features (Recruiter Management, Job Posting Creation)

## Usage

### 1. Login to the System
- Navigate to the login page
- **Admin**: Use `admin` / `Admin@123`
- **Recruiter**: Use your email and password provided by admin

### 2. Admin: Create Recruiters
- Navigate to Recruiters section (Admin only)
- Click "Add New Recruiter"
- Enter recruiter name, email, and password
- Recruiter can now login with their email

### 3. Admin: Create Job Postings
- Go to Job Postings section (Admin only)
- Create new job posting with title and detailed description
- Assign to a recruiter

### 4. Upload Resumes (Admin or Recruiter)
- Navigate to Resumes section
- Select the target job posting (recruiters see only their jobs)
- Click "Upload Resume"
- Upload resume files (PDF, DOC, DOCX)

### 5. Analyze Resumes
- Go to Resume Score section
- Select a job posting (filtered by role)
- Click "Analyze All Resumes"
- View ranked results with match percentages

## How the Scoring Works

The system uses keyword-based matching:

1. **Extraction**: Job descriptions and resumes are parsed into individual words
2. **Normalization**: Text is converted to lowercase and cleaned
3. **Matching**: Counts matching keywords between job description and resume
4. **Scoring**: `Score = (Matching Keywords / Total Job Keywords) × 100`
5. **Ranking**: Resumes are automatically ranked by score (highest first)

### Score Interpretation

- **70%+** (Green): Strong match - Highly qualified candidate
- **50-69%** (Yellow): Moderate match - Consider for interview
- **Below 50%** (Red): Weak match - May not meet requirements

## Database Schema

### Main Entities

- **Recruiter**: Manages recruiters with authentication credentials
  - Id, RecruiterName, Email, password
  
- **JobPosting**: Job posting details with recruiter relationship
  - Id, JobTitle, JobDescription, RecruiterId
  
- **Resume**: Uploaded resumes linked to job postings
  - Id, FileName, FilePath, ExtractedText, JobPostingId
  
- **ResumeScore**: Scoring results linking resumes to job postings
  - Id, ResumeId, JobPostingId, Score, AnalysisSummary

## Security Features

### Authentication
- Session-based authentication with secure session management
- Password protection for all accounts
- Automatic session timeout after 30 minutes of inactivity

### Authorization
- Custom authorization filters (`AuthorizeSession`, `AuthorizeAdmin`)
- Role-based access control at controller level
- Data filtering ensures recruiters only see their own data

### Data Protection
- Recruiters cannot access other recruiters' data
- Admin-only routes are protected from recruiter access
- File uploads are validated and securely stored

## Configuration

### File Upload Settings

Default upload path: `wwwroot/uploads`

Supported formats:
- PDF (.pdf)
- Microsoft Word (.doc, .docx)

### Session Configuration

```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

## Default Credentials

**Admin Account** (Hardcoded):
- Username: `admin`
- Password: `Admin@123`

**Recruiter Accounts**:
- Created by admin through the system
- Login using assigned email and password

## Troubleshooting

### Cannot Login
- Verify credentials are correct
- For recruiters, check with admin for correct email and password
- Clear browser cache and cookies

### Access Denied Error
- Recruiters trying to access admin-only pages will see Access Denied
- Ensure you're logged in with the correct role

### File Upload Issues
- Ensure `wwwroot/uploads` folder exists
- Check file format is PDF, DOC, or DOCX
- Verify file size is under 5MB

## Future Enhancements

- [ ] Advanced NLP-based scoring with ML.NET
- [ ] Password reset functionality
- [ ] Email notifications for recruiters
- [ ] Skills extraction and matching
- [ ] Experience level analysis
- [ ] Bulk resume upload
- [ ] Export results to Excel/PDF
- [ ] Candidate interview scheduling
- [ ] Two-factor authentication
- [ ] Activity logs and audit trail

## API Endpoints

### Authentication
- `GET /Account/Login` - Login page
- `POST /Account/Login` - Process login
- `GET /Account/Logout` - Logout user
- `GET /Account/AccessDenied` - Access denied page

### Admin Only
- `/Recruiter/*` - Recruiter management
- `/JobPosting/*` - Job posting management

### Authenticated Users
- `/Resume/*` - Resume management (filtered by role)
- `/ResumeScore/*` - Score analysis (filtered by role)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues and questions, please open an issue in the GitHub repository.

## Acknowledgments

- Built with ASP.NET Core MVC
- Bootstrap for responsive UI
- Entity Framework Core for data access
- Custom session-based authentication system
- Role-based authorization implementation

## Version History

### v2.0.0 (Current)
- Added role-based authentication (Admin & Recruiter)
- Implemented session-based login system
- Added custom authorization filters
- Data isolation for recruiters
- Enhanced security features

### v1.0.0
- Initial release with basic resume screening functionality
