//Related
import * as React from 'react';
import './App.css';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import * as actions from './actions';
import { StoreState } from './types/index';

//Components
import TestBool from './components/TestBool';

interface Props{
	dispatch: Dispatch<{}>;
	store: StoreState;
}

const logo = require('./logo.svg');

class App extends React.Component<Props> {
  render() {
	const { dispatch, store } = this.props;
	return (
	  <div className="App">
		<div className="App-header">
		  <img src={logo} className="App-logo" alt="logo" />
		  <h2>Welcome to React</h2>
		</div>
		<TestBool 
			testBool={store.testBool}
			onSet={() => dispatch(actions.setTestBool())}/>
	  </div>
	);
  }
}

const mapStateToProps = (state:StoreState) => ({
	store: state
});

export default connect(mapStateToProps)(App);