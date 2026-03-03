# -- Build -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/IncomeTax.Application/*.csproj src/IncomeTax.Application/
COPY src/IncomeTax.Domain/*.csproj src/IncomeTax.Domain/
COPY src/IncomeTax.Presentation.Web/*.csproj src/IncomeTax.Presentation.Web/
RUN dotnet restore src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj
COPY . .
RUN dotnet publish src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj --no-restore --configuration Release --output out
# -- Build -- #

# -- Runtime -- #
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /src
COPY --from=build /src/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #