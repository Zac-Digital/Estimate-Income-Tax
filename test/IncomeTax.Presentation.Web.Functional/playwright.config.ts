import {defineConfig, devices} from '@playwright/test';

export default defineConfig({
    testDir: './tests',
    fullyParallel: true,
    forbidOnly: !!process.env.CI,
    retries: 1,
    reporter: [['html', {open: 'never'}], ['list']],

    use: {
        baseURL: 'https://localhost:8443',
        ignoreHTTPSErrors: true,

        screenshot: 'only-on-failure',
        video: 'on-first-retry',
        trace: 'on-first-retry'
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
