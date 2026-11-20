# Before: a .NET runtime image with Chromium and what it needs. Build from the samples' root:
#   docker build -f src/PuppeteerMigration/Before/Dockerfile -t invoice-before .
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Directory.Build.props Directory.Build.targets ./
COPY src/PuppeteerMigration/invoice.html src/PuppeteerMigration/
COPY src/PuppeteerMigration/Before/ src/PuppeteerMigration/Before/
RUN dotnet publish src/PuppeteerMigration/Before -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:10.0
# The browser is part of the image, with its fonts
RUN apt-get update && apt-get install -y --no-install-recommends chromium fonts-liberation \
    && rm -rf /var/lib/apt/lists/*
ENV PUPPETEER_EXECUTABLE_PATH=/usr/bin/chromium
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Before.dll"]
