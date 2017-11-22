import Operation from 'models/Operation';
import Tab from 'models/Tab';

describe('Operation model', () => {
    it('should default id to -1', () => {
        const op = new Operation();
        expect(op.id).toEqual(-1);
    });
});

describe('Tab model', () => {
    it('should default id to -1', () => {
        const tab = new Tab();
        expect(tab.id).toEqual(-1);
    });
});