import * as React from 'react';
import * as ReactDOM from 'react-dom';
import createHistory from 'history/createBrowserHistory';
import { MemoryRouter } from 'react-router';
import { Provider } from 'react-redux';
import thunk from 'redux-thunk';
import App from '.';
import { StoreState, initialState } from 'types/index';
import { Store } from 'redux';
import { createStore } from 'redux';
import { rootReducer } from 'reducers/index';
import { applyMiddleware } from 'redux';
import { routerMiddleware } from 'react-router-redux';
import { shallowWithStore } from 'helpers/test';

const history = createHistory();
const historyMiddleware = routerMiddleware(history);

const store: Store<StoreState> = createStore(
    rootReducer,
    initialState,
    applyMiddleware(historyMiddleware, thunk)
);

it('renders without crashing', () => {
    const element = <MemoryRouter><App /></MemoryRouter>;
    shallowWithStore(element);
});