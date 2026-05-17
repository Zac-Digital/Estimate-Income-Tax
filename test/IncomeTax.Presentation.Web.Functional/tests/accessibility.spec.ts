import {test, expect, Page, TestInfo} from '@playwright/test';
import AxeBuilder from "@axe-core/playwright";

async function performAccessibilityChecks(page: Page, testInfo: TestInfo) {
    const analysis = await new AxeBuilder({ page })
        .withTags(['wcag22aa', 'wcag21aa', 'wcag2aa', 'best-practice'])
        .analyze();

    await testInfo.attach('Accessibility', {
        body: JSON.stringify(analysis, null, 2),
        contentType: 'application/json'
    });

    expect(analysis.violations).toEqual([]);
}

test.describe('Accessibility', () => {
    test('Index Page', async ({page}, testInfo) => {
        await page.goto('/');
        await performAccessibilityChecks(page, testInfo);
    });
    
    test('Salary Page', async ({page}, testInfo) => {
        await page.goto('/salary');
        await performAccessibilityChecks(page, testInfo);
    });
    
    test('Days Worked Page', async ({page}, testInfo) => {
        await page.goto('/salary');
        await page.getByTestId('amount').fill('128.32');
        await page.getByLabel('Daily').click();
        await page.getByTestId('button').click();
        await expect(page).toHaveURL('/days');
        await performAccessibilityChecks(page, testInfo);
    });

    test('Hours Worked Page', async ({page}, testInfo) => {
        await page.goto('/salary');
        await page.getByTestId('amount').fill('16.8');
        await page.getByLabel('Hourly').click();
        await page.getByTestId('button').click();
        await expect(page).toHaveURL('/hours');
        await performAccessibilityChecks(page, testInfo);
    });
    
    test('State Pension Page', async ({page}, testInfo) => {
        await page.goto('/salary');
        await page.getByTestId('amount').fill('32768.64');
        await page.getByLabel('Yearly').click();
        await page.getByTestId('button').click();
        await expect(page).toHaveURL('/state-pension');
        await performAccessibilityChecks(page, testInfo);
    });
    
    test('Check Answers Page', async ({page}, testInfo) => {
        await page.goto('/salary');
        await page.getByTestId('amount').fill('32768.64');
        await page.getByLabel('Yearly').click();
        await page.getByTestId('button').click();
        await expect(page).toHaveURL('/state-pension');
        await page.getByLabel('No').click();
        await page.getByTestId('button').click();
        await expect(page).toHaveURL('/check-answers');
        await performAccessibilityChecks(page, testInfo);
    })
});