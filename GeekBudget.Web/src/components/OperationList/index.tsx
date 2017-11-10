import * as React from 'react';
import Grid from 'material-ui/Grid';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from 'actions';
import { StoreState } from 'types/index';

// Router
import { Route, Redirect, Link, withRouter, RouteComponentProps } from 'react-router-dom';

import OperationModel from 'models/Operation';
import Operation from 'components/Operation';
import OperationEdit from 'components/OperationEdit';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

interface State {
    
}

class OperationList extends React.Component<Props> {
    render() {
        const { store } = this.props;
        return (
            <Grid container>
                {store.operations.items.map(o => {
                    return (
                        <Grid item xs={12} lg={12} key={o.id}>
                            <Operation operation={o}/>
                        </Grid>
                    );
                })}
            </Grid>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(OperationList));