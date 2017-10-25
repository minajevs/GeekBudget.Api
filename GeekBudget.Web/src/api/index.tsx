import Tab from '../models/Tab';

export default class Api{
    private static readonly _url:string = "https://geekbudgetapitest.azurewebsites.net/api";

    //private static readonly _url:string = "http://localhost:47570/api";
    private static readonly _tabUrl:string = "/tab";

    private static _accessKey:string = "cmMUou9gBdkAzqgOccS+OdoRscmKwjc/tTV+PDLaEWY=";
    //private static _accessKey:string = "admin";
    
    public static async getAllTabs():Promise<Tab[]>{
        const url = this._url + this._tabUrl + '/getall';
        const options = this.createRequestOptions("GET");
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
        const url = this._url + this._tabUrl + '/add';
        const options = this.createRequestOptions("POST", tab);
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
        const url = this._url + this._tabUrl + '/remove' + '/' + id;
        const options = this.createRequestOptions("POST");
        try{
            const response = await fetch(url, options);
            console.log("Fetch succeed.");
            return true;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }

    public static async updateTab(tab:Tab):Promise<boolean>{
        const url = this._url + this._tabUrl + '/update';
        const options = this.createRequestOptions("POST", tab);
        try{
            const response = await fetch(url, options);
            console.log("Fetch succeed.");
            return true;
        } catch (e){
            throw `Fetch failed! Reason: ${e}`
        }
    }    

    private static createRequestOptions(method:string, body:any = null):RequestInit{
        const headers = new Headers({
            "user-key": this._accessKey,
            "Content-Type": "application/json"
        });

        const init:RequestInit = {
            headers: headers,
            method: method,
            body: body == null ? null : JSON.stringify(body),
            mode: 'cors'
        };
        
        return init;
    }

}