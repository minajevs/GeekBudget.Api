import * as React from 'react';

import Dialog, {
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
} from 'material-ui/Dialog';

import Button from 'material-ui/Button';

interface Props{
    open:boolean;
    title:string;
    text:string;
    onConfirm?: ()=>void;
    onDecline?: ()=>void;
    onClose: ()=>void;
}

export default class ConfirmationDialog extends React.Component<Props> {
    constructor(props: Props) {
        super(props);
    }

    render() {
        const {onClose, onConfirm, onDecline, title, text, open} = this.props;
        const onDeclineInternal = () => {
            onClose();
            if(onDecline != null) onDecline();
        }

        const onConfirmInternal = () => {
            onClose();
            if(onConfirm != null) onConfirm();
        }

        return (
            <Dialog open={open} onRequestClose={onClose}>
                <DialogTitle>{title}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        {text}
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onDeclineInternal} color="primary" autoFocus>
                        No
                    </Button>
                    <Button onClick={onConfirmInternal} color="accent">
                        Yes
                </Button>
                </DialogActions>
            </Dialog>
        )
    }
}