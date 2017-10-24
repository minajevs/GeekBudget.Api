import { combineReducers } from 'redux';

import { tabReducers } from './tabReducers';

import { StoreState, TabState } from '../types';

export const rootReducer = combineReducers<StoreState>({
    tabs: tabReducers
});