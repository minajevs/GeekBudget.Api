import Tab from 'models/Tab';

import * as tabApi from './tab';
import * as operationApi from './operation';
import { apiHelpers } from 'helpers';

export default class Api{
    static tab = tabApi;
    static operation = operationApi;
    static helpers = apiHelpers;
}