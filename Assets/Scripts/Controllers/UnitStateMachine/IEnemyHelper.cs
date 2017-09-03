using UnityEngine;

public interface IEnemyHelper {

    Identification.Army MyArmy { get; }

    bool IsAlife();

    Vector3 GetPosition();

    void DamageHim(float damageValue);
}