import Tab from '../models/Tab';

export interface StoreState{
    testBool: boolean;
    tabs: TabState;
}

export interface TabState{
    isFetching: boolean;
    items: Tab[];
    // filter: TabFilter,
    // etc.

}

export const initialState : StoreState = {
    testBool: false,
    tabs: {
        isFetching: false,
        items: []  
    }
}