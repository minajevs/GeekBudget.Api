import * as React from 'react';

import Dialog, {
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
} from 'material-ui/Dialog';

import Button from 'material-ui/Button';
import TextField from 'material-ui/TextField';

import TabModel from '../models/Tab';

interface Props {
    open: boolean;
    title: string;
    text: string;
    tab: TabModel;
    onSave: (tab:TabModel) => void;
    onClose: () => void;
}

interface State {
    innerTab: TabModel;
}

export default class ConfirmationDialog extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            innerTab: props.tab
        }
    }

    componentWillReceiveProps(props:Props){
        this.setState({innerTab: props.tab});
    }

    handleChange = (name:string) => (event:any) => {
        let updateTab = { ...this.state.innerTab };
        updateTab[name] = event.target.value;
        this.setState({ 
            innerTab: updateTab
         });
      };

    render() {
        const { onClose, onSave, title, text, open } = this.props;
        const innerSave = () => {
            onSave(this.state.innerTab);
            onClose();
        }
        return (
            <Dialog open={open} onRequestClose={onClose}>
                <DialogTitle>{title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {text}
                    </DialogContentText>
                    <TextField
                        autoFocus
                        margin="dense"
                        id="name"
                        label="Tab Name"
                        onChange={this.handleChange('name')}
                        defaultValue={this.state.innerTab.name}
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClose} color="primary">
                        Cancel
                    </Button>
                    <Button onClick={innerSave} color="primary">
                        Save
                    </Button>
                </DialogActions>
            </Dialog>
        )
    }
}