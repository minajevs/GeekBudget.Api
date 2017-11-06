// React
import * as React from 'react';
// import { withStyles } from 'material-ui/styles'; TODO!!
import Grid from 'material-ui/Grid';
import Typography from 'material-ui/Typography';
import { grey } from 'material-ui/colors';
import './style.css';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

// Router
import { Route, Link, withRouter } from 'react-router-dom';

// Components
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
        textAlign: 'center',
        zIndex: -99
    } as React.CSSProperties,
    link: {
        backgroundColor: '#fff',
        margin: '10px'
    }
};

class App extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;
        return (
            <Grid container style={styles.app} spacing={40}>
                <Grid item xs={12} style={styles.header}>
                    <Typography type="display3" style={{color: grey[50]}}>
                        Geek<div className="App-logo" style={{display: 'inline-block'}}>Budget</div>
                    </Typography>
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