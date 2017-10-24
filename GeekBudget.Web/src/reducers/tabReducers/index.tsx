import * as constants from '../../constants/tabConstants';
import { TabState, initialState } from '../../types';
import { handleActions, Action } from 'redux-actions';

import Tab from '../../models/Tab';

export const tabReducers = handleActions<TabState, Tab[]|Tab|string|number|void>({
	[constants.REQUEST_ALL_TABS]: (state:TabState): TabState => {
        state.isFetching = true;
        //
        return state;
    },
    [constants.RECEIVE_ALL_TABS]: (state:TabState, action:Action<Tab[]>): TabState => {
        state.isFetching = false;
        state.items = action.payload || state.items;
        //
        return state;
    },

    [constants.REQUEST_TAB]: (state:TabState, action:Action<number>): TabState => {
        state.isFetching = true;
        //
        return state;
    },
    [constants.RECEIVE_TAB]: (state:TabState, action:Action<Tab>): TabState => {
        state.isFetching = false;
        const tab = action.payload as Tab;
        if(state.items.filter(x => x.id === tab.id).length === 0)
            state.items.push(tab);
        //
        return state;
    },

    [constants.REQUEST_ADD_TAB]: (state:TabState, action:Action<Tab>): TabState => {
        state.isFetching = true;
        const tab = action.payload as Tab;
        //
        return state;
    },

    [constants.REQUEST_REMOVE_TAB]: (state:TabState, action:Action<number>): TabState => {
        state.isFetching = true;
        //
        return state;
    },

    [constants.REQUEST_UPDATE_TAB]: (state:TabState, action:Action<Tab>): TabState => {
        state.isFetching = true;
        //
        return state;
    },

    [constants.RESPONSE_TAB]: (state:TabState, action:Action<string>): TabState => {
        state.isFetching = false;

        //
        return state;
    },
    
}, initialState.tabs);