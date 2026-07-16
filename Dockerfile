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
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:940f919ae84dd92ccd4aab7686fa5b777870b006c9360351039e16bcaad73d89 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:57bd717ac18ff6c8a39cc0ee4a76c1f15adc46df50434c73eff0c3f1df4c88f0
RUN apk add --no-cache icu-libs icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app

COPY --from=build ["/src/artifacts/publish/IncomeTax.Presentation.Web/release/", "."]

EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #