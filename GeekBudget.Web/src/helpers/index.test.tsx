import * as helpers from './index';

// API Settings
// REACT_APP_API_URL="https://localhost/api"
// REACT_APP_API_ACCESS_KEY="accesskey"

describe('Common helpers', () => {
    describe('date to string', () => {
        it('should create correct date string', () => {
            const date = new Date(2017, 1, 24); // 24 feb 2017
            const str = helpers.dateToString(date);
            expect(str).toEqual('2017-02-24');
        });
    });

    describe('pad start', () => {
        it('should pad string correctly', () => {
            const pad1 = helpers.padStart('######', 'test', 5); // #test
            const pad2 = helpers.padStart('######', 'test', 4); // test
            const pad3 = helpers.padStart('00', 1, 2); // 01
            expect(pad1).toEqual('#test');
            expect(pad2).toEqual('test');
            expect(pad3).toEqual('01');
        });
    });

    describe('insert item', () => {
        it('should insert new item in to position', () => {
            const before = [1, 2, 3, 4];
            const after = helpers.insertItem(before, 11, 1);
            expect(after).toEqual([1, 11, 2, 3, 4]);
        });
        it('should insert new item in to last place', () => {
            const before = [1, 2, 3, 4];
            const after = helpers.insertItem(before, 11);
            expect(after).toEqual([1, 2, 3, 4, 11]);
        });
        it('should insert new item in to first place', () => {
            const before = [1, 2, 3, 4];
            const after = helpers.insertItem(before, 11, 0);
            expect(after).toEqual([11, 1, 2, 3, 4]);
        });
    });

    describe('remove item', () => {
        it('should remove an item from position', () => {
            const before = [1, 2, 3, 4];
            const after = helpers.removeItem(before, 0);
            expect(after).toEqual([2, 3, 4]);
        });
    });
});