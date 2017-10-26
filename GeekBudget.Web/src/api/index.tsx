import Tab from '../models/Tab';

import { createApiUrl, createRequestOptions } from './helpers';

export default class Api{    
    public static async getAllTabs():Promise<Tab[]>{
        const url = createApiUrl('tab', 'getall')
        const options = createRequestOptions("GET");
        try{
            const response = await fetch(url, options);
            const data = await response.json() as Tab[];
            console.log("Fetch succeed.");
            console.log(data);
            return data;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }

    public static async addTab(tab:Tab):Promise<number>{
        const url = createApiUrl('tab', 'add')
        const options = createRequestOptions("POST", tab);
        try{
            const response = await fetch(url, options);
            const data = await response.json() as number;
            console.log("Fetch succeed.");
            console.log(data);
            return data;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }

    public static async removeTab(id:number):Promise<boolean>{
        const url = createApiUrl('tab', 'remove', id.toString())
        const options = createRequestOptions("POST");
        try{
            const response = await fetch(url, options);
            console.log("Fetch succeed.");
            return true;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }

    public static async updateTab(tab:Tab):Promise<boolean>{
        const url = createApiUrl('tab', 'update')
        const options = createRequestOptions("POST", tab);
        try{
            const response = await fetch(url, options);
            console.log("Fetch succeed.");
            return true;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }    
}