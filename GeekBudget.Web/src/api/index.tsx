import Tab from '../models/Tab';

export default class Api{
    private static readonly _url:string = "https://geekbudgetapitest.azurewebsites.net/api";

    //private static readonly _url:string = "http://localhost:47570/api";
    private static readonly _getAll:string = "/getall";
    private static readonly _tabUrl:string = "/tab";

    private static _accessKey:string = "cmMUou9gBdkAzqgOccS+OdoRscmKwjc/tTV+PDLaEWY=";
    
    public static async getAllTabs(){
        const url = this._url + this._tabUrl + this._getAll;
        const options = this.createRequestOptions("GET");
        console.log("Fetch started.");
        console.log(options);
        try{
            const response = await fetch(url, options);
            const data = await response.json();
            console.log("Fetch succeed.");
            console.log(data);
        } catch (e){
            console.log("Fetch failed.");
            console.log(e);
        }

    }

    private static createRequestOptions(method:string, body = null):RequestInit{
        const headers = new Headers({
            "user-key": this._accessKey
        });

        const init:RequestInit = {
            headers: headers,
            method: method,
            body: body,
            mode: 'cors'
        };

        return init;
    }

}