import { combineReducers } from 'redux';

import { testBoolActions } from './testReducers';

import { tabReducers } from './tabReducers';

import { StoreState } from '../types'

export const rootReducer = combineReducers<StoreState>({
    testBool: testBoolActions,
    tabs: tabReducers
});