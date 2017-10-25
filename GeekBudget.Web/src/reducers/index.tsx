import { combineReducers } from 'redux';

import { tabReducers } from './tabReducers';

import { StoreState, TabState } from '../types';

import { routerReducer } from 'react-router-redux'

export const rootReducer = combineReducers<StoreState>({
    tabs: tabReducers,
    router: routerReducer
});