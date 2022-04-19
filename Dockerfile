# author: Aleksandr Kovalyov
# build
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./A.Kovalyov-Zadanie1-TC/A.Kovalyov-Zadanie1-TC.csproj"
RUN dotnet publish "./A.Kovalyov-Zadanie1-TC/A.Kovalyov-Zadanie1-TC.csproj" -c release -o /app --no-restore

# stage
FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine
WORKDIR /app
COPY --from=build /app ./

EXPOSE 4000
ENTRYPOINT ["dotnet", "A.Kovalyov-Zadanie1-TC.dll"]
