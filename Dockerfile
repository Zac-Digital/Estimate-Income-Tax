# -- Build Node -- #
FROM node:26-alpine@sha256:ef24c5053d50fdc3e4e56eb4e7ddb7861874ab0fdc797046ba897581deb8e868 AS build-node
WORKDIR /src

COPY ["src/IncomeTax.Presentation.Web.Node/package.json", "."]
COPY ["src/IncomeTax.Presentation.Web.Node/package-lock.json", "."]

RUN npm ci --ignore-scripts

COPY ["src/IncomeTax.Presentation.Web.Node/", "."]

RUN npm run build
# -- Build Node -- #

# -- Build .NET -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:230c3fbae0ceedc70bca04eed2bfb5694e8b248cadbb81010f41b7a993601f55 AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/IncomeTax.Application/IncomeTax.Application.csproj", "src/IncomeTax.Application/"]
COPY ["src/IncomeTax.Domain/IncomeTax.Domain.csproj", "src/IncomeTax.Domain/"]
COPY ["src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj", "src/IncomeTax.Presentation.Web/"]

RUN dotnet restore src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj

COPY ["src/IncomeTax.Application/", "./src/IncomeTax.Application/"]
COPY ["src/IncomeTax.Domain/", "./src/IncomeTax.Domain/"]
COPY ["src/IncomeTax.Presentation.Web/", "./src/IncomeTax.Presentation.Web/"]

COPY --from=build-node ["/IncomeTax.Presentation.Web/wwwroot/", "./src/IncomeTax.Presentation.Web/wwwroot/"]

RUN dotnet publish src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj --no-restore --configuration Release
# -- Build .NET -- #

# -- Runtime -- #
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:819bd3649d8bf53f35527c68d62e17b348ef6f24ae73073f497f00a878348062
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app

COPY --from=build ["/src/artifacts/publish/IncomeTax.Presentation.Web/release/", "."]

EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #