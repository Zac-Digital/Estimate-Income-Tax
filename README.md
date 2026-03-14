# Estimate Income Tax

[![Functional Testing](https://github.com/Zac-Digital/Estimate-Income-Tax/actions/workflows/functional-testing.yml/badge.svg?branch=master)](https://github.com/Zac-Digital/Estimate-Income-Tax/actions/workflows/functional-testing.yml)

This codebase is a faithful recreation of the [Estimate Income Tax GOV.UK Service](https://www.gov.uk/estimate-income-tax).

The purpose of this web application is for me to consolidate my skills and serves as a reference for myself on how to consistently develop certain things, e.g., a .NET web app with a clean architecture, Playwright E2E tests, Dockerfiles and whatever else.

It uses the [GOV.UK FrontEnd](https://github.com/alphagov/govuk-frontend) and follows the [GOV.UK Design System (GDS)](https://design-system.service.gov.uk/) as reasonably as I can.

## Structure

```
.
|- certs
|- src
    |- IncomeTax.Application
    |- IncomeTax.Domain
    |- IncomeTax.Presentation.Web
    |- IncomeTax.Presentation.Web.Node
|- test
    |- IncomeTax.Application.Test.Integration
    |- IncomeTax.Presentation.Web.Functional
```

### certs

This folder contains some generated certificates and keys for running the Docker container locally with an Nginx proxy as the ingress.

### src

This follows a reasonable layered clean architecture approach:

- `IncomeTax.Application` - Contains business logic
- `IncomeTax.Domain` - Contains POCOs (Plain Old CLR Objects) and Constants, used to represent data passing between architectural layers.
- `IncomeTax.Presentation.Web` - Contains the user-facing web application; the entrypoint and frontend.
- `IncomeTax.Presentation.Web.Node` - Contains a build system that compiles the GOV.UK frontend and other SCSS/JS that populates `wwwroot` in `IncomeTax.Presentation.Web`.

### test

- `IncomeTax.Application.Test.Integration` - This is where the integration tests are located.
- `IncomeTax.Presentation.Web.Functional` - This is where the functional / E2E tests are located. Uses Playwright.

## Building & Running

### Native

From the root directory, execute `dotnet run --project src/IncomeTax.Presentation.Web/IncomeTax.Presentation.Web.csproj`

Or use your IDEs fancy pants built in way of running .NET projects.

The entry point in your browser is https://localhost:7234/

### Docker

If you have Docker installed, from the root directory execute `docker compose up`.

The entry point in your browser is https://localhost:8443/

## Testing

You must have Docker installed to run the functional tests. You can also run it natively but you'll have to change the endpoint in the Playwright config.

To run the functional tests properly:

- Ensure the web application is running with `docker compose up` from the root directory
- In another terminal, `cd` into `test/IncomeTax.Presentation.Web.Functional`
- Ensure you are up to date, execute `npm i` and then `npx playwright install`
- To run the tests, execute `npx playwright test`

To run the integration tests, it's simply `dotnet test` from the root directory.
