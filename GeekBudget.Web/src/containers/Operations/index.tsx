// Related
import * as React from 'react';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';
import AddIcon from 'material-ui-icons/Add';
import { CircularProgress } from 'material-ui/Progress';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { operationActions } from 'actions';
import { StoreState } from 'types/index';

// Router
import { Route, Link, withRouter } from 'react-router-dom';
import { push } from 'react-router-redux';

// Components
import Operation from 'components/Operation';
import OperationList from 'components/OperationList';
import OperationEdit from 'components/OperationEdit';
import OperationModel from 'models/Operation';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

class Operations extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;
        return (
            <Grid container>
                <Grid item xs={12}>
                    <Button 
                        raised 
                        onClick={() => dispatch(operationActions.uiEditOpenOperation(new OperationModel()))}
                    >
                        <AddIcon />
                        Add new operation
                    </Button>
                </Grid>
                <Grid item xs={12}>
                    {store.operations.isFetching ?
                        <Grid container alignItems="center" justify="center" spacing={0}>
                            <Grid item>
                                <CircularProgress size={100} />
                            </Grid>
                        </Grid>
                        :
                        <OperationList />
                    }
                </Grid>
                <OperationEdit />
            </Grid>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(Operations));