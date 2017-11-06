import Tab from '../models/Tab';
import Operation from '../models/Operation';
import { RouterState } from 'react-router-redux';

export interface StoreState{
    tabs: TabState;
    operations: OperationState;
    applicationError: ApplicationErrorState;
    router: RouterState;
}

export interface TabState{
    isFetching: boolean;
    items: Tab[];
    edit: TabEditState;
    // filter: TabFilter,
    // etc.
}

export interface OperationState{
    isFetching: boolean;
    items: Operation[];
    edit: OperationEditState;
    // filter: OperationFilter,
    // etc.
}

export interface TabEditState{
    open: boolean;
    isNew: boolean;
    tab: Tab | null;
}

export interface OperationEditState{
    open: boolean;
    isNew: boolean;
    operation: Operation | null;
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
    [key: string]: string[]; // { errorKey: string, errorValues: string[] }
}

export const initialState: StoreState = {
    tabs: {
        isFetching: false,
        items: [],
        edit: {
            open: false,
            isNew: true,
            tab: null
        }
    },
    operations: {
        isFetching: false,
        items: [],
        edit: {
            open: false,
            isNew: true,
            operation: null
        }
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
};