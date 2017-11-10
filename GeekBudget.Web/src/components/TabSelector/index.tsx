import * as React from 'react';
import * as Autosuggest from 'react-autosuggest';
let match = require('autosuggest-highlight/match');
let parse = require('autosuggest-highlight/parse');

import TextField from 'material-ui/TextField';
import Menu, { MenuItem } from 'material-ui/Menu';

import Operation from 'models/Operation';
import TabModel from 'models/Tab';

interface Props {
    onChange: (event: React.ChangeEvent<HTMLElement>) => void;
    tabs: TabModel[];
    label: string;
    defaultValue: number;
}

interface State {
    value: number;
}

export default class TabSelector extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            value: props.defaultValue
        };
    }

    handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        this.setState({
            value: parseInt(event.target.value),
        });
        this.props.onChange(event);
    }

    render() {
        const { tabs, label } = this.props;
        const { value } = this.state;
        return (
            <TextField
                autoFocus
                margin="dense"
                id="select-tab"
                select
                label={label}
                value={this.state.value}
                onChange={this.handleChange}
                fullWidth
            >
                {tabs.map((tab) => (
                    <MenuItem selected={value === tab.id} key={tab.id} value={tab.id}>
                        {tab.name}
                    </MenuItem>
                ))}
            </TextField>
        );
    }
}