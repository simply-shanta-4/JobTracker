\# JobTracker



A full-stack job application tracker. Log in, add the roles you're applying for, and move them across a board as they progress from wishlist to offer. Built with ASP.NET Core and Blazor WebAssembly.



> \*\*Live demo:\*\* coming soon (Azure deployment in progress)



\## Screenshots



| Board | Dashboard |

|-------|-----------|

| !\[Board](docs/screenshots/board.png) | !\[Dashboard](docs/screenshots/dashboard.png) |



\## Features



\- \*\*Authentication:\*\* register and log in with JWT tokens, with a "Remember me" option

\- \*\*Private data:\*\* every user only sees and edits their own applications

\- \*\*Kanban board:\*\* Wishlist, Applied, Interview, Offer and Rejected columns

\- \*\*Full CRUD:\*\* add, edit, move and delete applications, with a confirmation before deleting

\- \*\*Search and filter:\*\* search by company, role, location or notes, and filter by status

\- \*\*Follow-up tracking:\*\* overdue follow-ups are highlighted in red

\- \*\*Dashboard:\*\* totals, response rate, status breakdown and upcoming follow-ups

\- \*\*Friendly UX:\*\* loading spinners, empty states, and error messages with a Retry button

\- \*\*Protected pages:\*\* logged-out visitors are redirected to the login page, and expired sessions log out automatically



\## Tech stack



| Layer | Technology |

|-------|------------|

| Front end | Blazor WebAssembly, Bootstrap |

| Back end | ASP.NET Core Web API (.NET 10) |

| Data | Entity Framework Core, SQLite (local development) |

| Auth | ASP.NET Core Identity, JWT bearer tokens |



\## Project structure



```

JobTracker/

├── src/

│   ├── JobTracker.Api/          ASP.NET Core Web API

│   │   ├── Controllers/         AuthController, ApplicationsController

│   │   ├── Data/                AppDbContext (EF Core + Identity)

│   │   ├── Dtos/                Login and register request/response types

│   │   ├── Models/              JobApplication, ApplicationUser, ApplicationStatus

│   │   ├── Services/            TokenService (creates JWTs)

│   │   └── Migrations/          EF Core migrations

│   └── JobTracker.Web/          Blazor WebAssembly front end

│       ├── Components/          ApplicationCard, StatusColumn, ApplicationForm, StatsCard, ...

│       ├── Layout/              MainLayout, NavMenu

│       ├── Models/              Models shared with the UI

│       ├── Pages/               Dashboard, Board, ApplicationDetail, Login, Register

│       └── Services/            API client, auth state provider, token handler

└── README.md

```



\## API endpoints



| Method | Endpoint | Description | Auth |

|--------|----------|-------------|------|

| POST | `/api/auth/register` | Create an account and return a token | No |

| POST | `/api/auth/login` | Log in and return a token | No |

| GET | `/api/applications` | List your applications | Yes |

| GET | `/api/applications/{id}` | Get one application | Yes |

| POST | `/api/applications` | Create an application | Yes |

| PUT | `/api/applications/{id}` | Update an application | Yes |

| DELETE | `/api/applications/{id}` | Delete an application | Yes |



Protected endpoints expect an `Authorization: Bearer <token>` header.



\## Getting started



\### Prerequisites



\- \[.NET 10 SDK](https://dotnet.microsoft.com/download)

\- The EF Core tool: `dotnet tool install --global dotnet-ef`



\### Run locally



1\. Clone the repository:

```

&#x20;  git clone https://github.com/simply-shanta-4/JobTracker.git

&#x20;  cd JobTracker

```



2\. Set the JWT signing key as a user secret (use your own random string of 32+ characters):

```

&#x20;  dotnet user-secrets init --project src/JobTracker.Api

&#x20;  dotnet user-secrets set "Jwt:Key" "replace-with-your-own-long-random-string" --project src/JobTracker.Api

```



3\. Create the database:

```

&#x20;  dotnet ef database update --project src/JobTracker.Api

```



4\. Start the API (listens on `http://localhost:5192`):

```

&#x20;  dotnet run --project src/JobTracker.Api

```



5\. In a second terminal, start the web app:

```

&#x20;  dotnet run --project src/JobTracker.Web

```



6\. Open the URL printed by the web app, register an account, and add your first application.



If the API runs on a different port, update `apiUrl` in `src/JobTracker.Web/Program.cs`.



\## Design decisions



\- \*\*Service interface for the front end:\*\* pages talk to `IApplicationService`. The first version ran on an in-memory mock, then switched to the real API by changing one line in `Program.cs`.

\- \*\*Per-user data:\*\* every application stores a `UserId` taken from the JWT, and every query filters on it, so one user can never read or change another user's data.

\- \*\*Secrets stay out of Git:\*\* the JWT key lives in user secrets locally and will be an app setting in Azure.

\- \*\*Enums as text:\*\* statuses are stored and sent as readable strings (`"Interview"`) rather than numbers.



\## Roadmap



\- \[ ] Deploy to Azure: Azure SQL Database, App Service (API) and Static Web Apps (front end)

\- \[ ] CI/CD with GitHub Actions

\- \[ ] Azure Functions: email reminders for follow-ups

\- \[ ] Azure Blob Storage: resume uploads

\- \[ ] Application Insights monitoring

\- \[ ] Unit and integration tests (xUnit)



\## Notes



CORS is currently open to any origin for local development. It will be restricted to the deployed front-end URL when the app goes live.

