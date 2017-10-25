//Related
import * as React from 'react';
import './style.css';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

//Components
import Tab from '../../components/Tab';
import TabModel from '../../models/Tab';

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
		</div>
        <br/>
        <button onClick={() => dispatch(tabActions.addTab(new TabModel()))}>Add new</button>
        <button onClick={() => dispatch(tabActions.getAllTabs())}>Reload</button>
        <p>
            Fetching: <strong>{store.tabs.isFetching.toString()}</strong> 
            Tabs count: <strong>{store.tabs.items.length}</strong>
        </p>
        {store.tabs.items.map(t => {
            return <Tab 
                tab={t} 
                key={t.id}
                onRemove={() => dispatch(tabActions.removeTab(t.id))} 
                onSave={(tab) => dispatch(tabActions.updateTab(tab))}
                />
        })}
	  </div>
	);
  }
}

const mapStateToProps = (state:StoreState) => ({
	store: state
});

export default connect(mapStateToProps)(App);