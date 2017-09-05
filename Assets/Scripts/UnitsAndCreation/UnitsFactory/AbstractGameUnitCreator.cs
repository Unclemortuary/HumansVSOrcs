using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGameUnitCreator {

    public abstract AbstractGameUnit CreateGameUnit(Vector3 position);
    public abstract GameUnitCharacteristics GetGameUnitCharacteristics();
}
