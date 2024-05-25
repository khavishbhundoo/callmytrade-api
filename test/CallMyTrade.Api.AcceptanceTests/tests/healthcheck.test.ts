import { beforeAll, describe, expect, test } from 'vitest';
import { z } from 'zod';

import { BEFORE_ALL_TIMEOUT, HOST } from '../utils/env';

// All properties are required by default
const schema = z.object({
    status: z.string(),
    reports: z.array(z.unknown()),
    version: z.string(),
});

const ENDPOINT = '/_system/health';

describe(`GET ${HOST}${ENDPOINT}`, () => {
    let response: Response;
    let body: { [key: string]: unknown };

    beforeAll(async () => {
        const url = `${HOST}${ENDPOINT}`;
        response = await fetch(url);
        body = await response.json();
    }, BEFORE_ALL_TIMEOUT);

    test('Should have response status 200', () => {
        expect(response.status).toBe(200);
    });

    test('Should have content-type = application/json', () => {
        expect(response.headers.get('Content-Type')).toBe('application/json; charset=utf-8');
    });

    test('Should have valid body schema', () => {
        expect(() => schema.parse(body)).not.toThrowError();
    });
});