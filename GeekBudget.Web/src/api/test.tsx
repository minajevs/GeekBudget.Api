import Api from '.';
import * as tabApi from './tab';
import * as operationApi from './operation';
import { apiHelpers } from 'helpers';

describe('Api', () => {
    it('should have static methods', () => {
        expect(Api.operation).toBe(operationApi);
        expect(Api.tab).toBe(tabApi);
        expect(Api.helpers).toBe(apiHelpers);
    });
});