import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import './index.css';

// Redux
import { Store, createStore } from 'redux'
import { Provider } from 'react-redux';
import { initialState, StoreState } from './types';
import handleActions from './reducers/testReducers';

const store: Store<StoreState> = createStore(handleActions, initialState);

ReactDOM.render(
	<Provider store={store}>
		<App />
	</Provider>,
	document.getElementById('root') as HTMLElement
);
registerServiceWorker();
