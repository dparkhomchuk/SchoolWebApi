FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY School.sln .
COPY School/School.csproj ./School/
COPY School.Database/School.Database.csproj ./School.Database/
COPY School.Database.Integration.Tests/School.Database.Integration.Tests.csproj ./School.Database.Integration.Tests/
COPY School.UnitTest/School.UnitTest.csproj ./School.UnitTest/
COPY School.Rabbit/School.Rabbit.csproj ./School.Rabbit/
COPY School.Web/School.Web.csproj ./School.Web/
RUN dotnet restore

# copy everything else and build app
COPY School.Web/* ./School.Web/
COPY School.Database/* ./School.Database/
COPY School.Rabbit/* ./School.Rabbit/
COPY School/* ./School/
WORKDIR /app/School.Web
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/School.Web/out ./
ENTRYPOINT ["dotnet", "School.Web.dll"]