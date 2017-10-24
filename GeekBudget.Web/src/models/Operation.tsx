import Tab from './Tab';

export default class Operation{
    id:number;
    comment:string;
    amount:number;
    currency:number;
    from:Tab;
    to:Tab;
    date:Date;
}