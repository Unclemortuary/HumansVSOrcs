using System.Collections.Generic;
using UnityEngine;

public class ScriptablePrototypesLists : ScriptableObject {

    [SerializeField]
    private List<ClonnableGameUnit> humanWarriorsPrototypes;
    public List<ClonnableGameUnit> HumanWarriorsPrototypes {
        get {
            return humanWarriorsPrototypes;
        }
    }

    [SerializeField]
    private List<ClonnableGameUnit> orcWarriorsPrototypes;
    public List<ClonnableGameUnit> OrcWarriorsPrototypes {
        get {
            return orcWarriorsPrototypes;
        }
    }




    [SerializeField]
    private List<ClonnableGameUnit> humanBuildingsPrototypes;
    public List<ClonnableGameUnit> HumanBuildingsPrototypes {
        get {
            return humanBuildingsPrototypes;
        }
    }

    [SerializeField]
    private List<ClonnableGameUnit> orcBuildingsPrototypes;
    public List<ClonnableGameUnit> OrcBuildingsPrototypes {
        get {
            return orcBuildingsPrototypes;
        }
    }






}