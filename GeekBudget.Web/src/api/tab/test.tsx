import * as api from '.';
import { mockFetch, mockFetchResponse } from 'helpers/testHelpers';
import Tab from 'models/Tab';

const tabs: Tab[] = [
    {id: 1, amount: 100, name: 'test', currency: 'EUR'}
];

describe('Operation api', () => {
    describe('getall', () => {
        it('should return tabs', async () => {
            const fetch = mockFetch(mockFetchResponse(tabs, 200, 'ok'));
            const result = await api.getAll();
            expect(result).toEqual(tabs);
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
        it('should return new tabs id', async () => {
            const fetch = mockFetch(mockFetchResponse(123, 200, 'ok'));
            const result = await api.add(tabs[0]);
            expect(result).toEqual(123);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.add(tabs[0]);
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
            const result = await api.update(tabs[0]);
            expect(result).toEqual(true);
        });
        it('should throw an error', async () => {
            const fetch = mockFetch(mockFetchResponse('not ok', 400, 'not ok'));
            try{
                await api.update(tabs[0]);
            } catch (e){
                expect(e).toMatch(/Fetch failed!/);
                expect(e).toMatch(/not ok/);
            }
        });
    });
});