import { test, expect } from '@playwright/test';

test.use({ baseURL: process.env.STORE_ADMIN_URL });

test('can add product ', async ({ page }) => {
    await page.goto('/');

    await page.getByRole('link', { name: 'Products' }).click();
    await page.getByRole('button', { name: 'Save' }).click();

    await page.getByPlaceholder('Name').fill('Bear Snacks');
    await page.getByPlaceholder('Price').fill('29.99');
    await page.getByPlaceholder('Description').fill('dog, snack, treat, courage, ghosts');

    const askOpenAI = page.getByRole('button', { name: 'Ask OpenAI' });

    if (await askOpenAI.isVisible()) {
      const aiApiCall = page.waitForResponse('/ai/generate/description');
      await askOpenAI.click();
      await aiApiCall;
    }
    else {
        await page.getByPlaceholder('Description').fill('Something tasty for the pups');
    }

    await page.getByRole('button', { name: 'Save' }).click();
    await expect(page.getByRole('heading', { name: 'Bear Snacks - 29.99' })).toBeVisible();
    await expect(page.getByRole('button', { name: 'Edit Product' })).toBeVisible();
  });
