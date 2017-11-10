import * as React from 'react';
import Button from 'material-ui/Button';
import AddIcon from 'material-ui-icons/Add';

import TabModel from 'models/Tab';
import TabEdit from 'components/TabEdit';

// Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from 'actions';
import { StoreState } from 'types/index';

// Router
import { withRouter } from 'react-router-dom';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

interface State {
    openAddDialog: boolean;
    innerTab: TabModel;
}

class TabAddButton extends React.Component<Props, State> {
    constructor(props: Props) {
        super(props);
        this.state = {
            openAddDialog: false,
            innerTab: new TabModel()
        };
    }

    render() {
        const { dispatch } = this.props;
        return (
            <div>
                <Button 
                    fab 
                    aria-label="add" 
                    onClick={() => dispatch(tabActions.requestEditTab(new TabModel()))}
                >
                    <AddIcon />
                </Button>
            </div>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(TabAddButton));