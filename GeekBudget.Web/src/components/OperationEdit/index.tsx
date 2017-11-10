import * as React from 'react';
import { dateToString } from 'helpers';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { operationActions } from 'actions';
import { OperationEditState, StoreState } from 'types/index';

import Dialog, {
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
} from 'material-ui/Dialog';
import Button from 'material-ui/Button';
import Grid from 'material-ui/Grid';
import TextField from 'material-ui/TextField';
import Input, { InputLabel, InputAdornment } from 'material-ui/Input';
import { FormControl, FormHelperText } from 'material-ui/Form';
import Select from 'material-ui/Select';
import { MenuItem } from 'material-ui/Menu';

import OperationModel from 'models/Operation';
import TabModel from 'models/Tab';
import TabSelector from 'components/TabSelector';

interface Props {
    dispatch: Dispatch<{}>;
    store: OperationEditState;
    tabs: TabModel[];
}

interface State{
    innerOperation: OperationModel | null;
}

class OperationEdit extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            innerOperation: props.store.operation
        };
    }

    componentWillReceiveProps(props: Props) {
        this.setState({ innerOperation: props.store.operation });
    }

    handleChange = (name: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = name !== 'date'
            ? event.target.value
            : new Date(event.target.value);
        let updateOperation = { ...this.state.innerOperation } as OperationModel;
        updateOperation[name] = newValue;
        this.setState({
            innerOperation: updateOperation
        });
    }

    onSave = () => {
        this.props.dispatch(operationActions.saveOperation(this.state.innerOperation as OperationModel));
    }

    onClose = () => {
        this.props.dispatch(operationActions.closeEditOperation());
    }
    // TODO: https://reactjs.org/docs/portals.html
    render() {
        const { innerOperation } = this.state;
        if (innerOperation == null) return (<div/>);
        const { store, tabs } = this.props;
        const {open, isNew} = store;
        const title = isNew ? 'Add new operation' : 'Edit operation';
        return (
            <Dialog open={open} onRequestClose={this.onClose} fullWidth>
                <DialogTitle>{title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Enter operation info
                    </DialogContentText>
                    <Grid container>
                        <Grid item xs={6}>
                            <TabSelector
                                tabs={tabs}
                                label="From"
                                onChange={this.handleChange('from')}
                                defaultValue={innerOperation.from || 0}
                            />
                        </Grid>
                        <Grid item xs={6}>
                            <TabSelector
                                tabs={tabs}
                                label="To"
                                onChange={this.handleChange('to')}
                                defaultValue={innerOperation.to || 0}
                            />
                        </Grid>
                    </Grid>
                    <TextField
                        margin="dense"
                        id="comment"
                        label="Operation Comment"
                        onChange={this.handleChange('comment')}
                        defaultValue={innerOperation.comment}
                        fullWidth
                    />
                    <Grid container>
                        <Grid item xs={6}>
                            <FormControl fullWidth margin="dense">
                                <InputLabel htmlFor="amount">Amount</InputLabel>
                                <Input
                                    id="amount"
                                    defaultValue={innerOperation.amount}
                                    onChange={this.handleChange('amount')}
                                    startAdornment={<InputAdornment position="start">€</InputAdornment>}
                                />
                            </FormControl>
                        </Grid>
                        <Grid item xs={6}>
                            <TextField
                                margin="dense"
                                id="date"
                                type="date"
                                label="Date"
                                onChange={this.handleChange('date')}
                                defaultValue={(() => {
                                    const date = innerOperation.date || new Date();
                                    return dateToString(date);
                                })()}
                                InputLabelProps={{shrink: true}}
                                fullWidth
                            />
                        </Grid>
                    </Grid>
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
    store: state.operations.edit,
    tabs: state.tabs.items
});

export default connect(mapStateToProps)(OperationEdit);