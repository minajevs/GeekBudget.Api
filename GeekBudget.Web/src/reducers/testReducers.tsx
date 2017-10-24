import * as constants from '../constants';
import { StoreState, initialState } from '../types';
import { handleActions, Action } from 'redux-actions';

export default handleActions<StoreState, any>({
	[constants.SET_TESTBOOL]: (state:StoreState): StoreState => {
		return {...state, testBool: !state.testBool}
	},

	[constants.SET_TESTSTRING]: (state:StoreState, action: Action<string>): StoreState => {
		return {...state, testString: action.payload || state.testString}
	},
}, initialState);