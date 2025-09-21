import {test, expect} from '@playwright/test';

test.describe('Your Pay Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/your-pay');
    });

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('Your Pay - GOV.UK');
    });

    test('has content', async ({page}) => {
        const headers = page.getByRole('heading');
        expect(await headers.count()).toEqual(3);
        for (const h of await headers.all()) {
            await expect(h).toBeVisible();
        }
        
        const hint = page.getByTestId('amount-hint');
        expect(await hint.count()).toEqual(1);
        await expect(hint).toBeVisible();
        
        const textInput = page.getByTestId('Cost');
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
        await page.getByTestId('Cost').fill('32768.64');
        await page.getByTestId('F__Yearly').click();
        await page.getByRole('button').click();
        expect(page.url()).toContain('/your-pay'); // TODO: Next Page
    });
});

test.describe('Your Pay Page - Error State', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/your-pay');
    });
    
    test('no errors visible on initial page load', async ({page}) => {
        await expect(page.getByTestId('Error-Summary')).not.toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Cost'))).not.toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Frequency'))).not.toBeVisible();
        await (expect(page.getByTestId('C__Error'))).not.toBeVisible();
        await (expect(page.getByTestId('F__Error'))).not.toBeVisible();
    });
    
    test('no amount or frequency of salary given shows errors', async ({page}) => {
        await page.getByRole('button').click();
        await expect(page.getByTestId('Error-Summary')).toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Cost'))).toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Frequency'))).toBeVisible();
        await (expect(page.getByTestId('C__Error'))).toBeVisible();
        await (expect(page.getByTestId('F__Error'))).toBeVisible();
    });
    
    test('amount given but no frequency only shows frequency errors', async ({page}) => {
        await page.getByTestId('Cost').fill('32768.64');
        await page.getByRole('button').click();
        await expect(page.getByTestId('Error-Summary')).toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Cost'))).not.toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Frequency'))).toBeVisible();
        await (expect(page.getByTestId('C__Error'))).not.toBeVisible();
        await (expect(page.getByTestId('F__Error'))).toBeVisible();
    });
    
    test('frequency given but no amount only shows amount errors', async ({page}) => {
        await page.getByTestId('F__Yearly').click();
        await page.getByRole('button').click();
        await expect(page.getByTestId('Error-Summary')).toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Cost'))).toBeVisible();
        await (expect(page.getByTestId('Error-Summary__Frequency'))).not.toBeVisible();
        await (expect(page.getByTestId('C__Error'))).toBeVisible();
        await (expect(page.getByTestId('F__Error'))).not.toBeVisible();
    });
});