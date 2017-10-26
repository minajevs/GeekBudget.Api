import Tab from '../models/Tab';
import Operation from '../models/Operation';
import { RouterState } from 'react-router-redux'

export interface StoreState{
    tabs: TabState;
    operations: OperationState;
    router: RouterState;
}

export interface TabState{
    isFetching: boolean;
    items: Tab[];
    // filter: TabFilter,
    // etc.
}

export interface OperationState{
    isFetching: boolean;
    items: Operation[];
    // filter: OperationFilter,
    // etc.
}

export const initialState : StoreState = {
    tabs: {
        isFetching: false,
        items: []  
    },
    operations: {
        isFetching: false,
        items: []  
    },
    router: {
        location: null
    }
}