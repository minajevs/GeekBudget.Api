import * as api from '.';
import { mockFetch, mockFetchResponse } from 'helpers/test';
import Operation from 'models/Operation';

const operationsJson = [
    {id: 1, amount: 100, comment: 'test', currency: 'EUR', date: new Date(1990, 1, 12).toString(), from: 1,  to: 2}
];

const operations: Operation[] = [
    {id: 1, amount: 100, comment: 'test', currency: 'EUR', date: new Date(1990, 1, 12), from: 1,  to: 2}
];

describe('Operation api', () => {
    describe('getall', () => {
        it('should return operations', async () => {
            const fetch = mockFetch(mockFetchResponse(operations, 200, 'ok'));
            const result = await api.getAll();
            expect(result).toEqual(operations);
        });
        it('should fix dates', async () => {
            const fetch = mockFetch(mockFetchResponse(operationsJson, 200, 'ok'));
            const result = await api.getAll();
            expect(result).toEqual(operations);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.getAll();
            } catch (e){
                expect(e).toMatch(/Fetch failed!/);
                expect(e).toMatch(/not ok/);
            }
        });
    });
    describe('add', () => {
        it('should return new operation id', async () => {
            const fetch = mockFetch(mockFetchResponse(123, 200, 'ok'));
            const result = await api.add(operations[0]);
            expect(result).toEqual(123);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.add(operations[0]);
            } catch (e){
                expect(e).toMatch(/Fetch failed!/);
                expect(e).toMatch(/not ok/);
            }
        });
    });
    describe('remove', () => {
        it('should return boolean result', async () => {
            const fetch = mockFetch(mockFetchResponse('anything blabla', 200, 'ok'));
            const result = await api.remove(1);
            expect(result).toEqual(true);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.remove(1);
            } catch (e){
                expect(e).toMatch(/Fetch failed!/);
                expect(e).toMatch(/not ok/);
            }
        });
    });
    describe('update', () => {
        it('should return boolean result', async () => {
            const fetch = mockFetch(mockFetchResponse('anything blabla', 200, 'ok'));
            const result = await api.update(operations[0]);
            expect(result).toEqual(true);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.update(operations[0]);
            } catch (e){
                expect(e).toMatch(/Fetch failed!/);
                expect(e).toMatch(/not ok/);
            }
        });
    });
});