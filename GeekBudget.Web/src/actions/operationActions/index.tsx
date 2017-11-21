import * as constants from 'constants/operationConstants';
import { createAction } from 'redux-actions';
import { Dispatch } from 'redux';
import { push } from 'react-router-redux';

import { throwApplicationError } from 'actions/errorActions';
import { getAllTabs } from 'actions/tabActions';
import { ApplicationError, ValidationError } from 'types';

import Api from 'api';

import Operation from 'models/Operation';

export const apiRequestAllOperations = createAction(constants.API_REQUEST_ALL_OPERATION);
export const apiResponseAllOperations = createAction<Operation[]>(constants.API_RESPONSE_ALL_OPERATION);
export function getAllOperations() {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestAllOperations());
        try {
            let operations = await Api.operation.getAll();
            dispatch(apiResponseAllOperations(operations));
            dispatch(apiResponseOperation('Success'));
        } catch (e) {
            dispatch(errorOperation(e));
        }
    };
}

export const apiRequestGetOperation = createAction<number>(constants.API_REQUEST_GET_OPERATION);
export const apiResponseGetOperation = createAction<Operation>(constants.API_RESPONSE_GET_OPERATION);

export const apiRequestAddOperation = createAction<Operation>(constants.API_REQUEST_ADD_OPERATION);
export const apiResponseAddOperation = createAction<number>(constants.API_RESPONSE_ADD_OPERATION);
export function addOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestAddOperation(operation));
        try {
            const newId = await Api.operation.add(operation);    // Add operation, get its ID
            await getAllOperations()(dispatch);           // Reload alloperations
            dispatch(apiResponseAddOperation(newId));        // When its done dispatch success response
            dispatch(apiResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(errorOperation(e));
        }
    };
}

export const apiRequestRemoveOperation = createAction<number>(constants.API_REQUEST_REMOVE_OPERATION);
export function removeOperation(id: number) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestRemoveOperation(id));
        try {
            await Api.operation.remove(id);
            dispatch(getAllOperations());
            dispatch(apiResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(errorOperation(e));
        }
    };
}

export const apiRequestEditOperation = createAction<Operation>(constants.API_REQUEST_EDIT_OPERATION);
export function editOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiRequestEditOperation(operation));
        try {
            await Api.operation.update(operation);
            dispatch(getAllOperations());
            dispatch(apiResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(errorOperation(e));
        }
    };
}

export const apiResponseOperation = createAction<string>(constants.API_RESPONSE_OPERATION);
export const apiErrorOperation = createAction<ApplicationError>(constants.API_ERROR_OPERATION);
export function errorOperation(error: Error) {
    const appError: ApplicationError = {
        code: 400, // TODO
        object: error as {}, // TODO
        text: JSON.stringify(error) // TODO
    };
    return async function (dispatch: Dispatch<{}>){
        dispatch(apiResponseOperation('failed'));
        dispatch(throwApplicationError(appError));
    };
}

export const uiEditOpenOperation = createAction<Operation>(constants.UI_EDIT_OPEN_OPERATION);
export const uiEditCloseOperation = createAction(constants.UI_EDIT_CLOSE_OPERATION);
export const uiEditSaveOperation = createAction<Operation>(constants.UI_EDIT_SAVE_OPERATION);
export function saveOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(uiEditSaveOperation(operation));
        if (operation.id === -1) // new operation
            dispatch(addOperation(operation));
        else
            dispatch(editOperation(operation));

        dispatch(uiEditCloseOperation());
    };
}