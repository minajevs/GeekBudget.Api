import * as React from 'react';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from 'actions';
import { TabEditState, StoreState } from 'types/index';

import Dialog, {
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
} from 'material-ui/Dialog';

import Button from 'material-ui/Button';
import TextField from 'material-ui/TextField';

import TabModel from 'models/Tab';

interface Props {
    dispatch: Dispatch<{}>;
    store: TabEditState;
}

interface State {
    innerTab: TabModel | null;
}

class TabEdit extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            innerTab: props.store.tab
        };
    }

    componentWillReceiveProps(props: Props){
        this.setState({innerTab: props.store.tab});
    }

    handleChange = (name: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
        let updateTab = { ...this.state.innerTab } as TabModel;
        updateTab[name] = event.target.value;
        this.setState({ 
            innerTab: updateTab
         });
      }

    onSave = () => {
        this.props.dispatch(tabActions.saveTab(this.state.innerTab as TabModel));
    }

    onClose = () => {
        this.props.dispatch(tabActions.closeEditTab());
    }

    render() {
        const { innerTab } = this.state;
        if (innerTab == null) return null;
        const { open, isNew } = this.props.store;
        const title = isNew ? 'Add new tab' : `Edit "${innerTab.name}"`;
        return (
            <Dialog open={open} onRequestClose={this.onClose}>
                <DialogTitle>{title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Enter tab info
                    </DialogContentText>
                    <TextField
                        autoFocus
                        margin="dense"
                        id="name"
                        label="Tab Name"
                        onChange={this.handleChange('name')}
                        defaultValue={innerTab.name}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={this.onClose} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={this.onSave} color="primary">
                        Save
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state.tabs.edit
});

export default connect(mapStateToProps)(TabEdit);