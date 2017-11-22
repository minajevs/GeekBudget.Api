import Tab from 'models/Tab';

import { createApiUrl, createRequestOptions } from 'helpers/api';

export const getAll = async (): Promise<Tab[]> => {
    const url = createApiUrl('tab', 'getall');
    const options = createRequestOptions('GET');
    try{
        const response = await fetch(url, options);

        if (response.status !== 200)
            throw await response.text(); // TODO
        
        return await response.json() as Tab[];
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};

export const add = async (tab: Tab): Promise<number> => {
    const url = createApiUrl('tab', 'add');
    const options = createRequestOptions('POST', tab);
    try{
        const response = await fetch(url, options);

        if (response.status !== 200)
            throw await response.text(); // TODO
    
        return await response.json() as number;
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};

export const remove = async (id: number): Promise<boolean> => {
    const url = createApiUrl('tab', 'remove', id.toString());
    const options = createRequestOptions('POST');
    try{
        const response = await fetch(url, options);
        if (response.status !== 200)
            throw await response.text(); // TODO

        return await true;
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};

export const update = async (tab: Tab): Promise<boolean> => {
    const url = createApiUrl('tab', 'update');
    const options = createRequestOptions('POST', tab);
    try{
        const response = await fetch(url, options);
        if (response.status !== 200)
            throw await response.text(); // TODO

        return await true;
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};    