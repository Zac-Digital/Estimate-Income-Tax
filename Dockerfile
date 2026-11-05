# -- Build Node -- #
FROM node:26-alpine@sha256:c6ad9e9c1d80f6eb607c26b4b0a132ff7a2344fad1c78ea3baa2a7d4c8de6d94 AS build-node
WORKDIR /src

COPY ["src/IncomeTax.Presentation.Web.Node/package.json", "."]
COPY ["src/IncomeTax.Presentation.Web.Node/package-lock.json", "."]

RUN npm ci --ignore-scripts

COPY ["src/IncomeTax.Presentation.Web.Node/", "."]

RUN npm run build
# -- Build Node -- #

# -- Build .NET -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:cc02b814237a76c4735fb6eeee87181597b5e56f2ce458a435e527674d91f4d7 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:254fe017495996d3abcc487ffacdcce2b73affb380d3af87af12c8dff41fe0b0
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app

COPY --from=build ["/src/artifacts/publish/IncomeTax.Presentation.Web/release/", "."]

EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #