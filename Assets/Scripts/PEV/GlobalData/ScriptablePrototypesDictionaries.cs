using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ScriptablePrototypesDictionaries : ScriptableObject {



    [SerializeField]
    private TypeToGameUnitDictionary humanWarriorsPrototypes;
    public TypeToGameUnitDictionary HumanWarriorsPrototypes {
        get {
            return humanWarriorsPrototypes;
        }
    }


    [SerializeField]
    private TypeToGameUnitDictionary orcWarriorsPrototypes;
    public TypeToGameUnitDictionary OrcWarriorsPrototypes {
        get {
            return orcWarriorsPrototypes;
        }
    }




    [SerializeField]
    private TypeToGameUnitDictionary humanBuildingsPrototypes;
    public TypeToGameUnitDictionary HumanBuildingsPrototypes {
        get {
            return humanBuildingsPrototypes;
        }
    }

    [SerializeField]
    private TypeToGameUnitDictionary orcBuildingsPrototypes;
    public TypeToGameUnitDictionary OrcBuildingsPrototypes {
        get {
            return orcBuildingsPrototypes;
        }
    }


}