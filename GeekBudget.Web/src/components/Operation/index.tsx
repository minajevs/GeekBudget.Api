import * as React from 'react';
import { dateToString } from '../../helpers';

import Grid from 'material-ui/Grid';
import Paper from 'material-ui/Paper';
import EuroSymbol from 'material-ui-icons/EuroSymbol';
import Avatar from 'material-ui/Avatar';
import Typography from 'material-ui/Typography';
import IconButton from 'material-ui/IconButton';
import ModeEdit from 'material-ui-icons/ModeEdit';
import DeleteForever from 'material-ui-icons/DeleteForever';
import { green } from 'material-ui/colors';

import OperationModel from '../../models/Operation';
import OperationEdit from '../../components/OperationEdit';
import ConfirmationDialog from '../../components/ConfirmationDialog';
import TabModel from '../../models/Tab';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { operationActions } from '../../actions';
import { StoreState } from '../../types/index';

interface Props {
    operation: OperationModel;
    dispatch: Dispatch<{}>;
    tabs: TabModel[];
}

interface State {
    openConfirmationDialog: boolean;
}

const styles = {
    paper: {
        padding: 16
    }
};

class Operation extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            openConfirmationDialog: false
        };
    }
    onEditClick = () => {
        this.props.dispatch(operationActions.requestEditOperation(this.props.operation));
    }

    onRemoveClick = () => {
        this.setState({ openConfirmationDialog: true });
    }

    onRemoveConfirm = () => {
        this.props.dispatch(operationActions.removeOperation(this.props.operation.id));
        this.onRemoveClose();
    }

    onRemoveClose = () => {
        this.setState({ openConfirmationDialog: false });
    }

    render() {
        const { operation, tabs } = this.props;
        const fromTab = tabs.find(t => t.id === operation.from) as TabModel;
        const toTab = tabs.find(t => t.id === operation.to) as TabModel;

        return (
            <Paper style={styles.paper}>
                <Grid container alignItems="center">
                    <Grid item xs={1}>
                        <Avatar 
                            aria-label="OperationIcon" 
                            style={{ color: green[900], backgroundColor: green[300], marginRight: 16 }}
                        >
                            <EuroSymbol />
                        </Avatar>
                    </Grid>
                    <Grid item xs={6}>
                        <Typography type="title">
                            {fromTab.name} -> {toTab.name}
                        </Typography>
                        <Typography type="body2">
                            {dateToString(operation.date)}
                        </Typography>
                        <Typography type="body1">
                            {operation.comment}
                        </Typography>
                    </Grid>
                    <Grid item xs={2}>
                        <Typography type="subheading">
                            {operation.amount} €
                    </Typography>
                    </Grid>
                    <Grid item xs={3}>
                        <IconButton
                            aria-label="Edit"
                            onClick={this.onEditClick}
                        >
                            <ModeEdit />
                        </IconButton>
                        <IconButton
                            aria-label="Remove"
                            onClick={this.onRemoveClick}
                        >
                            <DeleteForever />
                        </IconButton>
                    </Grid>
                </Grid>
                <ConfirmationDialog
                    open={this.state.openConfirmationDialog}
                    onClose={this.onRemoveClose}
                    onConfirm={this.onRemoveConfirm}
                    title={`Do you really want to remove this operation?`}
                    text={`After opeartion is deleted you can't restore it!`}
                />
            </Paper>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    tabs: state.tabs.items
});

export default connect(mapStateToProps)(Operation);