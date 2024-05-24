import { beforeAll, describe, expect, test } from 'vitest';
import { z } from 'zod';

import { BEFORE_ALL_TIMEOUT, HOST } from '../utils/env';

// All properties are required by default
const schema = z.object({
    call_response: z.string(),
    utc_date_time: z.string(),
});

const errorSchema = z.object({
    error_code: z.string(),
    error_message: z.string(),
});

const failedSchema = z.object({
    validation_errors: z.array(errorSchema),
});

const ENDPOINT = '/webhook/tradingview';

describe(`POST successful request ${HOST}${ENDPOINT} with plain text`, () => {
    let response: Response;
    let body: { [key: string]: unknown };

    beforeAll(async () => {
        const url = `${HOST}${ENDPOINT}`;
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: 'BTCUSD Greater Than 9000'
        });
        body = await response.json();
    }, BEFORE_ALL_TIMEOUT);

    test('Should have response status 201', () => {
        expect(response.status).toBe(201);
    });

    test('Should have content-type = application/json', () => {
        expect(response.headers.get('Content-Type')).toBe('application/json; charset=utf-8');
    });

    test('Should have valid body schema', () => {
        expect(() => schema.parse(body)).not.toThrowError();
    });
});

describe(`POST successful request ${HOST}${ENDPOINT} with JSON`, () => {
    let response: Response;
    let body: { [key: string]: unknown };

    beforeAll(async () => {
        const url = `${HOST}${ENDPOINT}`;
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: JSON.stringify({text: 'BTCUSD Greater Than 9000'})
        });
        body = await response.json();
    }, BEFORE_ALL_TIMEOUT);

    test('Should have response status 201', () => {
        expect(response.status).toBe(201);
    });

    test('Should have content-type = application/json', () => {
        expect(response.headers.get('Content-Type')).toBe('application/json; charset=utf-8');
    });

    test('Should have valid body schema', () => {
        expect(() => schema.parse(body)).not.toThrowError();
    });
});


describe(`POST failed request ${HOST}${ENDPOINT} with missing message as plaintext`, () => {
    let response: Response;
    let body: { [key: string]: unknown };

    beforeAll(async () => {
        const url = `${HOST}${ENDPOINT}`;
        response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: ''
        });
        body = await response.json();
    }, BEFORE_ALL_TIMEOUT);

    test('Should have response status 422', () => {
        expect(response.status).toBe(422);
    });

    test('Should have content-type = application/json', () => {
        expect(response.headers.get('Content-Type')).toBe('application/json; charset=utf-8');
    });

    test('Should have valid body schema', () => {
        expect(() => failedSchema.parse(body)).not.toThrowError();
    });
});