//Related
import * as React from 'react';
import Grid from 'material-ui/Grid';
import Button from 'material-ui/Button';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

//Router
import { Route, Link, withRouter } from 'react-router-dom';

//Components
import Tab from '../../components/Tab';
import ConfirmationDialog from '../../components/ConfirmationDialog';
import TabEdit from '../../components/TabEdit';
import TabModel from '../../models/Tab';

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

class Tabs extends React.Component<Props, State> {
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
        console.log('hit');
        this.setState({ editTabRequestOpen: false});
    }

    render() {
        const { dispatch, store } = this.props;
        const { tabToRemove, removeTabRequestOpen, tabToEdit, editTabRequestOpen } = this.state;
        return (
            <Grid container>
                <Grid item xs={12}>
                    <button onClick={() => dispatch(tabActions.addTab(new TabModel()))}>Add new</button>
                    <button onClick={() => dispatch(tabActions.getAllTabs())}>Reload</button>
                    <p>
                        Fetching: <strong>{store.tabs.isFetching.toString()}</strong>
                        Tabs count: <strong>{store.tabs.items.length}</strong>
                    </p>
                </Grid>
                {store.tabs.items.map(t => {
                    return (<Grid item xs={12} sm={3} md={4} lg={4} key={t.id}>
                        <Tab
                            tab={t}
                            onRemoveClick={() => this.onTabRemoveRequest(t)}
                            onEditClick={() => this.onTabEditRequest(t)}
                        />
                    </Grid>)
                })}
                <ConfirmationDialog 
                    open={removeTabRequestOpen}
                    onClose={this.onTabRemoveClose}
                    onConfirm={this.onTabRemoveConfirm}
                    title={`Do you really want to remove "${tabToRemove.name}"?`}
                    text={`After tab is deleted you can't restore it!`}  
                />
                <TabEdit 
                    open={editTabRequestOpen}
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

export default withRouter(connect(mapStateToProps)(Tabs));