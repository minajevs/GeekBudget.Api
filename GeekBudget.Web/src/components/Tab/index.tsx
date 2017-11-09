import * as React from 'react';

import Paper from 'material-ui/Paper';
import Card, { CardActions, CardContent, CardHeader } from 'material-ui/Card';
import Typography from 'material-ui/Typography';
import IconButton from 'material-ui/IconButton';
import ModeEdit from 'material-ui-icons/ModeEdit';
import DeleteForever from 'material-ui-icons/DeleteForever';
import MoreVertIcon from 'material-ui-icons/MoreVert';
import AccountBalanceWallet from 'material-ui-icons/AccountBalanceWallet';
import Menu, { MenuItem } from 'material-ui/Menu';
import Grid from 'material-ui/Grid';
import Avatar from 'material-ui/Avatar';
import TabModel from '../../models/Tab';
import { blue } from 'material-ui/colors';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

import ConfirmationDialog from '../../components/ConfirmationDialog';

interface Props {
    tab: TabModel;
    dispatch: Dispatch<{}>;
}

interface State {
    openConfirmationDialog: boolean;
    menuEl: HTMLElement | undefined;
}

const styles = {
    tab: {
        cursor: 'pointer'
    }
};

class Tab extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            openConfirmationDialog: false,
            menuEl: undefined
        };
    }

    onEditClick = () => {
        this.props.dispatch(tabActions.requestEditTab(this.props.tab));
    }

    onRemoveClick = () => {
        this.setState({ openConfirmationDialog: true });
    }

    onRemoveConfirm = () => {
        this.props.dispatch(tabActions.removeTab(this.props.tab.id));
        this.onRemoveClose();
    }

    onRemoveClose = () => {
        this.setState({ openConfirmationDialog: false });
    }

    render() {
        const { tab } = this.props;
        return [(
            <Card key="card">
                <CardHeader
                    avatar={
                        <Avatar
                            aria-label="TabIcon"
                            style={{ color: blue[900], backgroundColor: blue[300] }}
                        >
                            <AccountBalanceWallet />
                        </Avatar>}
                    title={tab.name}
                    subheader={`${tab.amount} €`}
                />
                <CardActions>
                    <IconButton
                        aria-label="Edit"
                        onClick={this.onEditClick}
                    >
                        <ModeEdit />
                    </IconButton>
                    <div style={{ flex: '1 1 auto' }} />
                    <IconButton
                        aria-label="Remove"
                        onClick={this.onRemoveClick}
                    >
                        <DeleteForever />
                    </IconButton>
                </CardActions>
            </Card>
        ),
        (
            <ConfirmationDialog
                key="dialog"
                open={this.state.openConfirmationDialog}
                onClose={this.onRemoveClose}
                onConfirm={this.onRemoveConfirm}
                title={`Do you really want to remove this tab?`}
                text={`After tab is deleted you can't restore it!`}
            />
        )
        ];
    }
}

export default connect()(Tab);