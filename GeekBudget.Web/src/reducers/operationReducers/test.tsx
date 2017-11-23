import { operationReducers } from '.';
import * as actions from 'actions/operationActions';
import { initialState, ApplicationError, ApplicationErrorState, OperationState } from 'types';
import { Action } from 'redux-actions';
import Operation from 'models/Operation';

const operations: Operation[] = [
    { id: 1, date: new Date(1990, 1, 1), amount: 100, comment: '', currency: 'EUR', from: 1, to: 2 },
    { id: 2, date: new Date(1992, 1, 1), amount: 100, comment: '', currency: 'EUR', from: 1, to: 2 },
    { id: 3, date: new Date(1995, 1, 1), amount: 100, comment: '', currency: 'EUR', from: 1, to: 2 },
];

describe('Operation reducers', () => {
    describe('api', () => {
        describe('get all', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestAllOperations();
                const oldState: OperationState = { ...initialState.operations, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(0);
            });
            it('should reduce response', () => {
                const action = actions.apiResponseAllOperations(operations);
                const oldState: OperationState = { ...initialState.operations, isFetching: true, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(3);
            });
            it('reduce response should sort by date', () => {
                const action = actions.apiResponseAllOperations(operations);
                const oldState: OperationState = { ...initialState.operations, isFetching: true, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.items[0].id).toEqual(3);
                expect(newState.items[1].id).toEqual(2);
                expect(newState.items[2].id).toEqual(1);
            });
            it('should not override existing items if result is falsy', () => {
                // tslint:disable-next-line:no-string-literal
                const action = (actions as {})['apiResponseAllOperations'](undefined);
                const oldState: OperationState = { ...initialState.operations, isFetching: true, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.items).toBe(operations);
            });
        });

        describe('get', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestGetOperation(1);
                const oldState: OperationState = { ...initialState.operations, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.items.length).toEqual(3);
                expect(newState.isFetching).toEqual(true);
            });
            it('should reduce response', () => {
                const action = actions.apiResponseGetOperation({ ...operations[0], id: 4 });
                const oldState: OperationState = { ...initialState.operations, items: operations, isFetching: true };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(4);
                // TODO: Should sort array again
            });
            it('reduce response should not add duplicates', () => {
                const action = actions.apiResponseGetOperation(operations[0]);
                const oldState: OperationState = { ...initialState.operations, isFetching: true, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.items.length).toEqual(3);
            });
        });
        describe('add', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestAddOperation(new Operation());
                const newState = operationReducers(initialState.operations, action);
                expect(newState.isFetching).toEqual(true);
            });
            it('should reduce response', () => {
                const action = actions.apiResponseAddOperation(3);
                const oldState: OperationState = { ...initialState.operations, isFetching: true, items: operations };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('remove', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestRemoveOperation(1);
                const newState = operationReducers(initialState.operations, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('edit', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestEditOperation(new Operation());
                const newState = operationReducers(initialState.operations, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('response', () => {
            it('should reduce response', () => {
                const action = actions.apiResponseOperation('');
                const oldState: OperationState = { ...initialState.operations, isFetching: true };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(false);
            });
        });
        describe('error', () => {
            it('should reduce error', () => {
                const action = actions.apiErrorOperation({ code: 400, object: {}, text: 'test-error' });
                const alertMock = jest.fn();
                window.alert = alertMock;
                const oldState: OperationState = { ...initialState.operations, isFetching: true };
                const newState = operationReducers(oldState, action);
                expect(newState.isFetching).toEqual(false);
                expect(alertMock).toBeCalledWith('test-error');
            });
        });
    });
    describe('ui', () => {
        describe('edit', () => {
            describe('open', () => {
                it('should reduce existing tab', () => {
                    const action = actions.uiEditOpenOperation(operations[0]);
                    const newState = operationReducers(initialState.operations, action);
                    expect(newState.edit.open).toEqual(true);
                    expect(newState.edit.isNew).toEqual(false);
                    expect(newState.edit.operation).toBe(operations[0]);
                });
                it('should reduce new tab', () => {
                    const action = actions.uiEditOpenOperation({ ...operations[0], id: -1 });
                    const newState = operationReducers(initialState.operations, action);
                    expect(newState.edit.open).toEqual(true);
                    expect(newState.edit.isNew).toEqual(true);
                    expect(newState.edit.operation).not.toBeNull();
                });
            });
            describe('save', () => {
                it('should reduce', () => {
                    const action = actions.uiEditSaveOperation(operations[0]);
                    const newState = operationReducers(initialState.operations, action);
                    expect(newState.isFetching).toEqual(true);
                });
            });
            describe('close', () => {
                it('should reduce', () => {
                    const action = actions.uiEditCloseOperation();
                    const oldState: OperationState = {
                        ...initialState.operations,
                        edit: {
                            ...initialState.operations.edit,
                            isNew: true,
                            open: true
                        }
                    };
                    const newState = operationReducers(initialState.operations, action);
                    expect(newState.edit.open).toEqual(false);
                    expect(newState.edit.isNew).toEqual(true);
                });
            });
        });
    });
});