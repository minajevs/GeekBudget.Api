//Related
import * as React from 'react';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { operationActions } from '../../actions';
import { StoreState } from '../../types/index';

//Router
import { Route, Link, withRouter } from 'react-router-dom';
import { push } from 'react-router-redux'

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
                <button>
                    <Link to={'/operations/add'}>Add new</Link>
                </button>

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
                <Route path={'/operations/add'} render={() => 
                    (<Operation 
                        edit={true}
                        onRemove={() => dispatch(push('/operations'))}
                        operation={new OperationModel()}
                        onSave={(operation:OperationModel) => dispatch(operationActions.addOperation(operation))} />)} 
                />
            </div>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(Operations));