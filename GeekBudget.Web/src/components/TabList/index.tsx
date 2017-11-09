import * as React from 'react';
import Grid from 'material-ui/Grid';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

// Router
import { Route, Redirect, Link, withRouter, RouteComponentProps } from 'react-router-dom';

import TabModel from '../../models/Tab';
import Tab from '../../components/Tab';
import TabAddButton from '../../components/TabAdd';
import ConfirmationDialog from '../../components/ConfirmationDialog';
import TabEdit from '../../components/TabEdit';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

class TabList extends React.Component<Props> {
    constructor(props: Props) {
        super(props);
    }
    render() {
        const { store } = this.props;
        return (
            <Grid container>
                {store.tabs.items.map(t => {
                    return (
                        <Grid item xs={12} sm={3} md={4} lg={4} key={t.id}>
                            <Tab tab={t} />
                        </Grid>
                    );
                })}
                <Grid item xs={12} sm={3} md={4} lg={4}>
                    <Grid container alignItems="center" justify="center" spacing={0} style={{ height: '100%' }}>
                        <Grid item>
                            <TabAddButton />
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(TabList));