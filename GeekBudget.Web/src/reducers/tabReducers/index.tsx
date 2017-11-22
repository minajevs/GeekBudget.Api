import * as constants from 'constants/tabConstants';
import { TabState, initialState, ApplicationError } from 'types';
import { handleActions, Action } from 'redux-actions';

import Tab from 'models/Tab';

export const tabReducers = handleActions<TabState, Tab[] | Tab | ApplicationError | string | number | void>(
    {
        [constants.API_REQUEST_ALL_TAB]: (state: TabState): TabState => ({ ...state, isFetching: true }),
        [constants.API_RESPONSE_ALL_TAB]: (state: TabState, action: Action<Tab[]>): TabState => ({
            ...state,
            items: action.payload || state.items,
        }),

        [constants.API_REQUEST_GET_TAB]: (state: TabState, action: Action<number>): TabState => ({ 
            ...state, 
            isFetching: true 
        }),
        [constants.API_RESPONSE_GET_TAB]: (state: TabState, action: Action<Tab>): TabState => {
            const tab = action.payload as Tab;
            if (state.items.filter(x => x.id === tab.id).length === 0)
                state.items.push(tab);
            return { ...state };  // Shallow copy to force rerender
        },

        [constants.API_REQUEST_ADD_TAB]: (state: TabState, action: Action<Tab>): TabState => ({ 
            ...state, 
            isFetching: true 
        }),
        [constants.API_RESPONSE_ADD_TAB]: (state: TabState, action: Action<number>): TabState => {
            // TODO: automatically select tab for edit?
            return { ...state };  // Shallow copy to force rerender
        },

        [constants.API_REQUEST_REMOVE_TAB]: (state: TabState, action: Action<number>): TabState => ({
            ...state,
            isFetching: true
        }),

        [constants.API_REQUEST_EDIT_TAB]: (state: TabState, action: Action<Tab>): TabState => ({
            ...state,
            isFetching: true
        }),

        [constants.API_RESPONSE_TAB]: (state: TabState, action: Action<string>): TabState => {
            state.isFetching = false;
            return { ...state };  // Shallow copy to force rerender
        },

        [constants.API_ERROR_TAB]: (state: TabState, action: Action<ApplicationError>): TabState => {
            alert((action.payload as ApplicationError).text);
            state.isFetching = false;
            return { ...state };  // Shallow copy to force rerender
        },

        [constants.UI_EDIT_OPEN_TAB]: (state: TabState, action: Action<Tab>): TabState => (
            {
                ...state,
                edit: {
                    ...state.edit,
                    open: true,
                    tab: action.payload as Tab,
                    isNew: (action.payload as Tab).id === -1
                }
            }),

        [constants.UI_EDIT_SAVE_TAB]: (state: TabState, action: Action<Tab>): TabState => (
            {
                ...state,
                isFetching: true
            }),

        [constants.UI_EDIT_CLOSE_TAB]: (state: TabState, action: Action<Tab>): TabState => (
            {
                ...state,
                edit: {
                    ...state.edit,
                    open: false
                }
            }),

    },
    initialState.tabs);