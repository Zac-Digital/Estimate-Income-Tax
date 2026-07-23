# -- Build Node -- #
FROM node:26-alpine@sha256:e88a35be04478413b7c71c455cd9865de9b9360e1f43456be5951032d7ac1a66 AS build-node
WORKDIR /src

COPY ["src/IncomeTax.Presentation.Web.Node/package.json", "."]
COPY ["src/IncomeTax.Presentation.Web.Node/package-lock.json", "."]

RUN npm ci --ignore-scripts

COPY ["src/IncomeTax.Presentation.Web.Node/", "."]

RUN npm run build
# -- Build Node -- #

# -- Build .NET -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:d8ee39817ca03a3757288e83c37ed73cc969a286c603b827c7cbe33add1c2d1c AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:27b6b84beeede74fd16886177d360799c8e4299ceadfbd64eef57bafead7878a
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app

COPY --from=build ["/src/artifacts/publish/IncomeTax.Presentation.Web/release/", "."]

EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #