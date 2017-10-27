import * as constants from '../../constants/errorConstants';
import { ApplicationError, ApplicationErrorState } from '../../types/index';
import { createAction } from 'redux-actions';

export const throwApplicationError = createAction<ApplicationError>(constants.THROW_APPLICATION_ERROR);