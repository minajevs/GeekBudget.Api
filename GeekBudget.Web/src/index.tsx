import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './containers/App';
import registerServiceWorker from './registerServiceWorker';
import Api from './api';

// Redux
import thunk from 'redux-thunk';
import { Store, createStore, applyMiddleware  } from 'redux'
import { Provider } from 'react-redux';
import { StoreState, initialState } from './types';
import { rootReducer } from './reducers';
import { getAllTabs, removeTab } from './actions/tabActions';

const store: Store<StoreState> = createStore(rootReducer, initialState, applyMiddleware(thunk));

store.dispatch(getAllTabs());

ReactDOM.render(
	<Provider store={store}>
		<App />
	</Provider>,
	document.getElementById('root') as HTMLElement
);
registerServiceWorker();
