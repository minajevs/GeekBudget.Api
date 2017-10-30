//React
import * as React from 'react';
// import { withStyles } from 'material-ui/styles'; TODO!!
import Grid from 'material-ui/Grid';
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
    app: {
        fontFamily: 'roboto'
    },
    header: {
        backgroundColor: '#222',
        textAlign: 'center'
    } as React.CSSProperties,
    link: {
        backgroundColor: '#fff',
        margin: '10px'
    }
}

class App extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;
        return (
            <Grid container style={styles.app}>
                <Grid item xs={12} style={styles.header}>
                    <Link to={'/'}>
                        <img src={logo} className="App-logo" alt="logo" />
                    </Link>
                </Grid>
                <Grid item md={6} xs={12}>
                    <TabsContainer />
                </Grid>
                <Grid item md={6} xs={12}>
                    <OperationsContainer />
                </Grid>
            </Grid>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(App));