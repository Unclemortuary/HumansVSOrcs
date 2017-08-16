using UnityEngine;

public class CommonGameUnitCloneCreator : AbstractGameUnitCreator {

    private ClonnableGameUnit prototype;

    public CommonGameUnitCloneCreator(ClonnableGameUnit proto) {
        this.prototype = proto;
    }

    public override AbstractGameUnit CreateGameUnit(Vector3 position) {

        return prototype.CreateClone(position);
    }
}