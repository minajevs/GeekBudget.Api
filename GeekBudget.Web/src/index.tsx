import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './containers/App';
import registerServiceWorker from './registerServiceWorker';
import Api from './api';

// Redux
import { Store, createStore } from 'redux'
import { Provider } from 'react-redux';
import { StoreState, initialState } from './types';
import { rootReducer } from './reducers';

const store: Store<StoreState> = createStore(rootReducer, initialState);

Api.getAllTabs();

ReactDOM.render(
	<Provider store={store}>
		<App />
	</Provider>,
	document.getElementById('root') as HTMLElement
);
registerServiceWorker();
