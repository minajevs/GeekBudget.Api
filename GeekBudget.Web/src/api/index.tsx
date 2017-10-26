import Tab from '../models/Tab';

import * as tabApi from './tab';
import * as operationApi from './operation';

export default class Api{
    static tab = tabApi;
    static operation = operationApi;
}