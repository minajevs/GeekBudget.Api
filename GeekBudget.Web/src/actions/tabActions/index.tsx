import * as constants from 'constants/tabConstants';
import { createAction } from 'redux-actions';
import { Dispatch } from 'redux';
import { ApplicationError, ValidationError } from 'types';

import { throwApplicationError } from 'actions/errorActions';

import Api from 'api';

import Tab from 'models/Tab';

export const requestAllTabs = createAction(constants.REQUEST_ALL_TABS);
export const receiveAllTabs = createAction<Tab[]>(constants.RECEIVE_ALL_TABS);
export function getAllTabs() {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestAllTabs());
        try {
            const tabs = await Api.tab.getAll();
            dispatch(receiveAllTabs(tabs));
            dispatch(requestResponseTab('Success!'));
        } catch  (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestTab = createAction<number>(constants.REQUEST_TAB);
export const receiveTab = createAction<Tab>(constants.RECEIVE_TAB);

export const requestAddTab = createAction<Tab>(constants.REQUEST_ADD_TAB);
export const responseAddTab = createAction<number>(constants.RESPONSE_ADD_TAB);
export function addTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestAddTab(tab));
        try {
            const newId = await Api.tab.add(tab);    // Add tab, get its ID
            await getAllTabs()(dispatch);           // Reload alltabs
            dispatch(responseAddTab(newId));        // When its done dispatch success response
            dispatch(requestResponseTab('Success!'));
        } catch  (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestRemoveTab = createAction<number>(constants.REQUEST_REMOVE_TAB);
export function removeTab(id: number) {
    return async function (dispatch: Dispatch<{}>){
        let responseString = '';
        dispatch(requestRemoveTab(id));
        try {
            await Api.tab.remove(id);
            dispatch(getAllTabs());
            dispatch(requestResponseTab('Success!'));
        } catch  (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestUpdateTab = createAction<Tab>(constants.REQUEST_UPDATE_TAB);
export function updateTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        let responseString = '';
        dispatch(requestUpdateTab(tab));
        try {
            await Api.tab.update(tab);
            dispatch(getAllTabs());
            dispatch(requestResponseTab('Success!'));
        } catch  (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestResponseTab = createAction<string>(constants.RESPONSE_TAB);
export const requestResponseErrorTab = createAction<ApplicationError>(constants.RESPONSE_ERROR_TAB);
export function responseErrorOperation(error:  Error) {
    const appError: ApplicationError = {
        code: 400, // TODO
        object: error as {}, // TODO
        text: JSON.stringify(error) // TODO
    };
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestResponseTab('failed'));
        dispatch(throwApplicationError(appError));
    };
}

export const requestEditTab = createAction<Tab>(constants.REQUEST_EDIT_TAB);
export const closeEditTab = createAction(constants.CLOSE_EDIT_TAB);
export const saveEditTab = createAction<Tab>(constants.SAVE_EDIT_TAB);
export function saveTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(saveEditTab(tab));
        if (tab.id === -1) // new tab
            dispatch(addTab(tab));
        else
            dispatch(updateTab(tab));

        dispatch(closeEditTab());
    };
}