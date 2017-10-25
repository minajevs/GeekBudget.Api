//Related
import * as React from 'react';

//Redux
import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { tabActions } from '../../actions';
import { StoreState } from '../../types/index';

//Router
import { Route, Link, withRouter } from 'react-router-dom';

//Components
import Tab from '../../components/Tab';
import TabModel from '../../models/Tab';

interface Props {
    dispatch: Dispatch<{}>;
    store: StoreState;
}

class Tabs extends React.Component<Props> {
    render() {
        const { dispatch, store } = this.props;

        return (
            <div>
                <button onClick={() => dispatch(tabActions.addTab(new TabModel()))}>Add new</button>
                <button onClick={() => dispatch(tabActions.getAllTabs())}>Reload</button>
                <p>
                    Fetching: <strong>{store.tabs.isFetching.toString()}</strong>
                    Tabs count: <strong>{store.tabs.items.length}</strong>
                </p>
                {store.tabs.items.map(t => {
                    return <Tab
                        tab={t}
                        key={t.id}
                        onRemove={() => dispatch(tabActions.removeTab(t.id))}
                        onSave={(tab) => dispatch(tabActions.updateTab(tab))}
                    />
                })}
            </div>
        );
    }
}

const mapStateToProps = (state: StoreState) => ({
    store: state
});

export default withRouter(connect(mapStateToProps)(Tabs));