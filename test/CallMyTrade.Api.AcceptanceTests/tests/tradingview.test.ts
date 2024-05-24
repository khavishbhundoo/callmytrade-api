import { beforeAll, describe, expect, test } from 'vitest';
import { z } from 'zod';

import { BEFORE_ALL_TIMEOUT, HOST } from '../utils/env';

// All properties are required by default
const schema = z.object({
    call_response: z.string(),
    utc_date_time: z.string(),
});

const ENDPOINT = '/webhook/tradingview';

describe(`Request ${HOST}${ENDPOINT}`, () => {
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
        expect(response.headers.get('Content-Type')).toBe('application/json');
    });

    test('Should have valid body schema', () => {
        expect(() => schema.parse(body)).not.toThrowError();
    });
});