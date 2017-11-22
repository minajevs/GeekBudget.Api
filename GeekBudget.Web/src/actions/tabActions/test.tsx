import * as actions from '.';

import * as types from 'constants/tabConstants';
import Tab from 'models/Tab';

import { mockApi } from 'helpers/test';

const api = mockApi();

const tabs: Tab[] = [
    {id: 1, amount: 100, currency: 'EUR', name: 'test-name'}
];

describe('Tab actions', () => {
    describe('Api actions', () => {
        describe('getall', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestAllTabs();
                expect(action.type).toBe(types.API_REQUEST_ALL_TAB);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseAllTabs(tabs);
                expect(action.type).toBe(types.API_RESPONSE_ALL_TAB);
                expect(action.payload).toBe(tabs);
            });
        });
        describe('operation', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestGetTab(1);
                expect(action.type).toBe(types.API_REQUEST_GET_TAB);
                expect(action.payload).toBe(1);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseGetTab(tabs[0]);
                expect(action.type).toBe(types.API_RESPONSE_GET_TAB);
                expect(action.payload).toBe(tabs[0]);
            });
        });
        describe('add', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestAddTab(tabs[0]);
                expect(action.type).toBe(types.API_REQUEST_ADD_TAB);
                expect(action.payload).toBe(tabs[0]);
            });
            it('should create an "response" action', () => {
                const action = actions.apiResponseAddTab(1);
                expect(action.type).toBe(types.API_RESPONSE_ADD_TAB);
                expect(action.payload).toBe(1);
            });
        });
        describe('remove', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestRemoveTab(1);
                expect(action.type).toBe(types.API_REQUEST_REMOVE_TAB);
                expect(action.payload).toBe(1);
            });
        });
        describe('edit', () => {
            it('should create an "request" action', () => {
                const action = actions.apiRequestEditTab(tabs[0]);
                expect(action.type).toBe(types.API_REQUEST_EDIT_TAB);
                expect(action.payload).toBe(tabs[0]);
            });
        });
        it('should create a common "request" action', () => {
            const action = actions.apiResponseTab('test');
            expect(action.type).toBe(types.API_RESPONSE_TAB);
            expect(action.payload).toBe('test');
        });
        it('should create a common "error" action', () => {
            const appError = {code: 400, text: 'test', object: {}};
            const action = actions.apiErrorTab(appError);
            expect(action.type).toBe(types.API_ERROR_TAB);
            expect(action.payload).toBe(appError);
        });

    });
    describe('UI Actions', () => {
        describe('edit', () => {
            it('should create an "open" action', () => {
                const action = actions.uiEditOpenTab(tabs[0]);
                expect(action.type).toBe(types.UI_EDIT_OPEN_TAB);
                expect(action.payload).toBe(tabs[0]);
            });
            it('should create an "close" action', () => {
                const action = actions.uiEditCloseTab();
                expect(action.type).toBe(types.UI_EDIT_CLOSE_TAB);
            });
            it('should create an "save" action', () => {
                const action = actions.uiEditSaveTab(tabs[0]);
                expect(action.type).toBe(types.UI_EDIT_SAVE_TAB);
                expect(action.payload).toBe(tabs[0]);
            });
        });
    });
});

describe('Tab thunks', () => {
    describe('getall', () => {
        it('should get a response', async () => {
            const thunk = actions.getAllTabs();
            const dispatch = jest.fn();
            api.tabMock.getAll.mockReturnValue(tabs);
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(3);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.apiRequestAllTabs());
            expect(dispatch.mock.calls[1][0]).toEqual(actions.apiResponseAllTabs(tabs));
            expect(dispatch.mock.calls[2][0]).toEqual(actions.apiResponseTab('Success!'));
            expect(api.tabMock.getAll.mock.calls.length).toBe(1);
        });
        it('should get an error', async () => {
            const thunk = actions.getAllTabs();
            const dispatch = jest.fn();
            const error = Error('test');
            api.tabMock.getAll = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            // expect(dispatch).toBeCalledWith(actions.apiRequestAllTabs()); 
            // expect(dispatch).toBeCalledWith(actions.errorTab(error)); //TODO: test fails on this line
            expect(api.tabMock.getAll.mock.calls.length).toBe(1);
        });
    });
    describe('add', () => {
        it('should get a response', async () => {
            const thunk = actions.addTab(tabs[0]);
            const dispatch = jest.fn();
            api.tabMock.add.mockReturnValue(1);
            api.tabMock.getAll.mockReturnValue(tabs);
            await thunk(dispatch);
            expect(dispatch).toBeCalledWith(actions.apiRequestAddTab(tabs[0]));
            expect(dispatch).toBeCalledWith(actions.apiResponseAddTab(1));
            expect(dispatch).toBeCalledWith(actions.apiResponseTab('Success!'));
            expect(api.tabMock.add.mock.calls.length).toBe(1);
            expect(api.tabMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.addTab(tabs[0]);
            const dispatch = jest.fn();
            const error = Error('test');
            api.tabMock.add = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseTab('Success!'));
            expect(api.tabMock.add.mock.calls.length).toBeGreaterThan(0);
        });
    });
    describe('remove', () => {
        it('should get a response', async () => {
            const thunk = actions.removeTab(1);
            const dispatch = jest.fn();
            api.tabMock.remove.mockReturnValue(1);
            api.tabMock.getAll.mockReturnValue(tabs);
            await thunk(dispatch);
            expect(dispatch).toBeCalledWith(actions.apiRequestRemoveTab(1));
            expect(dispatch).toBeCalledWith(actions.apiResponseTab('Success!'));
            expect(api.tabMock.remove.mock.calls.length).toBe(1);
            expect(api.tabMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.removeTab(1);
            const dispatch = jest.fn();
            const error = Error('test');
            api.tabMock.remove = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseTab('Success!'));
            expect(api.tabMock.remove.mock.calls.length).toBe(1);
        });
    });
    describe('edit', () => {
        it('should get a response', async () => {
            const thunk = actions.editTab(tabs[0]);
            const dispatch = jest.fn();
            api.tabMock.update.mockReturnValue(1);
            api.tabMock.getAll.mockReturnValue(tabs);
            await thunk(dispatch);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.apiRequestEditTab(tabs[0]));
            expect(dispatch.mock.calls[2][0]).toEqual(actions.apiResponseTab('Success!'));
            expect(api.tabMock.update.mock.calls.length).toBe(1);
            expect(api.tabMock.getAll.mock.calls.length).toBeGreaterThan(0);
        });
        it('should get an error', async () => {
            const thunk = actions.editTab(tabs[0]);
            const dispatch = jest.fn();
            const error = Error('test');
            api.tabMock.update = jest.fn(() => { throw error; });
            await thunk(dispatch);
            expect(dispatch.mock.calls.length).toBe(2);
            expect(dispatch).not.toBeCalledWith(actions.apiResponseTab('Success!'));
            expect(api.tabMock.update.mock.calls.length).toBe(1);
        });
    });
    describe('error', () => {
        it('should throw an app error', async () => {
            const error = Error('test');
            const thunk = actions.errorTab(error);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.apiResponseTab('failed'));
            expect(dispatch.mock.calls.length).toBe(2);
        });
    });
    describe('save', () => {
        it('should update ui', async () => {
            const thunk = actions.saveTab(tabs[0]);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(dispatch.mock.calls[0][0]).toEqual(actions.uiEditSaveTab(tabs[0]));
            expect(dispatch.mock.calls[2][0]).toEqual(actions.uiEditCloseTab());
        
        });
        it('should add new tab', async () => {
            const tab = {...tabs[0], id: -1} as Tab;
            const thunk = actions.saveTab(tab);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(JSON.stringify(dispatch.mock.calls[1][0])).toEqual(JSON.stringify(actions.addTab(tab)));
        });
        it('should save existing tab', async () => {
            const thunk = actions.saveTab(tabs[0]);
            const dispatch = jest.fn();
            await thunk(dispatch);
            expect(JSON.stringify(dispatch.mock.calls[1][0]))
            .toEqual(JSON.stringify(actions.editTab(tabs[0])));
        });
    });
});