using UnityEngine;

public interface IEnemyHelper {

    Identification.Army MyArmy { get; }

    bool IsAlife();

    Vector3 GetPosition();

    float GetRadius();

    float DamageHim(float damageValue);

}