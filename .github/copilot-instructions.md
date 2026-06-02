# Copilot Instructions

## Project Guidelines
- User prefers using Dapper for the database switch and wants step-by-step guidance.
- User prefers storing the SQL Server connection string in user secrets instead of appsettings.json.
- Database name should be spelled "ActivityAssistent" (with double s).
- User prefers integer identity primary keys (no GUIDs) and column names like Company.Name instead of CompanyName.
- User decided to drop/remove SubNr from the database model (not needed).

## Language Preferences
- User wants conversation in Dutch but all UI text and errors in English.

## Testing Requirements
- Tests should use xUnit and UI/device tests target the .NET MAUI app.
- Definition of Done includes:
  - Acceptance criteria met
  - Documentation completed
  - No known defects
  - Unit/integration/functional tests passed
  - Design patterns applied
  - No build failures
  - Coding/architecture standards satisfied
  - Code check-in

## Setup Reminders
- For running MAUI UI tests, ensure the following environment variables are set:
  - `ANDROID_HOME`/`SDK_ROOT` is configured.
  - `adb` is included in the `PATH`.
  - `JAVA_HOME` is set correctly.