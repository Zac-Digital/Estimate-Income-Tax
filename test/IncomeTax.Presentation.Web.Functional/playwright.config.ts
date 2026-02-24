import {defineConfig, devices} from '@playwright/test';

export default defineConfig({
    testDir: './tests',
    fullyParallel: true,
    forbidOnly: !!process.env.CI,
    retries: process.env.CI ? 1 : 0,
    reporter: [['html'], ['github']],

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
            name: 'Chrome',
            use: {...devices['Desktop Chrome']},
        },
        {
            name: 'Firefox',
            use: {...devices['Desktop Firefox']},
        },
        {
            name: 'Safari',
            use: {...devices['Desktop Safari'], ignoreHTTPSErrors: true},
        },
        // -- Desktop --

        // -- Mobile --
        {
          name: 'Android',
          use: { ...devices['Pixel 5'] },
        },
        {
          name: 'iOS',
          use: { ...devices['iPhone 12'], ignoreHTTPSErrors: true },
        },
        // -- Mobile --
    ],
});
