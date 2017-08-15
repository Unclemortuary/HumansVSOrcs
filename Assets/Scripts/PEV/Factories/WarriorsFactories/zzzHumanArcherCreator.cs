using UnityEngine;

public class zzzHumanArcherCreator : AbstractGameUnitCreator {

    private GameObject prefab;

    public override AbstractGameUnit CreateGameUnit(Vector3 position) {
        float maxAttackDistance = 5;
        float maxHP = 20;
        float maxMP = 5;
        float maxSpeed = 3;
        float defence = 1;
        GameObject avatar = Object.Instantiate(prefab);

        return null; //new ClonnableWarrior("Archer", maxAttackDistance, maxHP, maxMP, maxSpeed, defence, avatar);


    }
}