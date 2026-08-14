FROM mcr.microsoft.com/playwright:v1.62.1-resolute@sha256:aebd85bce8056dcdc2269853fd94ea432b6a201da4f0ef125b509489ecd52ddb
WORKDIR /test

COPY ["package.json", "."]
COPY ["package-lock.json", "."]
RUN npm ci --ignore-scripts

COPY ["playwright.config.ts", "."]
COPY ["tests/", "./tests"]
CMD ["npx", "playwright", "test"]