// React
import * as React from 'react'
import * as ReactDOM from 'react-dom'

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
import { getAllOperations, addOperation } from './actions/operationActions'

// Other
import App from './containers/App'
import registerServiceWorker from './registerServiceWorker'
import Api from './api'

// Setup router and redux-router
const history = createHistory();
const historyMiddleware = routerMiddleware(history)

// Create redux store
const store:Store<StoreState> = createStore(
    rootReducer, 
    initialState,
    applyMiddleware(historyMiddleware, thunk)
);

// Init app with tabs
store.dispatch(getAllTabs());

store.dispatch(getAllOperations());


//store.dispatch(addOperation({
//    from: 11,
//    to: 12,
//    amount: 1,
//    comment: "test op",
//    currency: 'EUR',
//    date: new Date(),
//    id: 1
//}));


ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}> 
            <App />
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root') as HTMLElement
);
registerServiceWorker();
