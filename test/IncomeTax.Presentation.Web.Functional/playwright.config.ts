import {defineConfig, devices} from '@playwright/test';

export const BASE_URL: string = 'http://localhost:5062';

export default defineConfig({
    testDir: './tests',
    fullyParallel: true,
    workers: '100%',
    forbidOnly: !!process.env.CI,
    retries: 1,
    reporter: [['html', {open: 'never'}], [process.env.CI ? 'github' : 'list']],

    use: {
        baseURL: BASE_URL,

        screenshot: 'only-on-failure',
        video: 'on-first-retry',
        trace: 'on-first-retry'
    },

    webServer: {
        command: `\
            npm --prefix ../../src/IncomeTax.Presentation.Web.Node ci --ignore-scripts && \
            npm --prefix ../../src/IncomeTax.Presentation.Web.Node run build && \
            dotnet run --project ../../src/IncomeTax.Presentation.Web\
        `,
        stdout: 'pipe',
        stderr: 'pipe',
        url: BASE_URL,
        reuseExistingServer: !process.env.CI,
    },

    projects: [
        // -- Desktop --
        {
            name: 'Google Chrome',
            use: {...devices['Desktop Chrome']},
        },
        {
            name: 'Microsoft Edge',
            use: {...devices['Desktop Edge']},
        },
        {
            name: 'Mozilla Firefox',
            use: {...devices['Desktop Firefox']},
        },
        {
            name: 'Apple Safari',
            use: {...devices['Desktop Safari']},
        },
        // -- Desktop --

        // -- Mobile --
        {
            name: 'Android Phone',
            use: {...devices['Pixel 7']},
        },
        {
            name: 'iOS',
            use: {...devices['iPhone 15']},
        },
        // -- Mobile --

        // -- Tablet --
        {
            name: 'Android Tablet',
            use: {...devices['Galaxy Tab S9']},
        },
        {
            name: 'iPadOS',
            use: {...devices['iPad (gen 11)']},
        },
        // -- Tablet --
    ],
});
