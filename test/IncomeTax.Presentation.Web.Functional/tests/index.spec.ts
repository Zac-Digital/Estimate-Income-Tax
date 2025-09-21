import {test, expect} from '@playwright/test';

test.describe('Home Page', () => {
    test.beforeEach(async ({page}) => {
        await page.goto('/');
    })

    test('has title', async ({page}) => {
        await expect(page).toHaveTitle('Start Page - GOV.UK');
    });

    test('has content', async ({page}) => {
        const header = page.getByRole('heading');
        expect(await header.count()).toEqual(1);
        await expect(header).toBeVisible();
        const paragraphs = page.getByRole('paragraph');
        expect(await paragraphs.count()).toEqual(4);
        for (const p of await paragraphs.all()) {
            await expect(p).toBeVisible();
        }
    });

    test('has start now button', async ({page}) => {
        const button = page.getByRole('button');
        expect(await button.count()).toEqual(1);
        await expect(button).toBeVisible();
        await expect(button).toHaveText('Start now');
    });

    test('start now button navigates to next page', async ({page}) => {
        await page.getByRole('button').click();
        expect(page.url()).toContain('/your-pay');
    });
});