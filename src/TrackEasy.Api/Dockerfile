FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS base

USER root
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    ca-certificates curl wget gnupg \
    fonts-liberation libasound2 libatk1.0-0 libatk-bridge2.0-0 libcups2 libdbus-1-3 \
    libdrm2 libgbm1 libgtk-3-0 libnspr4 libnss3 libx11-6 \
    libxcomposite1 libxdamage1 libxext6 libxfixes3 libxrandr2 \
    libxss1 libxtst6 xdg-utils && \
    # Add Google Chrome repository and install the stable channel
    curl -fsSL https://dl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /usr/share/keyrings/google-chrome.gpg && \
    echo "deb [arch=amd64 signed-by=/usr/share/keyrings/google-chrome.gpg] https://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list && \
    apt-get update && \
    apt-get install -y --no-install-recommends google-chrome-stable && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TrackEasy.Api/TrackEasy.Api.csproj", "src/TrackEasy.Api/"]
COPY ["src/TrackEasy.Infrastructure/TrackEasy.Infrastructure.csproj", "src/TrackEasy.Infrastructure/"]
COPY ["src/TrackEasy.Application/TrackEasy.Application.csproj", "src/TrackEasy.Application/"]
COPY ["src/TrackEasy.Domain/TrackEasy.Domain.csproj", "src/TrackEasy.Domain/"]
COPY ["src/TrackEasy.Shared.Domain.Abstractions/TrackEasy.Shared.Domain.Abstractions.csproj", "src/TrackEasy.Shared.Domain.Abstractions/"]
COPY ["src/TrackEasy.Shared.Exceptions/TrackEasy.Shared.Exceptions.csproj", "src/TrackEasy.Shared.Exceptions/"]
COPY ["src/TrackEasy.Mails.Abstractions/TrackEasy.Mails.Abstractions.csproj", "src/TrackEasy.Mails.Abstractions/"]
COPY ["src/TrackEasy.Shared.Application.Abstractions/TrackEasy.Shared.Application.Abstractions.csproj", "src/TrackEasy.Shared.Application.Abstractions/"]
COPY ["src/TrackEasy.Shared.Pagination.Abstractions/TrackEasy.Shared.Pagination.Abstractions.csproj", "src/TrackEasy.Shared.Pagination.Abstractions/"]
COPY ["src/TrackEasy.Pdf.Abstractions/TrackEasy.Pdf.Abstractions.csproj", "src/TrackEasy.Pdf.Abstractions/"]
COPY ["src/TrackEasy.Shared.Files.Abstractions/TrackEasy.Shared.Files.Abstractions.csproj", "src/TrackEasy.Shared.Files.Abstractions/"]
COPY ["src/TrackEasy.Mails/TrackEasy.Mails.csproj", "src/TrackEasy.Mails/"]
COPY ["src/TrackEasy.Shared.Infrastructure/TrackEasy.Shared.Infrastructure.csproj", "src/TrackEasy.Shared.Infrastructure/"]
COPY ["src/TrackEasy.Shared.Pagination.Infrastructure/TrackEasy.Shared.Pagination.Infrastructure.csproj", "src/TrackEasy.Shared.Pagination.Infrastructure/"]
COPY ["src/TrackEasy.Pdf/TrackEasy.Pdf.csproj", "src/TrackEasy.Pdf/"]
COPY ["src/TrackEasy.Shared.Files/TrackEasy.Shared.Files.csproj", "src/TrackEasy.Shared.Files/"]
RUN dotnet restore "src/TrackEasy.Api/TrackEasy.Api.csproj"
COPY . .
WORKDIR "/src/src/TrackEasy.Api"
RUN dotnet build "./TrackEasy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrackEasy.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrackEasy.Api.dll"]
