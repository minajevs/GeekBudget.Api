import * as constants from '../../constants/operationConstants';
import { OperationState, initialState } from '../../types';
import { handleActions, Action } from 'redux-actions';

import Operation from '../../models/Operation';

export const operationReducers = handleActions<OperationState, Operation[] | Operation | string | number | void>({
    [constants.REQUEST_ALL_OPERATIONS]: (state: OperationState): OperationState => ({ ...state, isFetching: true, items: [] }),
    [constants.RECEIVE_ALL_OPERATIONS]: (state: OperationState, action: Action<Operation[]>): OperationState => ({
        ...state,
        items: action.payload || state.items,
    }),

    [constants.REQUEST_OPERATION]: (state: OperationState, action: Action<number>): OperationState => ({ ...state, isFetching: true }),
    [constants.RECEIVE_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => {
        const tab = action.payload as Operation;
        if (state.items.filter(x => x.id === tab.id).length === 0)
            state.items.push(tab);
        //
        return { ...state };  //Shallow copy to force rerender
    },

    [constants.REQUEST_ADD_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => ({ ...state, isFetching: true }),
    [constants.RESPONSE_ADD_OPERATION]: (state: OperationState, action: Action<number>): OperationState => {
        //TODO: automatically select tab for edit?
        return { ...state };  //Shallow copy to force rerender
    },

    [constants.REQUEST_REMOVE_OPERATION]: (state: OperationState, action: Action<number>): OperationState => ({ ...state, isFetching: true }),

    [constants.REQUEST_UPDATE_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => ({ ...state, isFetching: true }),

    [constants.RESPONSE_OPERATION]: (state: OperationState, action: Action<string>): OperationState => {
        state.isFetching = false;
        return { ...state };  //Shallow copy to force rerender
    },

}, initialState.operations);