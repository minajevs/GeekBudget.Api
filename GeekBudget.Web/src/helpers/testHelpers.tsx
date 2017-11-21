import * as React from 'react';
import * as ReactDOM from 'react-dom';
import createHistory from 'history/createBrowserHistory';
import { MemoryRouter } from 'react-router';
import { Provider } from 'react-redux';
import thunk from 'redux-thunk';
import { StoreState, initialState } from 'types/index';
import { Store } from 'redux';
import { createStore } from 'redux';
import { rootReducer } from 'reducers/index';
import { applyMiddleware } from 'redux';
import { routerMiddleware } from 'react-router-redux';
import { shallow, mount, ShallowWrapper } from 'enzyme';

import Api from 'api';

const history = createHistory();
const historyMiddleware = routerMiddleware(history);

export const shallowWithStore = (component: React.ReactElement<{}>, state: StoreState = initialState) => {
    const store = createStore(
        rootReducer, 
        state,
        applyMiddleware(historyMiddleware, thunk)
    );
    const context = { store };
    return shallow(component, { context });
};

export const mountWithStore = (component: React.ReactElement<{}>, state: StoreState = initialState) => {
    const store = createStore(
        rootReducer, 
        state,
        applyMiddleware(historyMiddleware, thunk)
    );
    const context = { store };
    return mount(component, { context });
};

export const getMaterialUIValue = (element: ShallowWrapper): string => {
    return element.shallow().shallow().text();
};

export const mockApi = () => {
    const operationMock = {
        getAll: jest.fn(),
        add: jest.fn(),
        remove: jest.fn(),
        update: jest.fn()
    };
    const tabMock = {
        getAll: jest.fn(),
        add: jest.fn(),
        remove: jest.fn(),
        update: jest.fn()
    };
    Api.operation = operationMock;
    Api.tab = tabMock;

    return { operationMock, tabMock };
};