import {test, expect} from '@playwright/test';

test.describe('State Pension Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/state-pension');
    });

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('State Pension - GOV.UK');
    });

    test('has content', async ({page}) => {
        const header = page.getByRole('heading');
        expect(await header.count()).toEqual(1);
        await expect(header).toBeVisible();
        
        const radios = page.getByRole('radio');
        expect(await radios.count()).toEqual(2);
        for (const r of await radios.all()) {
            await expect(r).toBeVisible();
        }
        
        const detail = page.getByTestId('state-pension__details-summary');
        expect(await detail.count()).toEqual(1);
        await expect(detail).toBeVisible();
    });

    test('has continue button', async ({page}) => {
        const button = page.getByRole('button');
        expect(await button.count()).toEqual(1);
        await expect(button).toBeVisible();
        await expect(button).toHaveText('Continue');
    });

    test('continue button navigates to next page', async ({page}) => {
        await page.goto('/salary');
        await page.getByTestId('salary__text-input__amount').fill('32768.64');
        await page.getByTestId('salary__radio-set__frequency_Yearly').click();
        await page.getByRole('button').click();
        await page.waitForURL('**/state-pension')
        await page.getByTestId('state-pension__no').click();
        await page.getByRole('button').click();
        expect(page.url()).toContain('/check-answers');
    });
});

test.describe('State Pension Page - Error State', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/state-pension');
    });

    test('no errors visible on initial page load', async ({page}) => {
        await expect(page.getByTestId('state-pension__error-summary__error')).not.toBeVisible();
        await expect(page.getByTestId('state-pension__error-summary__error__no-selection')).not.toBeVisible();
        await expect(page.getByTestId('state-pension__error')).not.toBeVisible();
    });

    test('no option selected shows errors', async ({page}) => {
        await page.getByRole('button').click();
        await expect(page.getByTestId('state-pension__error-summary__error')).toBeVisible();
        await expect(page.getByTestId('state-pension__error-summary__error__no-selection')).toBeVisible();
        await expect(page.getByTestId('state-pension__error')).toBeVisible();
    });
});