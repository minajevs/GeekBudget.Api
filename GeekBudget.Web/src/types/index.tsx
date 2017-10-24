import Tab from '../models/Tab';

export interface StoreState{
    tabs: TabState;
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
    }
}