export interface StoreState{
	testBool: boolean;
	testString: string;
}

export const initialState: StoreState = {
	testBool: false,
	testString: ""
}