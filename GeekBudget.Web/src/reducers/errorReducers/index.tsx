import * as constants from '../../constants/errorConstants';
import { ApplicationErrorState, ApplicationError, initialState } from '../../types';
import { handleActions, Action } from 'redux-actions';

import Operation from '../../models/Operation';

export const errorReducers = handleActions<ApplicationErrorState, ApplicationError>(
    {
        [constants.THROW_APPLICATION_ERROR]: (state: ApplicationErrorState, 
                                              action: Action<ApplicationError>): ApplicationErrorState => {
            alert((action.payload as ApplicationError).text);
            return {
                ...state,
                error: action.payload as ApplicationError,
                errorText: (action.payload as ApplicationError).text
            };
        },
    }, 
    initialState.applicationError);