# After: a .NET runtime image and fontconfig; no browser. Build from the samples' root:
#   docker build -f src/PuppeteerMigration/After/Dockerfile -t invoice-after .
# (published for the image's platform: the package carries the script engine for every platform, a container needs one)
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src
COPY Directory.Build.props Directory.Build.targets ./
COPY src/PuppeteerMigration/invoice.html src/PuppeteerMigration/
COPY src/PuppeteerMigration/After/ src/PuppeteerMigration/After/
RUN rid=linux-$([ "$TARGETARCH" = "amd64" ] && echo x64 || echo "$TARGETARCH") \
    && dotnet publish src/PuppeteerMigration/After -c Release -r "$rid" --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/runtime:10.0
RUN apt-get update && apt-get install -y --no-install-recommends libfontconfig1 \
    && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "After.dll"]
