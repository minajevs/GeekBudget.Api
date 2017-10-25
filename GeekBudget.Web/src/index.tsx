import * as React from 'react'
import * as ReactDOM from 'react-dom'
import App from './containers/App'
import registerServiceWorker from './registerServiceWorker'
import Api from './api'

// Router
import createHistory from 'history/createBrowserHistory'
import { Route } from 'react-router' // probably should use <Router> insted
import { ConnectedRouter, routerMiddleware, push } from 'react-router-redux'

// Redux
import thunk from 'redux-thunk';
import { Store, createStore, applyMiddleware, combineReducers } from 'redux'
import { Provider } from 'react-redux'
import { StoreState, initialState } from './types'
import { rootReducer } from './reducers'
import { getAllTabs, removeTab } from './actions/tabActions'

const history = createHistory();
const historyMiddleware = routerMiddleware(history)

const store:Store<StoreState> = createStore(
    rootReducer, 
    initialState,
    applyMiddleware(historyMiddleware, thunk)
);

//Init app with tabs
store.dispatch(getAllTabs());

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}> 
            <App />
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root') as HTMLElement
);
registerServiceWorker();
