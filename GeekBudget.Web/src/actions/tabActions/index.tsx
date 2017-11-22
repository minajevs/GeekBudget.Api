import * as constants from 'constants/tabConstants';
import { createAction } from 'redux-actions';
import { Dispatch } from 'redux';
import { ApplicationError, ValidationError } from 'types';

import { throwApplicationError } from 'actions/errorActions';

import Api from 'api';

import Tab from 'models/Tab';

export const apiRequestAllTabs = createAction(constants.API_REQUEST_ALL_TAB);
export const apiResponseAllTabs = createAction<Tab[]>(constants.API_RESPONSE_ALL_TAB);
export function getAllTabs() {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestAllTabs());
        try {
            const tabs = await Api.tab.getAll();
            dispatch(apiResponseAllTabs(tabs));
            dispatch(apiResponseTab('Success!'));
        } catch  (e) {
            dispatch(errorTab(e));
        }
    };
}

export const apiRequestGetTab = createAction<number>(constants.API_REQUEST_GET_TAB);
export const apiResponseGetTab = createAction<Tab>(constants.API_RESPONSE_GET_TAB);

export const apiRequestAddTab = createAction<Tab>(constants.API_REQUEST_ADD_TAB);
export const apiResponseAddTab = createAction<number>(constants.API_RESPONSE_ADD_TAB);
export function addTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestAddTab(tab));
        try {
            const newId = await Api.tab.add(tab);    // Add tab, get its ID
            await getAllTabs()(dispatch);           // Reload alltabs
            dispatch(apiResponseAddTab(newId));        // When its done dispatch success response
            dispatch(apiResponseTab('Success!'));
        } catch  (e) {
            dispatch(errorTab(e));
        }
    };
}

export const apiRequestRemoveTab = createAction<number>(constants.API_REQUEST_REMOVE_TAB);
export function removeTab(id: number) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestRemoveTab(id));
        try {
            await Api.tab.remove(id);
            dispatch(getAllTabs());
            dispatch(apiResponseTab('Success!'));
        } catch  (e) {
            dispatch(errorTab(e));
        }
    };
}

export const apiRequestEditTab = createAction<Tab>(constants.API_REQUEST_EDIT_TAB);
export function editTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestEditTab(tab));
        try {
            await Api.tab.update(tab);
            dispatch(getAllTabs());
            dispatch(apiResponseTab('Success!'));
        } catch  (e) {
            dispatch(errorTab(e));
        }
    };
}

export const apiResponseTab = createAction<string>(constants.API_RESPONSE_TAB);
export const apiErrorTab = createAction<ApplicationError>(constants.API_ERROR_TAB);
export function errorTab(error:  Error) {
    const appError: ApplicationError = {
        code: 400, // TODO
        object: error as {}, // TODO
        text: JSON.stringify(error) // TODO
    };
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiResponseTab('failed'));
        dispatch(throwApplicationError(appError));
    };
}

export const uiEditOpenTab = createAction<Tab>(constants.UI_EDIT_OPEN_TAB);
export const uiEditCloseTab = createAction(constants.UI_EDIT_CLOSE_TAB);
export const uiEditSaveTab = createAction<Tab>(constants.UI_EDIT_SAVE_TAB);
export function saveTab(tab: Tab) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(uiEditSaveTab(tab));
        if (tab.id === -1) // new tab
            dispatch(addTab(tab));
        else
            dispatch(editTab(tab));

        dispatch(uiEditCloseTab());
    };
}