import * as React from 'react';
//import './TestBool.css';

export interface Props{
	testBool: boolean;
	onSet: () => void;
}

export default class TestBool extends React.Component<Props, object> {
  render() {
	return (
	  <div>
		  <p>Current bool value is: <strong>{this.props.testBool.toString()}</strong></p>
		  <button onClick={this.props.onSet}>Change</button>
	  </div>
	);
  }
}