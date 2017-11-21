import * as React from 'react';
import * as ReactDOM from 'react-dom';
import * as enzyme from 'enzyme';
import Operation from '.';

import { shallowWithStore, getMaterialUIValue, mountWithStore } from 'helpers/testHelpers';
import OperationModel from 'models/Operation';
import TabModel from 'models/Tab';
import { StoreState, initialState } from 'types/index';

const operationModel: OperationModel = {
    comment: 'test-comment',
    amount: 11.33,
    currency: 'EUR',
    date: new Date('11/07/2017'),  // 7th Nov
    id: 1,
    from: 2,
    to: 3
};

const tabsArr: TabModel[] = [
    { id: 2, name: 'test-from', amount: 100, currency: 'EUR' },
    { id: 3, name: 'test-to', amount: 100, currency: 'EUR' },
];

const state: StoreState = { ...initialState, tabs: { ...initialState.tabs, items: tabsArr } };                         

describe('Operation', () => {
    it('renders all parameters', () => {
        const operation = shallowWithStore(<Operation operation={operationModel} />, state);
        expect(operation.html())
            .not.toBeNull();
        expect(getMaterialUIValue(operation.dive().find('.comment').at(0)))
            .toBe(operationModel.comment);
        expect(getMaterialUIValue(operation.dive().find('.amount').at(0)))
            .toBe(operationModel.amount.toString() + ' €');
        expect(getMaterialUIValue(operation.dive().find('.date').at(0)))
            .toBe('2017-11-07');
        expect(getMaterialUIValue(operation.dive().find('.name').at(0)))
            .toContain('test-from');
        expect(getMaterialUIValue(operation.dive().find('.name').at(0)))
            .toContain('test-to');
    });
});