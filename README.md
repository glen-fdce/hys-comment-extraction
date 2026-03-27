# CommentAnalysis

A tool for scraping and analyzing comments from BBC News "Have Your Say" (HYS) sections and storing them in a SQLite database for further sentiment analysis.

## Overview

CommentAnalysis is a .NET application designed to collect comments from BBC News HYS posts and save them in a structured database format. This allows for subsequent sentiment analysis and other forms of text processing to understand public opinion on various topics covered by BBC News.

## Features

- **Comment Scraping**: Automatically fetches all comments and their replies from BBC News HYS posts
- **Pagination Support**: Handles paginated results to ensure all comments are captured
- **User Tracking**: Captures and preserves user identities across comments
- **Comment Relationships**: Maintains parent-child relationships between comments and replies
- **Rating Information**: Preserves positive and negative ratings for each comment
- **SQLite Storage**: Stores all data in a portable SQLite database
- **Entity Framework Core**: Uses EF Core for database operations

## Project Structure

- **GetComments**: Main project containing the scraping logic
  - `GetComments.cs`: Core application logic for fetching and processing comments
  - `Services/BBCCommunicationService.cs`: Handles HTTP communication with the BBC API
  - `Services/DatabaseService.cs`: Manages database operations
  - `Entities/`: Contains database entity models
  - `Models/Dtos/`: Data transfer objects for the BBC API responses
  - `Data/`: Database context and configuration
  - `Options/`: Configuration options classes
  - `Extensions/`: Extension methods for dependency injection
  - `Migrations/`: EF Core database migrations

- **GetSchema**: Secondary project for schema generation

## Setup and Configuration

### Prerequisites

- .NET 9.0 SDK
- Access to BBC News HYS API (requires API key)

### Configuration

Edit the `appsettings.json` file to configure the application:

```json
{
  "HYS": {
    "ApiKey": "your-api-key", // Leave default value
    "BaseUrl": "https://www.bbc.co.uk", // Leave default value
    "UrlPrefix": "/wc-data/container/comments" // Leave default value
    "ForumId": "your-forum-id", // Change to the forum ID of the HYS section you want to scrape
  },
  "Database": {
    "Type": "sqlite",
    "ConnectionString": "Data Source=path/to/your/comments.db" // Change to your desired SQLite database path
  }
}
```

- **ApiKey**: Your BBC News API key (leave as default)
- **BaseUrl**: Base URL for BBC News (leave as default)
- **UrlPrefix**: Endpoint path for the comments API (leave as default)
- **ForumId**: The forum ID of the HYS section you want to scrape (change)
- **ConnectionString**: SQLite database path (change)

## Usage

### Database Initialization

If you've changed the database connection string or are running the application for the first time, you need to run the database migrations to create the database schema:

```bash
# Navigate to the GetComments project directory
cd GetComments

# Run EF Core migrations
dotnet ef database update
```

If you encounter any issues with the EF Core tools, you may need to install them first:

```bash
dotnet tool install --global dotnet-ef
```

### Running the Application

Run the application with:

```bash
dotnet run --project GetComments
```

The application will:
1. Connect to the specified BBC News HYS section
2. Fetch all main comments and their replies
3. Store all data in the SQLite database
4. Display progress in the console with color-coded output

## Database Schema

The database consists of two main tables:

### Users Table
- **Id**: Primary key (BBC user ID)
- **DisplayName**: User's display name

### Comments Table
- **Id**: Primary key (BBC comment ID)
- **CreatedDate**: Comment timestamp
- **UserId**: Foreign key to Users table
- **CommentType**: Type of comment (Main, ReplyToMain, ReplyToReply)
- **ParentCommentId**: ID of the parent comment
- **InReplyToId**: ID of the comment being replied to (for replies)
- **RatingPositive**: Number of positive ratings/likes
- **RatingNegative**: Number of negative ratings/dislikes
- **Text**: Comment content

## Sentiment Analysis

The primary purpose of this tool is to collect data for sentiment analysis. After scraping, the SQLite database can be used with separate processes to:

- Analyze sentiment across comments
- Identify common topics and themes
- Track opinion changes over time
- Generate visualizations of public sentiment
- Identify influential commenters and popular viewpoints

## License

This project is licensed under the BSD License - see the [LICENSE](./LICENSE) file for details.