import * as constants from '../../constants/operationConstants';
import { createAction } from 'redux-actions';

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
            dispatch(requestResponseOperation('success'));
        } catch(e){
            dispatch(requestResponseOperation(e.toString()));
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
            dispatch(requestResponseOperation('success'));
        } catch(e){
            dispatch(requestResponseOperation(e));
        }
    }
}

export const requestRemoveOperation = createAction<number>(constants.REQUEST_REMOVE_OPERATION);
export function removeOperation(id:number){
    return async function (dispatch:any){
        let responseString = "";
        dispatch(requestRemoveOperation(id));
        try{
            await Api.operation.remove(id);
            dispatch(getAllOperations());
            responseString = 'Success!';
        } catch(e){
            responseString = e;
        } finally{
            dispatch(requestResponseOperation(responseString));
        }
    }
}

export const requestUpdateOperation = createAction<Operation>(constants.REQUEST_UPDATE_OPERATION);
export function updateOperation(operation:Operation){
    return async function (dispatch:any){
        let responseString = "";
        dispatch(requestUpdateOperation(operation));
        try{
            await Api.operation.update(operation);
            dispatch(getAllOperations());
            responseString = 'Success!';
        } catch(e){
            responseString = e;
        } finally{
            dispatch(requestResponseOperation(responseString));
        }
    }
}

export const requestResponseOperation = createAction<string>(constants.RESPONSE_OPERATION);