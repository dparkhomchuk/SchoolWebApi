FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY School.Web/School.Web.csproj ./School.Web
COPY School/School.csproj ./School
COPY School.Database/School.Database.csproj ./School.Database
RUN dotnet restore

# copy everything else and build app
COPY School.Web/. ./School.Web/
WORKDIR /app/School.Web
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/School.Web/out ./
ENTRYPOINT ["dotnet", "School.Web.dll"]