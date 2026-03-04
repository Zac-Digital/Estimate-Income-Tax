import {test, expect} from '@playwright/test';
import AxeBuilder from "@axe-core/playwright";

test.describe('Pension Contribution Percentage Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/pension-contribution-percentage');
    });

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('Pension Contribution Percentage - GOV.UK');
    });

    test('has content', async ({page}) => {
        const headings = page.getByRole('heading');
        expect(await headings.count()).toEqual(2);
        for (const h of await headings.all()) {
            await expect(h).toBeVisible();
        }
        
        const paragraph = page.getByTestId('pension-contribution__instruction');
        expect(await paragraph.count()).toEqual(1);
        await expect(paragraph).toBeVisible();
        
        const hint = page.getByTestId('pension-contribution__hint');
        expect(await hint.count()).toEqual(1);
        await expect(hint).toBeVisible();
        
        const input = page.getByTestId('pension-contribution__input');
        expect(await input.count()).toEqual(1);
        await expect(input).toBeVisible();
        
        const swap = page.getByTestId('pension-contribution__swap');
        expect(await swap.count()).toEqual(1);
        await expect(swap).toBeVisible();
    });

    test('has continue button', async ({page}) => {
        const button = page.getByRole('button');
        expect(await button.count()).toEqual(1);
        await expect(button).toBeVisible();
        await expect(button).toHaveText('Continue');
    });
    
    test('swap link navigates to alternative pension contribution and clears input', async ({page}) => {
        await page.getByTestId('pension-contribution__input').fill('8');
        await page.getByTestId('pension-contribution__swap').click();
        expect(page.url()).toContain('/pension-contribution-fixed');
        await expect(page.getByTestId('pension-contribution__input')).toBeEmpty();
    });

    test('continue button navigates to next page', async ({page}) => {
        await page.goto('/salary');
        await page.getByTestId('salary__text-input__amount').fill('32768.64');
        await page.getByTestId('salary__radio-set__frequency_Yearly').click();
        await page.getByRole('button').click();
        await page.waitForURL('**/state-pension')
        await page.getByTestId('state-pension__no').click();
        await page.getByRole('button').click();
        await page.waitForURL('**/check-answers')
        await page.goto('/pension-contribution-percentage');
        await page.getByTestId('pension-contribution__input').fill('8');
        await page.getByRole('button').click();
        expect(page.url()).toContain('/check-answers');
    });

    test('passes accessibility checks', async ({page}, testInfo) => {
        const analysis = await new AxeBuilder({ page })
            .withTags(['wcag22aa', 'wcag21aa', 'wcag2aa', 'best-practice'])
            .analyze();

        await testInfo.attach('Accessibility', {
            body: JSON.stringify(analysis, null, 2),
            contentType: 'application/json'
        });

        expect(analysis.violations).toEqual([]);
    });
});