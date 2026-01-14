# ASP.NET Core Web Application template
ASP.Net core using .net 8, I believe.

**Project purpose:**
To provide web developers a quick base to develop their websites from and save time from having to create the common things among most websites, so they can give more time to the more unique features for their website.

**Working pages featured are:**

-> Log in page

-> Sign up page

-> Forgot password \& change password pages

-> Dashboard page (With only the session of the user's username in the top right corner.)

**Features:**

-> Dark and light theme toggle on the navbar

-> Comments in the code to make it clear what each function does and to make the code more maintainable by any developer. And for future edits.

**Video walk through of template:**

\[N/A]


**How to run this template (In visual studio and using SQLite):**

-> If visual studio is installed, run the 'ASPlogInV2.sln' file.

-> Then in the right side, open the 'Model' folder, click 'DbConnectorSQLite.cs' then in line 8 inside "(@"data source=C:\\SQLiteDb.db")" Replace the inside of the bracket like this "(@"data source=C:\\\[filepath]\\SQLiteDb.db")".
![WhichLineInDbConnectorSQLite.csToChange](docs/img/DBdataSource.png)

*(Tip in your file explorer, click the right corner of the bar to the left of the search bar before copying the file path so you can past it in the 'DbConnectorSQLite.cs')*
*(Black bars to redact personal information.)*
![CopyFilepathFromFileExplorer](docs/img/HowToCopyFilePath.png)


**Frame work used:**

-> ASP.NET CORE, using .NET 8.


**Database used:**

-> SQLite (Go to the DbConnectorSQLite.cs to change the database to a different one.)


**Code libraries/NuGet packages used:**

-> Microsoft.VisualStudio.Web.CodeGeneration.Design

-> Microsoft.EntityFrameworkCore.Tools

-> Microsoft.EntityFrameworkCore.Sqlite

-> Microsoft.AspNetCore.Session

-> BCrypt.Net-Next

-> Bootstrap
