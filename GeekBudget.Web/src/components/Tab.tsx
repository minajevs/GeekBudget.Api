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
import TabModel from '../models/Tab';

interface Props {
    tab: TabModel;
    onRemoveClick: () => void;
    onEditClick: () => void;
}

interface State {
    menuOpen: boolean;
    menuEl: HTMLElement | undefined;
    innerTab: TabModel;
}

const styles = {
    tab: {
        cursor: 'pointer'
    }
}

export default class Tab extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            menuOpen: false,
            menuEl: undefined,
            innerTab: props.tab
        }
    }

    handleChange(property: string, event: any) {
        let updateTab = { ...this.state.innerTab };
        updateTab[property] = event.target.value;
        this.setState({
            innerTab: updateTab
        });
    }

    render() {
        const { onRemoveClick, onEditClick } = this.props;
        const { innerTab: tab } = this.state;
        return (
            <Card>
                <CardHeader
                    avatar={<Avatar aria-label='TabIcon'><AccountBalanceWallet /></Avatar>}
                    title={tab.name}
                    subheader={`${tab.amount} €`}
                />
                <CardActions>
                    <IconButton
                        aria-label="Edit"
                        onClick={onEditClick}
                    >
                        <ModeEdit />
                    </IconButton>
                    <div style={{ flex: '1 1 auto' }} />
                    <IconButton
                        aria-label="Remove"
                        onClick={onRemoveClick}
                    >
                        <DeleteForever />
                    </IconButton>
                </CardActions>
            </Card>
        );
    }
}