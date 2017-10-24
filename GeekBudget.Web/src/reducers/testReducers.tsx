import * as constants from '../constants';
import { StoreState, initialState } from '../types';
import { handleActions, Action } from 'redux-actions';

export const testBoolActions = handleActions<boolean>({
	[constants.SET_TESTBOOL]: (state:boolean): boolean => {
		return !state
	}
}, false);