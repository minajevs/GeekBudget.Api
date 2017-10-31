import * as React from 'react';
import Grid from 'material-ui/Grid';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../actions';
import { StoreState } from '../types/index';

//Router
import { Route, Redirect, Link, withRouter, RouteComponentProps } from 'react-router-dom';

import TabModel from '../models/Tab';
import Tab from '../components/Tab';
import TabAddButton from '../components/TabAdd';
import ConfirmationDialog from '../components/ConfirmationDialog';
import TabEdit from '../components/TabEdit';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

interface State {
    removeTabRequestOpen: boolean;
    tabToRemove: TabModel;

    editTabRequestOpen: boolean;
    tabToEdit: TabModel;
}

class TabList extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            removeTabRequestOpen: false,
            tabToRemove: new TabModel(),

            editTabRequestOpen: false,
            tabToEdit: new TabModel()
        }
    }

    onTabRemoveRequest = (tab: TabModel) => {
        this.setState({ removeTabRequestOpen: true, tabToRemove: tab });
    }

    onTabRemoveConfirm = () => {
        this.props.dispatch(tabActions.removeTab(this.state.tabToRemove.id));
    }

    onTabRemoveClose = () => {
        this.setState({ removeTabRequestOpen: false });
    }

    onTabEditRequest = (tab: TabModel) => {
        this.setState({ editTabRequestOpen: true, tabToEdit: tab });
    }

    onTabEditSave = (tab: TabModel) => {
        this.props.dispatch(tabActions.updateTab(tab));
    }

    onTabEditClose = () => {
        this.setState({ editTabRequestOpen: false });
    }

    render() {
        const { store } = this.props;
        const { removeTabRequestOpen, tabToRemove, editTabRequestOpen, tabToEdit } = this.state;
        return (
            <Grid container>
                {store.tabs.items.map(t => {
                    return (
                        <Grid item xs={12} sm={3} md={4} lg={4} key={t.id}>
                            <Tab
                                tab={t}
                                onRemoveClick={() => this.onTabRemoveRequest(t)}
                                onEditClick={() => this.onTabEditRequest(t)}
                            />
                        </Grid>
                    )
                })}
                <Grid item xs={12} sm={3} md={4} lg={4}>
                    <Grid container alignItems="center" justify="center" spacing={0} style={{ height: '100%' }}>
                        <Grid item>
                            <TabAddButton />
                        </Grid>
                    </Grid>
                </Grid>
                <ConfirmationDialog
                        open={removeTabRequestOpen}
                        onClose={this.onTabRemoveClose}
                        onConfirm={this.onTabRemoveConfirm}
                        title={`Do you really want to remove "${tabToRemove.name}"?`}
                        text={`After tab is deleted you can't restore it!`}
                    />
                <TabEdit
                    open={this.state.editTabRequestOpen}
                    tab={tabToEdit}
                    onSave={(t) => this.onTabEditSave(t)}
                    onClose={this.onTabEditClose}
                    title={`Edit ${tabToEdit.name != null ? '"' + tabToEdit.name + '"' : ' tab'}`}
                    text={`Edit this tab and press save.`}
                />
            </Grid>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(TabList));