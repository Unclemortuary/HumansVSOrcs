using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ClonnableGameUnit : AbstractGameUnit {


    [SerializeField]
    private int id;
    public override int ID {
        get {
            return id;
        }
        set {
            id = value;
        }
    }

    [SerializeField]
    private string description;
    public override string Description {
        get {
            return description + "\nid=" + ID + "\ncurrent HP=" + CurrentHP;
        }
        set {
            description = value;
        }
    }


    [SerializeField]
    private float currentHP;
    public override float CurrentHP {
        get {
            return currentHP;
        }
    }


    [SerializeField]
    private float currentMP;
    public override float CurrentMP {
        get {
            return currentMP;
        }
    }



    [SerializeField]
    private GameUnitCharacteristics characteristics;
    public override GameUnitCharacteristics Characteristics {
        get {
            return characteristics;
        }
    }


    [SerializeField]
    private GameObject avatar;
	public override UnityEngine.GameObject Avatar {
		get {
			return avatar;
		}
	}


    [SerializeField]
    private List<RTSActionType> actionsList;
    public override List<RTSActionType> ActionsList {
        get {
            return actionsList;
        }
    }


///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private ClonnableGameUnit(int id, string descr, GameUnitCharacteristics characteristics, GameObject avatar, List<RTSActionType> list) {

        this.id = id;
        this.description = descr;
        this.characteristics = characteristics;

        this.avatar = avatar;

        this.currentHP = this.characteristics.MaxHP;
        this.currentMP = this.characteristics.MaxMP;

        this.actionsList = list;
    }


    /**
    * All clones refer to the same GameUnitCharacteristics instance
    * Each clone has its own avatar instance
     */
    public ClonnableGameUnit CreateClone(Vector3 position, int id = 0, string descr = "") {

        GameObject newAvatar = GameObject.Instantiate(this.Avatar, position, this.Avatar.transform.rotation);

        return new ClonnableGameUnit(id, descr, characteristics, newAvatar, actionsList);

    }

    /**
    * Creates copy of prototype with the same avatar GameObject,
    * but with different instance of GameUnitCharacteristics
     */
    public ClonnableGameUnit CreateCopy() {

        GameUnitCharacteristics characteristicsCopy = characteristics.CreateCopy();

        List<RTSActionType> newActionsList = new List<RTSActionType>();

        foreach(RTSActionType type in this.actionsList) {
            newActionsList.Add(type);
        }

        return new ClonnableGameUnit(id, description, characteristicsCopy, avatar, newActionsList);
    }


}
