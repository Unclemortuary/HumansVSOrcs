using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TypeToGameUnitDictionary : Dictionary<Identification.UnitType, ClonnableGameUnit>, ISerializationCallbackReceiver {
//public class TypeToGameUnitDictionary : Dictionary<Identification.UnitType, ClonnableGameUnit> {

    [System.Serializable]
    public class Pair {

        public Identification.UnitType unitType;
        public ClonnableGameUnit unitPrototype;
        public Pair(Identification.UnitType tp, ClonnableGameUnit unit) {
            this.unitType = tp;
            this.unitPrototype = unit;
        }
    }

    [SerializeField]
    private List<Pair> keyValueList = new List<Pair>();

    public void OnBeforeSerialize()
    {
        keyValueList.Clear();

        foreach (KeyValuePair<Identification.UnitType, ClonnableGameUnit> pair in this)
        {
            keyValueList.Add(new Pair(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < keyValueList.Count; i++) {
            if (!this.ContainsKey(keyValueList[i].unitType)) {
                this.Add(keyValueList[i].unitType, keyValueList[i].unitPrototype);
            }
        }
    }

}
