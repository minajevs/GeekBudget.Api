import * as React from 'react';

import Tab from '../models/Tab';

interface Props {
    tab: Tab;
    onRemove: () => void;
    onSave: (tab: Tab) => void;
}

interface State {
    edit: boolean;
    innerTab: Tab;
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
            innerTab: props.tab
        }
    }

    handleChange(property:string, event:any) {
        let updateTab = { ...this.state.innerTab };
        updateTab[property] = event.target.value;
        this.setState({ 
            innerTab: updateTab
         });
    }

    render() {
        const { onRemove } = this.props;
        const { innerTab: tab, edit } = this.state;
        return (
            <div 
                onDoubleClick={() => this.setState({edit: !edit})} 
                style={{ border: '1px solid', width: '300px', margin: '2px', display: 'inline-block', cursor: 'pointer' }}>
                <ul>
                    <li>id: {tab.id}</li>
                    <li>name: {textOrEdit(edit, (tab.name || '').toString(), this.handleChange.bind(this, 'name'))}</li>
                    <li>amount: {textOrEdit(edit, (tab.amount || '').toString(), this.handleChange.bind(this, 'amount'))}</li>
                    <li>currency: {textOrEdit(edit, (tab.currency || '').toString(), this.handleChange.bind(this, 'currency'))}</li>
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
        this.props.onSave(this.state.innerTab);
    }

}