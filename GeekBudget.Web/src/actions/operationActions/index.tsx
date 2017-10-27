import * as constants from '../../constants/operationConstants';
import { createAction } from 'redux-actions';
import { push } from 'react-router-redux'

import { throwApplicationError } from '../errorActions';
import { ApplicationError, ValidationError } from '../../types';

import Api from '../../api';

import Operation from '../../models/Operation';

export const requestAllOperations = createAction(constants.REQUEST_ALL_OPERATIONS);
export const receiveAllOperations = createAction<Operation[]>(constants.RECEIVE_ALL_OPERATIONS);
export function getAllOperations(){
    return async function (dispatch:any){
        dispatch(requestAllOperations());
        try{
            const operations = await Api.operation.getAll();
            dispatch(receiveAllOperations(operations));
            dispatch(requestResponseOperation('Success'));
        } catch(e){
            dispatch(responseErrorOperation(e));
        }
    }
}

export const requestOperation = createAction<number>(constants.REQUEST_OPERATION);
export const receiveOperation = createAction<Operation>(constants.RECEIVE_OPERATION);

export const requestAddOperation = createAction<Operation>(constants.REQUEST_ADD_OPERATION);
export const responseAddOperation = createAction<number>(constants.RESPONSE_ADD_OPERATION);
export function addOperation(operation:Operation){
    return async function (dispatch:any){
        dispatch(requestAddOperation(operation));
        try{
            const newId = await Api.operation.add(operation);    //Add operation, get its ID
            await getAllOperations()(dispatch);           //Reload alloperations
            dispatch(responseAddOperation(newId));        //When its done dispatch success response
            dispatch(requestResponseOperation('Success'));
        } catch(e){
            dispatch(responseErrorOperation(e));
        } finally{
            dispatch(push('/operations'));
        }
    }
}

export const requestRemoveOperation = createAction<number>(constants.REQUEST_REMOVE_OPERATION);
export function removeOperation(id:number){
    return async function (dispatch:any){
        dispatch(requestRemoveOperation(id));
        try{
            await Api.operation.remove(id);
            dispatch(getAllOperations());
            dispatch(requestResponseOperation('Success'));
        } catch(e){
            dispatch(responseErrorOperation(e));
        }
    }
}

export const requestUpdateOperation = createAction<Operation>(constants.REQUEST_UPDATE_OPERATION);
export function updateOperation(operation:Operation){
    return async function (dispatch:any){
        dispatch(requestUpdateOperation(operation));
        try{
            await Api.operation.update(operation);
            dispatch(getAllOperations());
            dispatch(requestResponseOperation('Success'));
        } catch(e){
            dispatch(responseErrorOperation(e));
        }
    }
}

export const requestResponseOperation = createAction<string>(constants.RESPONSE_OPERATION);
export const requestResponseErrorOperation = createAction<ApplicationError>(constants.RESPONSE_ERROR_OPERATION);
export function responseErrorOperation(error:any){
    const appError = {
        code: 400, // TODO
        object: error, // TODO
        text: error // TODO
    }
    return async function (dispatch:any){
        dispatch(throwApplicationError(appError));
    }
}