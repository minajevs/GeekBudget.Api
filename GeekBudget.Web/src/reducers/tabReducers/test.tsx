import { tabReducers } from '.';
import * as actions from 'actions/tabActions';
import { initialState, ApplicationError, ApplicationErrorState, TabState } from 'types';
import { Action } from 'redux-actions';
import Tab from 'models/Tab';

const tabs: Tab[] = [
    { id: 1, amount: 100, currency: 'EUR', name: 'test1'},
    { id: 2, amount: 200, currency: 'EUR', name: 'test2'},
    { id: 3, amount: 300, currency: 'EUR', name: 'test3'},
];

describe('Tab reducers', () => {
    describe('api', () => {
        describe('get all', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestAllTabs();
                const oldState: TabState = { ...initialState.tabs, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(3); // Does not rewrite 
            });
            it('should reduce response', () => {
                const action = actions.apiResponseAllTabs([...tabs]);
                const oldState: TabState = { ...initialState.tabs, isFetching: true, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(3);
                expect(newState.items).not.toBe(tabs);
            });
            it('should not override existing items if result is falsy', () => {
                // tslint:disable-next-line:no-string-literal
                const action = (actions as {})['apiResponseAllTabs'](undefined);
                const oldState: TabState = { ...initialState.tabs, isFetching: true, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.items).toBe(tabs);
            });
        });

        describe('get', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestGetTab(1);
                const oldState: TabState = { ...initialState.tabs, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.items.length).toEqual(3);
                expect(newState.isFetching).toEqual(true);
            });
            it('should reduce response', () => {
                const action = actions.apiResponseGetTab({ ...tabs[0], id: 4 });
                const oldState: TabState = { ...initialState.tabs, items: tabs, isFetching: true };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
                expect(newState.items.length).toEqual(4);
                // TODO: Should sort array again
            });
            it('reduce response should not add duplicates', () => {
                const action = actions.apiResponseGetTab(tabs[0]);
                const oldState: TabState = { ...initialState.tabs, isFetching: true, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.items.length).toEqual(3);
            });
        });
        describe('add', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestAddTab(new Tab());
                const newState = tabReducers(initialState.tabs, action);
                expect(newState.isFetching).toEqual(true);
            });
            it('should reduce response', () => {
                const action = actions.apiResponseAddTab(3);
                const oldState: TabState = { ...initialState.tabs, isFetching: true, items: tabs };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('remove', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestRemoveTab(1);
                const newState = tabReducers(initialState.tabs, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('edit', () => {
            it('should reduce request', () => {
                const action = actions.apiRequestEditTab(new Tab());
                const newState = tabReducers(initialState.tabs, action);
                expect(newState.isFetching).toEqual(true);
            });
        });
        describe('response', () => {
            it('should reduce response', () => {
                const action = actions.apiResponseTab('');
                const oldState: TabState = { ...initialState.tabs, isFetching: true };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(false);
            });
        });
        describe('error', () => {
            it('should reduce error', () => {
                const action = actions.apiErrorTab({ code: 400, object: {}, text: 'test-error' });
                const alertMock = jest.fn();
                window.alert = alertMock;
                const oldState: TabState = { ...initialState.tabs, isFetching: true };
                const newState = tabReducers(oldState, action);
                expect(newState.isFetching).toEqual(false);
                expect(alertMock).toBeCalledWith('test-error');
            });
        });
    });
    describe('ui', () => {
        describe('edit', () => {
            describe('open', () => {
                it('should reduce existing tab', () => {
                    const action = actions.uiEditOpenTab(tabs[0]);
                    const newState = tabReducers(initialState.tabs, action);
                    expect(newState.edit.open).toEqual(true);
                    expect(newState.edit.isNew).toEqual(false);
                    expect(newState.edit.tab).toBe(tabs[0]);
                });
                it('should reduce new tab', () => {
                    const action = actions.uiEditOpenTab({ ...tabs[0], id: -1 });
                    const newState = tabReducers(initialState.tabs, action);
                    expect(newState.edit.open).toEqual(true);
                    expect(newState.edit.isNew).toEqual(true);
                    expect(newState.edit.tab).not.toBeNull();
                });
            });
            describe('save', () => {
                it('should reduce', () => {
                    const action = actions.uiEditSaveTab(tabs[0]);
                    const newState = tabReducers(initialState.tabs, action);
                    expect(newState.isFetching).toEqual(true);
                });
            });
            describe('close', () => {
                it('should reduce', () => {
                    const action = actions.uiEditCloseTab();
                    const oldState: TabState = {
                        ...initialState.tabs,
                        edit: {
                            ...initialState.tabs.edit,
                            isNew: true,
                            open: true
                        }
                    };
                    const newState = tabReducers(initialState.tabs, action);
                    expect(newState.edit.open).toEqual(false);
                    expect(newState.edit.isNew).toEqual(true);
                });
            });
        });
    });
});