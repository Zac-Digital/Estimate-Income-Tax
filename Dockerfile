# -- Build -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /
COPY . .
RUN dotnet restore
RUN dotnet publish --no-restore --configuration Release
# -- Build -- #

# -- Runtime -- #
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /
COPY --from=build /artifacts/publish/IncomeTax.Presentation.Web/release/ .
EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #