import * as React from 'react';

import Operation from '../models/Operation';
import Tab from '../models/Tab';

interface Props {
    onChange: (tab: Tab) => void;
    availableTabs:  Tab[];
}

interface State {
    innerOperation: Operation
}

const styles = {
    popup: {
        position: 'relative', 
        border: '1px solid', 
        width: '300px', 
        margin: ' 0 auto', 
        clear: 'left',
        display: 'inline-block', 
        textAling: 'center',
        zIndex: '9'
    }
}

export default class TabSelector extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            innerOperation: new Operation()
        }
    }

    render() {
        const { availableTabs } = this.props;
        const { innerOperation } = this.state;
        return (
            <div 
                style={{  }}>

            </div>
        );
    }
}