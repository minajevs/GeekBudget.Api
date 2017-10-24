import * as constants from '../../constants/tabConstants';
import { createAction } from 'redux-actions';

import Api from '../../api';

import Tab from '../../models/Tab';

export const requestAllTabs = createAction(constants.REQUEST_ALL_TABS);
export const receiveAllTabs = createAction<Tab[]>(constants.RECEIVE_ALL_TABS);
export function getAllTabs(){
    return async function (dispatch:any){
        dispatch(requestAllTabs());
        try{
            const tabs = await Api.getAllTabs();
            dispatch(receiveAllTabs(tabs));
        } catch(e){
            dispatch(requestResponseTab(e.toString()));
        }
    }
}

export const requestTab = createAction<number>(constants.REQUEST_TAB);
export const receiveTab = createAction<Tab>(constants.RECEIVE_TAB);

export const requestRemoveTab = createAction<number>(constants.REQUEST_REMOVE_TAB);
export function removeTab(id:number){
    return async function (dispatch:any){
        let responseString = "";
        dispatch(requestRemoveTab(id));
        try{
            await Api.removeTab(id);
            dispatch(getAllTabs());
            responseString = 'Success!';
        } catch(e){
            responseString = e;
        } finally{
            dispatch(requestResponseTab(responseString));
        }
    }
}

export const requestUpdateTab = createAction<Tab>(constants.REQUEST_UPDATE_TAB);
export function updateTab(tab:Tab){
    return async function (dispatch:any){
        let responseString = "";
        dispatch(requestUpdateTab(tab));
        try{
            await Api.updateTab(tab);
            dispatch(getAllTabs());
            responseString = 'Success!';
        } catch(e){
            responseString = e;
        } finally{
            dispatch(requestResponseTab(responseString));
        }
    }
}

export const requestResponseTab = createAction<string>(constants.RESPONSE_TAB);