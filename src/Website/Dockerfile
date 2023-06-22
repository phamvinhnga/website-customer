#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Website/Website.csproj", "src/Website/"]
COPY ["src/Website.Biz/Website.Biz.csproj", "src/Website.Biz/"]
COPY ["src/Website.Entity/Website.Entity.csproj", "src/Website.Entity/"]
COPY ["src/Website.Shared/Website.Shared.csproj", "src/Website.Shared/"]
RUN dotnet restore "src/Website/Website.csproj"
COPY . .
WORKDIR "/src/src/Website"
RUN dotnet build "Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Website.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Website.dll"]