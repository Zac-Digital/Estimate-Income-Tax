import {test, expect} from '@playwright/test';

test.describe('Scottish Tax Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/scottish-tax');
    });

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('Scottish Tax - GOV.UK');
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

        const detail = page.getByTestId('scottish-tax__details-summary');
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
        await page.waitForURL('**/check-answers')
        await page.goto('/scottish-tax');
        await page.getByTestId('scottish-tax__no').click();
        await page.getByRole('button').click();
        expect(page.url()).toContain('/check-answers');
    });
});