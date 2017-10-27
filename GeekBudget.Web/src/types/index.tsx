import Tab from '../models/Tab';
import Operation from '../models/Operation';
import { RouterState } from 'react-router-redux'

export interface StoreState{
    tabs: TabState;
    operations: OperationState;
    applicationError: ApplicationErrorState;
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

export interface ApplicationErrorState{
    errorText: string;
    error: ApplicationError;
}

export interface ApplicationError{
    code: number;
    text: string;
    object: ValidationError | string;
}

export interface ValidationError{
    [key: string] : string[]; //{ errorKey: string, errorValues: string[] }
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
    applicationError: {
        error: {
            code: 0,
            object: {},
            text: ''
        },
        errorText: ''
    },
    router: {
        location: null
    }
}