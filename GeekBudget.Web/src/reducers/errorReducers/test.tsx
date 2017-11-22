import * as reducers from '.';
import * as actions from 'actions/errorActions';
import { initialState, ApplicationError, ApplicationErrorState } from 'types';
import { Action } from 'redux-actions';

const appError: ApplicationError = {
    code: 400,
    object: 'error',
    text: 'test-error'
};

describe('Error reducers', () => {
    const action = actions.throwApplicationError(appError);
    it('should reduce "app error"', () => {
        const newState = reducers.errorReducers(initialState.applicationError, action);
        expect(newState.error).toBe(appError);
        expect(newState.errorText).toBe(appError.text);
    });
});