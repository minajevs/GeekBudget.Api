import * as constants from 'constants/operationConstants';
import { createAction } from 'redux-actions';
import { Dispatch } from 'redux';
import { push } from 'react-router-redux';

import { throwApplicationError } from 'actions/errorActions';
import { getAllTabs } from 'actions/tabActions';
import { ApplicationError, ValidationError } from 'types';

import Api from 'api';

import Operation from 'models/Operation';

export const requestAllOperations = createAction(constants.REQUEST_ALL_OPERATIONS);
export const receiveAllOperations = createAction<Operation[]>(constants.RECEIVE_ALL_OPERATIONS);
export function getAllOperations() {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestAllOperations());
        try {
            let operations = await Api.operation.getAll();
            dispatch(receiveAllOperations(operations));
            dispatch(requestResponseOperation('Success'));
        } catch (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestOperation = createAction<number>(constants.REQUEST_OPERATION);
export const receiveOperation = createAction<Operation>(constants.RECEIVE_OPERATION);

export const requestAddOperation = createAction<Operation>(constants.REQUEST_ADD_OPERATION);
export const responseAddOperation = createAction<number>(constants.RESPONSE_ADD_OPERATION);
export function addOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestAddOperation(operation));
        try {
            const newId = await Api.operation.add(operation);    // Add operation, get its ID
            await getAllOperations()(dispatch);           // Reload alloperations
            dispatch(responseAddOperation(newId));        // When its done dispatch success response
            dispatch(requestResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestRemoveOperation = createAction<number>(constants.REQUEST_REMOVE_OPERATION);
export function removeOperation(id: number) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestRemoveOperation(id));
        try {
            await Api.operation.remove(id);
            dispatch(getAllOperations());
            dispatch(requestResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestUpdateOperation = createAction<Operation>(constants.REQUEST_UPDATE_OPERATION);
export function updateOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestUpdateOperation(operation));
        try {
            await Api.operation.update(operation);
            dispatch(getAllOperations());
            dispatch(requestResponseOperation('Success'));
            dispatch(getAllTabs());
        } catch (e) {
            dispatch(responseErrorOperation(e));
        }
    };
}

export const requestResponseOperation = createAction<string>(constants.RESPONSE_OPERATION);
export const requestResponseErrorOperation = createAction<ApplicationError>(constants.RESPONSE_ERROR_OPERATION);
export function responseErrorOperation(error: Error) {
    const appError: ApplicationError = {
        code: 400, // TODO
        object: error as {}, // TODO
        text: JSON.stringify(error) // TODO
    };
    return async function (dispatch: Dispatch<{}>){
        dispatch(requestResponseOperation('failed'));
        dispatch(throwApplicationError(appError));
    };
}

export const requestEditOperation = createAction<Operation>(constants.REQUEST_EDIT_OPERATION);
export const closeEditOperation = createAction(constants.CLOSE_EDIT_OPERATION);
export const saveEditOperation = createAction<Operation>(constants.SAVE_EDIT_OPERATION);
export function saveOperation(operation: Operation) {
    return async function (dispatch: Dispatch<{}>){
        dispatch(saveEditOperation(operation));
        if (operation.id === -1) // new operation
            dispatch(addOperation(operation));
        else
            dispatch(updateOperation(operation));

        dispatch(closeEditOperation());
    };
}