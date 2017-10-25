import Tab from '../models/Tab';
import { RouterState } from 'react-router-redux'

export interface StoreState{
    tabs: TabState;
    router: RouterState;
}

export interface TabState{
    isFetching: boolean;
    items: Tab[];
    // filter: TabFilter,
    // etc.

}

export const initialState : StoreState = {
    tabs: {
        isFetching: false,
        items: []  
    },
    router: {
        location: null
    }
}