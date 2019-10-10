FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["School.Web/School.Web.csproj", "School.Web/"]
COPY ["School/School.csproj", "School/"]
COPY ["School.Database/School.Database.csproj", "School.Database/"]
RUN dotnet restore "School.Web/School.Web.csproj"
COPY . .
WORKDIR "/src/School.Web"
RUN dotnet build "School.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "School.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "School.Web.dll"]