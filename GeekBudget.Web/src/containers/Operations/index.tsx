//Related
import * as React from 'react';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { operationActions } from '../../actions';
import { StoreState } from '../../types/index';

//Router
import { Route, Link, withRouter } from 'react-router-dom';

//Components
import Operation from '../../components/Operation';
import OperationModel from '../../models/Operation';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

class Operations extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;

        return (
            <div>
                <button onClick={() => dispatch(operationActions.addOperation(new OperationModel()))}>Add new</button>
                <button onClick={() => dispatch(operationActions.getAllOperations())}>Reload</button>
                <p>
                    Fetching: <strong>{store.operations.isFetching.toString()}</strong>
                    Operations count: <strong>{store.operations.items.length}</strong>
                </p>
                {store.operations.items.map(o => {
                    return <Operation
                        operation={o}
                        key={o.id}
                        onRemove={() => dispatch(operationActions.removeOperation(o.id))}
                        onSave={(operation) => dispatch(operationActions.updateOperation(operation))}
                    />
                })}
            </div>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(Operations));