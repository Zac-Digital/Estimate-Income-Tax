import {test, expect} from '@playwright/test';

test.describe('Core End-To-End', () => {
    test('Required Only', async ({page}) => {
        await test.step('Start from Home Page', async () => {
            await page.goto('/');
            await page.getByTestId('button-start-now').click();
            await expect(page).toHaveURL('/salary');
        });
        
        await test.step('Enter Salary Information', async () => {
            await page.getByTestId('amount').fill('32768.64');
            await page.getByLabel('Yearly').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/state-pension');
        });
        
        await test.step('Enter State Pension Information', async () => {
            await page.getByLabel('No').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/check-answers');
        });
        
        await test.step('Check Answers Page Contains Information', async () => {
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Gross income' }))
                .toContainText('£32,768.64 a year');
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Over State Pension age' }))
                .toContainText('No');
        });
    });
    
    test('Required + Optional Days Worked / Week', async ({page}) => {
        await test.step('Start from Home Page', async () => {
            await page.goto('/');
            await page.getByTestId('button-start-now').click();
            await expect(page).toHaveURL('/salary');
        });

        await test.step('Enter Salary Information', async () => {
            await page.getByTestId('amount').fill('128.32');
            await page.getByLabel('Daily').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/days');
        });
        
        await test.step('Enter Days Worked Information', async () => {
            await page.getByTestId('days-worked').fill('5');
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/state-pension');
        });

        await test.step('Enter State Pension Information', async () => {
            await page.getByLabel('No').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/check-answers');
        });

        await test.step('Check Answers Page Contains Information', async () => {
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Gross income' }))
                .toContainText('£128.32 a day');
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Days worked a week' }))
                .toContainText('5');
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Over State Pension age' }))
                .toContainText('No');
        });
    });

    test('Required + Optional Hours Worked / Week', async ({page}) => {
        await test.step('Start from Home Page', async () => {
            await page.goto('/INVALID');
            await page.getByTestId('button-start-now').click();
            await expect(page).toHaveURL('/salary');
        });

        await test.step('Enter Salary Information', async () => {
            await page.getByTestId('amount').fill('16.8');
            await page.getByLabel('Hourly').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/hours');
        });

        await test.step('Enter Hours Worked Information', async () => {
            await page.getByTestId('hours-worked').fill('40');
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/state-pension');
        });

        await test.step('Enter State Pension Information', async () => {
            await page.getByLabel('No').click();
            await page.getByTestId('button').click();
            await expect(page).toHaveURL('/check-answers');
        });

        await test.step('Check Answers Page Contains Information', async () => {
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Gross income' }))
                .toContainText('£16.80 an hour');
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Hours worked a week' }))
                .toContainText('40');
            await expect(page.locator('.govuk-summary-list__row', { hasText: 'Over State Pension age' }))
                .toContainText('No');
        });
    });
});