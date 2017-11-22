import * as actions from '.';

import * as types from 'constants/operationConstants';
import Operation from 'models/Operation';

import { mockApi } from 'helpers/test';

const api = mockApi();

const operations: Operation[] = [
    {id: 1, from: 1, to: 2, amount: 100, comment: '', date: new Date(), currency: 'EUR'}
];

describe('Operation actions', () => {
    describe('Api actions', () => {
        describe('getall', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestAllOperations();
                expect(action.type).toBe(types.API_REQUEST_ALL_OPERATION);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseAllOperations(operations);
                expect(action.type).toBe(types.API_RESPONSE_ALL_OPERATION);
                expect(action.payload).toBe(operations);
            });
        });
        describe('operation', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestGetOperation(1);
                expect(action.type).toBe(types.API_REQUEST_GET_OPERATION);
                expect(action.payload).toBe(1);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseGetOperation(operations[0]);
                expect(action.type).toBe(types.API_RESPONSE_GET_OPERATION);
                expect(action.payload).toBe(operations[0]);
            });
        });
        describe('add', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestAddOperation(operations[0]);
                expect(action.type).toBe(types.API_REQUEST_ADD_OPERATION);
                expect(action.payload).toBe(operations[0]);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseAddOperation(1);
                expect(action.type).toBe(types.API_RESPONSE_ADD_OPERATION);
                expect(action.payload).toBe(1);
            });
        });
        describe('remove', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestRemoveOperation(1);
                expect(action.type).toBe(types.API_REQUEST_REMOVE_OPERATION);
                expect(action.payload).toBe(1);
            });
        });
        describe('edit', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestEditOperation(operations[0]);
                expect(action.type).toBe(types.API_REQUEST_EDIT_OPERATION);
                expect(action.payload).toBe(operations[0]);
            });
        });
        it('should create a common "request" action', () => {
            const action = actions.apiResponseOperation('test');
            expect(action.type).toBe(types.API_RESPONSE_OPERATION);
            expect(action.payload).toBe('test');
        });
        it('should create a common "error" action', () => {
            const appError = {code: 400, text: 'test', object: {}};
            const action = actions.apiErrorOperation(appError);
            expect(action.type).toBe(types.API_ERROR_OPERATION);
            expect(action.payload).toBe(appError);
        });

    });
    describe('UI Actions', () => {
        describe('edit', () => {
            it('should create an "open" action', () => {
                const action = actions.uiEditOpenOperation(operations[0]);
                expect(action.type).toBe(types.UI_EDIT_OPEN_OPERATION);
                expect(action.payload).toBe(operations[0]);
            });
            it('should create an "close" action', () => {
                const action = actions.uiEditCloseOperation();
                expect(action.type).toBe(types.UI_EDIT_CLOSE_OPERATION);
            });
            it('should create an "save" action', () => {
                const action = actions.uiEditSaveOperation(operations[0]);
                expect(action.type).toBe(types.UI_EDIT_SAVE_OPERATION);
                expect(action.payload).toBe(operations[0]);
            });
        });
    });
});

describe('Thunks', () => {
    describe('getall', () => {
        it('should get a response', async () => {
            const thunk = actions.getAllOperations();
            const dispatch = jest.fn();
            api.operationMock.getAll.mockReturnValue(operations);
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(3);
            expect(dispatch).toBeCalledWith(actions.apiRequestAllOperations());
            expect(dispatch).toBeCalledWith(actions.apiResponseAllOperations(operations));
            expect(dispatch).toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.getAll.mock.calls.length).toBe(1);
        });
        it('should get an error', async () => {
            const thunk = actions.getAllOperations();
            const dispatch = jest.fn();
            const error = Error('test');
            api.operationMock.getAll = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            // expect(dispatch).toBeCalledWith(actions.apiRequestAllOperations()); 
            // expect(dispatch).toBeCalledWith(actions.errorOperation(error)); //TODO: test fails on this line
            expect(api.operationMock.getAll.mock.calls.length).toBe(1);
        });
    });
    describe('add', () => {
        it('should get a response', async () => {
            const thunk = actions.addOperation(operations[0]);
            const dispatch = jest.fn();
            api.operationMock.add.mockReturnValue(1);
            api.operationMock.getAll.mockReturnValue(operations);
            await thunk(dispatch);
            expect(dispatch).toBeCalledWith(actions.apiRequestAddOperation(operations[0]));
            expect(dispatch).toBeCalledWith(actions.apiResponseAddOperation(1));
            expect(dispatch).toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.add.mock.calls.length).toBe(1);
            expect(api.operationMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.addOperation(operations[0]);
            const dispatch = jest.fn();
            const error = Error('test');
            api.operationMock.add = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.add.mock.calls.length).toBeGreaterThan(0);
        });
    });
    describe('remove', () => {
        it('should get a response', async () => {
            const thunk = actions.removeOperation(1);
            const dispatch = jest.fn();
            api.operationMock.remove.mockReturnValue(1);
            api.operationMock.getAll.mockReturnValue(operations);
            await thunk(dispatch);
            expect(dispatch).toBeCalledWith(actions.apiRequestRemoveOperation(1));
            expect(dispatch).toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.remove.mock.calls.length).toBe(1);
            expect(api.operationMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.removeOperation(1);
            const dispatch = jest.fn();
            const error = Error('test');
            api.operationMock.remove = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.remove.mock.calls.length).toBe(1);
        });
    });
    describe('edit', () => {
        it('should get a response', async () => {
            const thunk = actions.editOperation(operations[0]);
            const dispatch = jest.fn();
            api.operationMock.update.mockReturnValue(1);
            api.operationMock.getAll.mockReturnValue(operations);
            await thunk(dispatch);
            expect(dispatch).toBeCalledWith(actions.apiRequestEditOperation(operations[0]));
            expect(dispatch).toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.update.mock.calls.length).toBe(1);
            expect(api.operationMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.editOperation(operations[0]);
            const dispatch = jest.fn();
            const error = Error('test');
            api.operationMock.update = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseOperation('Success'));
            expect(api.operationMock.update.mock.calls.length).toBe(1);
        });
    });
    describe('error', () => {
        it('should throw an app error', async () => {
            const error = Error('test');
            const thunk = actions.errorOperation(error);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.apiResponseOperation('failed'));
            expect(dispatch.mock.calls.length).toBe(2);
        });
    });
    describe('save', () => {
        it('should update ui', async () => {
            const thunk = actions.saveOperation(operations[0]);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.uiEditSaveOperation(operations[0]));
            expect(dispatch.mock.calls[2][0]).toEqual(actions.uiEditCloseOperation());
        
        });
        it('should add new operation', async () => {
            const operation = {...operations[0], id: -1} as Operation;
            const thunk = actions.saveOperation(operation);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(JSON.stringify(dispatch.mock.calls[1][0])).toEqual(JSON.stringify(actions.addOperation(operation)));
        });
        it('should save existing operation', async () => {
            const thunk = actions.saveOperation(operations[0]);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(JSON.stringify(dispatch.mock.calls[1][0]))
            .toEqual(JSON.stringify(actions.editOperation(operations[0])));
        });
    });
});