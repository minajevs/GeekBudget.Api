import { combineReducers } from 'redux';

import { tabReducers } from './tabReducers';
import { operationReducers } from './operationReducers';
import { errorReducers } from './errorReducers';

import { StoreState, TabState } from '../types';

import { routerReducer } from 'react-router-redux';

export const rootReducer = combineReducers<StoreState>({
    tabs: tabReducers,
    operations: operationReducers, 
    router: routerReducer,
    applicationError: errorReducers
});