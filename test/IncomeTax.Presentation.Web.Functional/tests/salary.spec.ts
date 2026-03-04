import {test, expect} from '@playwright/test';
import AxeBuilder from "@axe-core/playwright";

test.describe('Salary Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/salary');
    });

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('Salary - GOV.UK');
    });

    test('has content', async ({page}) => {
        const headers = page.getByRole('heading');
        expect(await headers.count()).toEqual(3);
        for (const h of await headers.all()) {
            await expect(h).toBeVisible();
        }
        
        const hint = page.getByTestId('salary__text-input__amount-hint');
        expect(await hint.count()).toEqual(1);
        await expect(hint).toBeVisible();
        
        const textInput = page.getByTestId('salary__text-input__amount');
        expect(await textInput.count()).toEqual(1);
        await expect(textInput).toBeVisible();
        
        const radios = page.getByRole('radio');
        expect(await radios.count()).toEqual(6);
        for (const r of await radios.all()) {
            await expect(r).toBeVisible();
        }
    });

    test('has continue button', async ({page}) => {
        const button = page.getByRole('button');
        expect(await button.count()).toEqual(1);
        await expect(button).toBeVisible();
        await expect(button).toHaveText('Continue');
    });

    test('continue button navigates to next page', async ({page}) => {
        await page.getByTestId('salary__text-input__amount').fill('32768.64');
        await page.getByTestId('salary__radio-set__frequency_Yearly').click();
        await page.getByRole('button').click();
        expect(page.url()).toContain('/state-pension');
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

test.describe('Salary Page - Error State', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/salary');
    });
    
    test('no errors visible on initial page load', async ({page}) => {
        await expect(page.getByTestId('salary__error-summary__error')).not.toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-decimal'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__frequency'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-decimal'))).not.toBeVisible();
        await (expect(page.getByTestId('F__Error'))).not.toBeVisible();
    });
    
    test('no amount or frequency of salary given shows errors', async ({page}) => {
        await page.getByRole('button').click();
        await expect(page.getByTestId('salary__error-summary__error')).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-decimal'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__frequency'))).toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-decimal'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__radio-set__error__frequency'))).toBeVisible();
    });
    
    test('amount given but no frequency only shows frequency errors', async ({page}) => {
        await page.getByTestId('salary__text-input__amount').fill('32768.64');
        await page.getByRole('button').click();
        await expect(page.getByTestId('salary__error-summary__error')).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__frequency'))).toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__radio-set__error__frequency'))).toBeVisible();
    });
    
    test('frequency given but no amount only shows amount errors', async ({page}) => {
        await page.getByTestId('salary__radio-set__frequency_Yearly').click();
        await page.getByRole('button').click();
        await expect(page.getByTestId('salary__error-summary__error')).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__frequency'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__radio-set__error__frequency'))).not.toBeVisible();
    });
    
    test('amount with too many decimal places given shows relevant error', async ({page}) => {
        await page.getByTestId('salary__text-input__amount').fill('32768.6432');
        await page.getByRole('button').click();
        await expect(page.getByTestId('salary__error-summary__error')).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-decimal'))).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-decimal'))).toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).not.toBeVisible();
    });

    test('amount as a string shows invalid type error', async ({page}) => {
        await page.getByTestId('salary__text-input__amount').fill('Invalid');
        await page.getByRole('button').click();
        await expect(page.getByTestId('salary__error-summary__error')).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__error-summary__error__amount-decimal'))).not.toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-invalid-type'))).toBeVisible();
        await (expect(page.getByTestId('salary__text-input__error__amount-decimal'))).not.toBeVisible();
    });

    test('passes accessibility checks', async ({page}, testInfo) => {
        await page.getByRole('button').click();
        
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