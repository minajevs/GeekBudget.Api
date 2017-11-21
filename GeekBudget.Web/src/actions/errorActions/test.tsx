import * as actions from '.';

import * as types from 'constants/errorConstants';

describe('errorActions', () => {
    it('should create an "throw error" action', () => {
        const appError = {text: 'error', code: 500, object: {}};
        const action = actions.throwApplicationError(appError);
        expect(action.type).toBe(types.THROW_APPLICATION_ERROR);
        expect(action.payload).toBe(appError);
    });
});