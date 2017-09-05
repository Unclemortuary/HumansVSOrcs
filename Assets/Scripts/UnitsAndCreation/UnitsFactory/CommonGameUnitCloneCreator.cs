using UnityEngine;

public class CommonGameUnitCloneCreator : AbstractGameUnitCreator {

    public override GameUnitCharacteristics GetGameUnitCharacteristics() {
        return this.prototype.Characteristics;
    }

    private ClonnableGameUnit prototype;

    public CommonGameUnitCloneCreator(ClonnableGameUnit proto) {
        this.prototype = proto;
    }

    public override AbstractGameUnit CreateGameUnit(Vector3 position) {

        return prototype.CreateClone(position);
    }
}