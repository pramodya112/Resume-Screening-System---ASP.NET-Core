# Resume Screening System

An AI-powered resume screening application built with ASP.NET Core MVC that helps recruiters efficiently evaluate and rank resumes against job postings.

## Features

- **Job Posting Management**: Create and manage job postings with detailed descriptions
- **Resume Upload**: Upload and store resumes (PDF, DOCX, TXT formats)
- **Automated Screening**: AI-powered keyword matching to score resumes against job descriptions
- **Resume Ranking**: Automatically rank resumes by compatibility score
- **Recruiter Dashboard**: Track and manage multiple job postings and candidates
- **Score Visualization**: Visual progress bars showing resume match percentages

## Technology Stack

- **Framework**: ASP.NET Core 9.0 MVC
- **Database**: Entity Framework Core with SQL Server
- **Frontend**: Bootstrap 5, jQuery
- **File Processing**: PDF/DOCX text extraction
- **Architecture**: MVC Pattern with Repository Pattern

## Project Structure

```
ResumeScreeningSystem/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── JobPostingController.cs
│   ├── RecruiterController.cs
│   ├── ResumeController.cs
│   └── ResumeScoreController.cs
├── Models/              # Data models and ViewModels
├── Views/               # Razor views
│   ├── Home/
│   ├── JobPosting/
│   ├── Recruiter/
│   ├── Resume/
│   ├── ResumeScore/
│   └── Shared/
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

## Usage

### 1. Create a Recruiter Profile
- Navigate to Recruiters section
- Add recruiter details (name, email, company)

### 2. Create Job Postings
- Go to Job Postings
- Create new job posting with title and detailed description
- Assign to a recruiter

### 3. Upload Resumes
- Navigate to Resumes section
- Select the target job posting
- Upload resume files (PDF, DOCX, or TXT)

### 4. Analyze Resumes
- Go to Resume Score section
- Select a job posting
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

- **70%+** (Green): Strong match
- **50-69%** (Yellow): Moderate match  
- **Below 50%** (Red): Weak match

## Database Schema

### Main Entities

- **Recruiter**: Manages recruiters
- **JobPosting**: Job posting details with recruiter relationship
- **Resume**: Uploaded resumes linked to job postings
- **ResumeScore**: Scoring results linking resumes to job postings

## Configuration

### File Upload Settings

Default upload path: `wwwroot/uploads`

Supported formats:
- PDF (.pdf)
- Microsoft Word (.docx)
- Plain Text (.txt)

## Future Enhancements

- [ ] Advanced NLP-based scoring with ML.NET
- [ ] Skills extraction and matching
- [ ] Experience level analysis
- [ ] Email notifications for top candidates
- [ ] Bulk resume upload
- [ ] Export results to Excel/PDF
- [ ] Candidate interview scheduling

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
