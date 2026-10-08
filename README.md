# csharp-sonarcloud-demo

Small .NET console app used to experiment with SonarCloud: C# analysis rules
and quality gate behavior, via SonarCloud's Automatic Analysis.

It intentionally contains a handful of issues so there's something for
SonarCloud to find on first analysis:

- `UserRepository.cs` — hardcoded credential, SQL built by string
  concatenation (injection risk), empty catch block.
- `OrderCalculator.cs` — deeply nested conditionals / high cognitive
  complexity, magic numbers.
- `ReportBuilder.cs` — duplicated logic between two methods, unused
  variable.

## Requirements

- .NET SDK 8+ (tested with 10)

## Analyze with SonarCloud (Automatic Analysis)

This repo is meant to be imported into SonarCloud with **Automatic
Analysis** enabled — no CI pipeline or token needed, SonarCloud scans the
repo directly on every push. In SonarCloud: *Import project* → pick this
repo → leave Automatic Analysis on.

## Build & run locally

```bash
dotnet build
dotnet run --project src/SonarCloudDemo
```

## Scan locally instead (optional)

If you ever want a CI-based or local scan instead of Automatic Analysis,
install the scanner and run:

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

Note: CI-based analysis and Automatic Analysis are mutually exclusive in
SonarCloud for a given project — don't enable both.
