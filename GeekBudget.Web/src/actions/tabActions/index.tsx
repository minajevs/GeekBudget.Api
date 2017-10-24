import * as constants from '../../constants/tabConstants';
import { createAction } from 'redux-actions';

import Tab from '../../models/Tab';

export const requestAllTabs = createAction<void>(constants.REQUEST_ALL_TABS);
export const receiveAllTabs = createAction<Tab[]>(constants.REQUEST_ALL_TABS);

export const requestTab = createAction<number>(constants.REQUEST_TAB);
export const receiveTab = createAction<Tab>(constants.RECEIVE_TAB);

export const removeTab = createAction<number>(constants.REQUEST_REMOVE_TAB);

export const updateTab = createAction<Tab>(constants.REQUEST_UPDATE_TAB);

export const responseTab = createAction<string>(constants.RESPONSE_TAB);