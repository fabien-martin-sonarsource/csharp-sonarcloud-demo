# csharp-sonarcloud-demo

Small .NET console app used to experiment with SonarCloud: C# analysis
rules, quality gate behavior, and NuGet dependency-risk analysis, via
CI-based (GitHub Actions) analysis.

It intentionally contains a handful of issues so there's something for
SonarCloud to find:

- `UserRepository.cs` — hardcoded credential, SQL built by string
  concatenation (injection risk), empty catch block.
- `OrderCalculator.cs` — deeply nested conditionals / high cognitive
  complexity, magic numbers.
- `ReportBuilder.cs` — duplicated logic between two methods, unused
  variable.
- `SonarCloudDemo.csproj` — a direct dependency on `Newtonsoft.Json`
  9.0.1, a known high-severity vulnerability
  ([GHSA-5crp-9r3c-p9vr](https://github.com/advisories/GHSA-5crp-9r3c-p9vr)),
  plus `Microsoft.Extensions.Hosting` to pull in a deeper tree of
  transitive dependencies.

## Requirements

- .NET SDK 8+ (tested with 10)

## Analyze with SonarCloud (CI-based analysis)

This project uses **CI-based analysis** via `.github/workflows/sonarcloud.yml`
rather than Automatic Analysis, because Automatic Analysis does not
resolve NuGet's transitive dependency tree — it can't see anything beyond
what's written in the `.csproj` — so it can't report full dependency
risks.

Setup:

1. In SonarCloud, make sure **Automatic Analysis is disabled** for this
   project (*Administration → Analysis Method*) — the two methods can't
   run on the same project at once.
2. Generate a SonarCloud token and add it as a repo secret named
   `SONAR_TOKEN` (GitHub repo → *Settings → Secrets and variables →
   Actions*).
3. Add a repo secret `SONAR_ORGANIZATION` with your SonarCloud
   organization key.
4. Push to `main` or open a PR — the workflow builds the project and runs
   `dotnet-sonarscanner`.

## Build & run locally

```bash
dotnet build
dotnet run --project src/SonarCloudDemo
```

## Scan locally (optional)

```bash
dotnet tool install --global dotnet-sonarscanner

export SONAR_TOKEN=<your-token>

dotnet sonarscanner begin \
  /k:"<your-project-key>" \
  /o:"<your-organization>" \
  /d:sonar.token="$SONAR_TOKEN" \
  /d:sonar.host.url="https://sonarcloud.io"

dotnet build

dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
```
