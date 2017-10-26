import * as React from 'react';

import Operation from '../models/Operation';

interface Props {
    operation: Operation;
    onRemove: () => void;
    onSave: (operation: Operation) => void;
}

interface State {
    edit: boolean;
    innerOperation: Operation;
}

const textOrEdit = function (edit: boolean, value: string, onChange: any) {
    if (edit) {
        return <input type="text" value={value} onChange={onChange} />
    } else {
        return value;
    }
}

export default class TabView extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            edit: false,
            innerOperation: props.operation
        }
    }

    handleChange(property:string, event:any) { //TODO: change from any to correct type
        let updateOperation = { ...this.state.innerOperation };
        updateOperation[property] = event.target.value;
        this.setState({ 
            innerOperation: updateOperation
         });
    }

    render() {
        const { onRemove } = this.props;
        const { innerOperation: operation, edit } = this.state;
        return (
            <div 
                onDoubleClick={() => this.setState({edit: !edit})} 
                style={{ border: '1px solid', width: '300px', margin: '2px', display: 'inline-block', cursor: 'pointer' }}>
                <ul>
                    <li>id: {operation.id}</li>
                    <li>comment: {textOrEdit(edit, (operation.comment || '').toString(), this.handleChange.bind(this, 'comment'))}</li>
                    <li>amount: {textOrEdit(edit, (operation.amount || '').toString(), this.handleChange.bind(this, 'amount'))}</li>
                    <li>currency: {textOrEdit(edit, (operation.currency || '').toString(), this.handleChange.bind(this, 'currency'))}</li>
                    <li>date: {textOrEdit(edit, (operation.date || '').toString(), this.handleChange.bind(this, 'date'))}</li>
                    <li>from: {textOrEdit(edit, (operation.from || '').toString(), this.handleChange.bind(this, 'from'))}</li>
                    <li>to: {textOrEdit(edit, (operation.to || '').toString(), this.handleChange.bind(this, 'to'))}</li>
                </ul>
                {edit  
                    ? <button onClick={this.onSave.bind(this)}>save</button>
                    : null
                }
                
                <button onClick={onRemove}>remove</button>
            </div>
        );
    }

    onSave(){
        console.log('save');
        this.setState({edit: false});
        this.props.onSave(this.state.innerOperation);
    }

}