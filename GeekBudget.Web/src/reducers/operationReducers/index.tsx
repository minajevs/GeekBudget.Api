import * as constants from '../../constants/operationConstants';
import { OperationState, OperationEditState, initialState, ApplicationError } from '../../types';
import { handleActions, Action } from 'redux-actions';

import Operation from '../../models/Operation';

export const operationReducers =
    handleActions<OperationState, Operation[] | Operation | ApplicationError | string | number | void>(
        {
            [constants.REQUEST_ALL_OPERATIONS]: (state: OperationState): OperationState => ({
                ...state,
                isFetching: true,
                items: []
            }),
            [constants.RECEIVE_ALL_OPERATIONS]: (state: OperationState,
                                                 action: Action<Operation[]>): OperationState => ({
                    ...state,
                    items: (action.payload || state.items)
                        .map(a => ({ ...a, date: new Date(a.date) })) // fix dates for JS
                        .sort((a, b) => {
                            return (+b.date) - (+a.date);           // sort by date
                        }),
                }),

            [constants.REQUEST_OPERATION]: (state: OperationState, action: Action<number>): OperationState => ({
                ...state,
                isFetching: true
            }),
            [constants.RECEIVE_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => {
                const operation = action.payload as Operation;
                if (state.items.filter(x => x.id === operation.id).length === 0)
                    state.items.push(operation);
                //
                return { ...state };  // Shallow copy to force rerender
            },

            [constants.REQUEST_ADD_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => ({
                ...state,
                isFetching: true
            }),
            [constants.RESPONSE_ADD_OPERATION]: (state: OperationState, action: Action<number>): OperationState => ({
                ...state // Shallow copy to force rerender
            }),

            [constants.REQUEST_REMOVE_OPERATION]: (state: OperationState, action: Action<number>): OperationState => ({
                ...state,
                isFetching: true
            }),

            [constants.REQUEST_UPDATE_OPERATION]: (state: OperationState,
                                                   action: Action<Operation>): OperationState => ({
                    ...state,
                    isFetching: true
                }),

            [constants.RESPONSE_OPERATION]: (state: OperationState, action: Action<string>): OperationState => {
                state.isFetching = false;
                return { ...state };  // Shallow copy to force rerender
            },

            [constants.RESPONSE_ERROR_OPERATION]: (state: OperationState,
                                                   action: Action<ApplicationError>): OperationState => {
                alert((action.payload as ApplicationError).text);
                state.isFetching = false;
                return { ...state };  // Shallow copy to force rerender
            },

            [constants.REQUEST_EDIT_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => (
                {
                    ...state,
                    edit: {
                        ...state.edit,
                        open: true,
                        operation: action.payload as Operation,
                        isNew: (action.payload as Operation).id === -1
                    }
                }),

            [constants.SAVE_EDIT_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => (
                {
                    ...state,
                    isFetching: true
                }),

            [constants.CLOSE_EDIT_OPERATION]: (state: OperationState, action: Action<Operation>): OperationState => (
                {
                    ...state,
                    edit: {
                        ...state.edit,
                        open: false
                    }
                }),

        },
        initialState.operations);