import Tab from 'models/Tab';

import * as tabApi from './tab';
import * as operationApi from './operation';
import { apiHelpers } from 'helpers';

module Api{
    export const tab = tabApi;
    export const operation = operationApi;
    export const helpers = apiHelpers;
}

export default Api;