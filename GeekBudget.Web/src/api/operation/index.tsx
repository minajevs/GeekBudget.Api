import Operation from 'models/Operation';

import { createApiUrl, createRequestOptions } from 'helpers/api';

export const getAll = async (): Promise<Operation[]> => {
    const url = createApiUrl('operation', 'getall');
    const options = createRequestOptions('GET');
    try{
        const response = await fetch(url, options);

        if (response.status !== 200)
            throw await response.text(); // TODO
        
        return await response.json() as Operation[];
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};

export const add = async (operation: Operation): Promise<number> => {
    const url = createApiUrl('operation', 'add');
    const options = createRequestOptions('POST', operation);
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
    const url = createApiUrl('operation', 'remove', id.toString());
    const options = createRequestOptions('POST');
    try{
        const response = await fetch(url, options);

        if (response.status !== 200)
            throw await response.text(); // TODO
    
        return true;
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};

export const update = async (operation: Operation): Promise<boolean> => {
    const url = createApiUrl('operation', 'update');
    const options = createRequestOptions('POST', operation);
    try{
        const response = await fetch(url, options);

        if (response.status !== 200)
            throw await response.text(); // TODO
            
        return true;
    } catch (e){
        throw `Fetch failed! Reason: ${e}`;
    }
};    