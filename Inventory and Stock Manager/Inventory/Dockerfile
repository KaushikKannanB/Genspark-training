# # Build Stage
# FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
# WORKDIR /src
# COPY . .
# RUN dotnet restore "Inventory.csproj"
# RUN dotnet publish "Inventory.csproj" -c Release -r linux-x64 --self-contained true \
#     /p:PublishSingleFile=true /p:PublishTrimmed=false -o /app/publish

# # Runtime Stage (just to run the binary)
# FROM debian:bullseye-slim
# WORKDIR /app
# COPY --from=build /app/publish .

# EXPOSE 8080
# ENV ASPNETCORE_URLS="http://+:8080"
# ENTRYPOINT ["./Inventory"]
# ========= Base image for runtime =========
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app


# Install openssl to generate self-signed certificate
RUN apt-get update && apt-get install -y openssl && rm -rf /var/lib/apt/lists/*

# Generate self-signed HTTPS certificate
RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
        -newkey rsa:2048 \
        -keyout /https/aspnetcore.key \
        -out /https/aspnetcore.crt \
        -subj "/CN=localhost" && \
    openssl pkcs12 -export \
        -out /https/aspnetcore.pfx \
        -inkey /https/aspnetcore.key \
        -in /https/aspnetcore.crt \
        -passout pass:password

# Expose HTTP and HTTPS ports
EXPOSE 8080
EXPOSE 8081

# ========= Build Stage =========
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY . .

# Restore and build from same location
RUN dotnet restore "Inventory.csproj"
RUN dotnet build "Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/build


# ========= Publish Stage =========
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ========= Final Runtime Image =========
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Set environment to Development (change to Production for prod)
ENV ASPNETCORE_ENVIRONMENT=Development

# Specify both HTTP and HTTPS Kestrel endpoints
ENV ASPNETCORE_URLS="http://+:8080;https://+:8081"

# Use the generated certificate (you’ll need to configure this in Program.cs too)
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=password
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetcore.pfx

# Start the app
ENTRYPOINT ["dotnet", "Inventory.dll"]