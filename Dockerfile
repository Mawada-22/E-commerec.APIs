# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first so `restore` is cached and only re-runs when
# dependencies actually change (not on every source edit).
COPY ["E-commerce.Apis/E-commerce.Apis.csproj", "E-commerce.Apis/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["ServicesAbstractions/ServicesAbstractions.csproj", "ServicesAbstractions/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "E-commerce.Apis/E-commerce.Apis.csproj"

COPY . .
RUN dotnet publish "E-commerce.Apis/E-commerce.Apis.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Run as the non-root user the image already provides.
USER $APP_UID

COPY --from=build /app/publish .

# Kestrel listens on 8080 inside the container; TLS is terminated by the host
# platform (Container Apps / App Service / nginx), so no dev cert is needed here.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "E-commerce.Apis.dll"]
