\# JobTracker



A full-stack job application tracker built with \*\*ASP.NET Core\*\* and \*\*Blazor WebAssembly\*\*. Add the roles you're applying for and move them across a board from wishlist to offer.



\*\*Live demo:\*\* coming soon (Azure deployment in progress)



<!-- Add screenshots once docs/screenshots/board.png and dashboard.png exist, then delete this line and the closing line below.



!\[Board](docs/screenshots/board.png)

!\[Dashboard](docs/screenshots/dashboard.png)



\-->



\## Features



\- Register and log in with JWT authentication, with a "Remember me" option

\- Private data: every user only sees their own applications

\- Kanban board with Wishlist, Applied, Interview, Offer and Rejected columns

\- Add, edit, move and delete applications, with a confirmation before deleting

\- Search by company, role, location or notes, and filter by status

\- Overdue follow-ups highlighted in red

\- Dashboard with totals, response rate, status breakdown and upcoming follow-ups

\- Loading spinners, empty states, and error messages with a Retry button



\## Tech stack



\- \*\*Front end:\*\* Blazor WebAssembly, Bootstrap

\- \*\*Back end:\*\* ASP.NET Core Web API (.NET 10)

\- \*\*Data:\*\* Entity Framework Core with SQLite (Azure SQL planned)

\- \*\*Auth:\*\* ASP.NET Core Identity and JWT bearer tokens



\## Run locally



You need the \[.NET 10 SDK](https://dotnet.microsoft.com/download) and the EF Core tool (`dotnet tool install --global dotnet-ef`).



```

git clone https://github.com/simply-shanta-4/JobTracker.git

cd JobTracker



dotnet user-secrets init --project src/JobTracker.Api

dotnet user-secrets set "Jwt:Key" "replace-with-your-own-long-random-string" --project src/JobTracker.Api



dotnet ef database update --project src/JobTracker.Api

dotnet run --project src/JobTracker.Api

```



In a second terminal:



```

dotnet run --project src/JobTracker.Web

```



Open the URL the web app prints and register an account. The API listens on `http://localhost:5192`. If yours differs, update `apiUrl` in `src/JobTracker.Web/Program.cs`.



\## API



| Method | Endpoint | Description | Auth |

|--------|----------|-------------|------|

| POST | `/api/auth/register` | Create an account | No |

| POST | `/api/auth/login` | Log in | No |

| GET | `/api/applications` | List your applications | Yes |

| GET | `/api/applications/{id}` | Get one application | Yes |

| POST | `/api/applications` | Create an application | Yes |

| PUT | `/api/applications/{id}` | Update an application | Yes |

| DELETE | `/api/applications/{id}` | Delete an application | Yes |



\## Design notes



\- Pages use an `IApplicationService` interface. It ran on an in-memory mock first, then switched to the real API by changing one line.

\- Every application stores the owner's `UserId` from the JWT, and every query filters on it.

\- The JWT signing key lives in user secrets, never in Git.



\## Roadmap



\- \[ ] Deploy to Azure (Azure SQL, App Service, Static Web Apps)

\- \[ ] CI/CD with GitHub Actions

\- \[ ] Azure Functions for follow-up reminder emails

\- \[ ] Unit and integration tests (xUnit)

