import { StoreState } from 'types/index';

import * as apiHelpers from './api';
import * as testHelpers from './test/index';

export function dateToString(date: Date): string{
    const yyyy = date.getFullYear();
    const MM = padStart('00', date.getMonth() + 1, 2); // +1 because months go 0..11
    const dd = padStart('00', date.getDate(), 2);
    return [yyyy, MM, dd].join('-');
}

export function padStart(fillString: string, value: string|number, length: number): string{
    return (fillString + value).substr(-length);
}

export function insertItem<T>(array: T[], item: T, index: number = array.length) {
    return [
        ...array.slice(0, index),
        item,
        ...array.slice(index)
    ];
}

export function removeItem<T>(array: T[], index: number) {
    return [
        ...array.slice(0, index),
        ...array.slice(index + 1)
    ];
}

export { 
    apiHelpers,
    testHelpers
};