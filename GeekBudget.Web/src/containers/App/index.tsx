//Related
import * as React from 'react';
import './style.css';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

//Router
import { Route, Link, withRouter } from 'react-router-dom';

//Components
import Tab from '../../components/Tab';
import TabModel from '../../models/Tab';

import TabsContainer from '../../containers/Tabs'; 
import OperationsContainer from '../../containers/Operations'; 

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

const logo = require('./logo.svg');

const styles = {
    link: {
        backgroundColor: '#fff', 
        margin: '10px'
    }
}

class App extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;
        return (
            <div className="App">
                <div className="App-header">
                    <Link to={'/'}>
                        <img src={logo} className="App-logo" alt="logo" />
                    </Link>
                    <br/>
                    <Link to={'/tabs'} style={styles.link}>Tabs</Link>
                    <Link to={'/operations'} style={styles.link}>Operations</Link>
                </div>
                <br />
                <Route exact path={'/'} render={() => (<h1>Welcome!</h1>)}/>
                <Route path={'/tabs'} render={() => (<TabsContainer />)} />
                <Route path={'/operations'} render={() => (<OperationsContainer />)} />
            </div>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(App));