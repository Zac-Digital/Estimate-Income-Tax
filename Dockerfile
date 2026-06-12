# -- Build Node -- #
FROM node:26-alpine@sha256:144769ec3f32e8ee36b3cfde91e82bee25d9367b20f31a151f3f7eea3a2a8541 AS build-node
WORKDIR /src

COPY ["src/IncomeTax.Presentation.Web.Node/package.json", "."]
COPY ["src/IncomeTax.Presentation.Web.Node/package-lock.json", "."]

RUN npm ci --ignore-scripts

COPY ["src/IncomeTax.Presentation.Web.Node/", "."]

RUN npm run build
# -- Build Node -- #

# -- Build .NET -- #
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine@sha256:5c559aa5d99337e400d39ab4fa1f6979d126c29b20939d53658ed38300571e74 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine@sha256:1e37a8236c558ae31bd6bc8144e38e6036b73cf1b0616fe56d79e60babb9d93b
RUN apk add --no-cache icu-libs && apk add --no-cache icu-data-full
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER app
WORKDIR /app

COPY --from=build ["/src/artifacts/publish/IncomeTax.Presentation.Web/release/", "."]

EXPOSE 8080
ENTRYPOINT ["dotnet", "IncomeTax.Presentation.Web.dll"]
# -- Runtime -- #