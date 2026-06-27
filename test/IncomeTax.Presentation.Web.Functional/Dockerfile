FROM mcr.microsoft.com/playwright:v1.61.1-resolute@sha256:6b3d024edb132382b0064c4ca56abec52d5df95b93c63dd61a81d3a37abdef7f
WORKDIR /test

COPY ["package.json", "."]
COPY ["package-lock.json", "."]
RUN npm ci --ignore-scripts

COPY ["playwright.config.ts", "."]
COPY ["tests/", "./tests"]
CMD ["npx", "playwright", "test"]