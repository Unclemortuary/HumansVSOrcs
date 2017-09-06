using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ClonnableGameUnit : AbstractGameUnit {


    [SerializeField]
    private float radius;
    public override float Radius {
        get {
            return radius;
        }
    }

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
            return description;// + "\nid=" + ID + "\ncurrent HP=" + CurrentHP;
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

    public override void ChangeHP(float value) {
        currentHP += value;
        if (currentHP < 0) {
            currentHP = 0;
        }
        if (currentHP > characteristics.MaxHP) {
            currentHP = characteristics.MaxHP;
        }
    }

    public override void ChangeMP(float value) {
        currentMP += value;
        if (currentMP < 0) {
            currentMP = 0;
        }
        if (currentMP > characteristics.MaxMP) {
            currentMP = characteristics.MaxMP;
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




///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private ClonnableGameUnit(int id, string descr, GameUnitCharacteristics characteristics, GameObject avatar) {

        this.id = id;
        this.description = descr;
        this.characteristics = characteristics;

        this.avatar = avatar;
        if (this.avatar == null) {
            this.avatar = characteristics.AvatarPrefab;
        }

        this.currentHP = this.characteristics.MaxHP;
        this.currentMP = this.characteristics.MaxMP;

//        this.actionsList = list;

        this.radius = CalculateUnitRadius();
    }

    private float CalculateUnitRadius() {
        Vector3 extents = Avatar.GetComponent<Collider>().bounds.extents;
        float radius2 = extents.x * extents.x + extents.z * extents.z;

        NavMeshObstacle meshObstacle = Avatar.GetComponent<NavMeshObstacle>();
        NavMeshAgent meshAgent = Avatar.GetComponent<NavMeshAgent>();


        if (meshObstacle != null) {

            Vector3 obstacleSize = meshObstacle.size;

            float obstacleRadius2 = obstacleSize.x * obstacleSize.x * 0.25f +
            obstacleSize.z * obstacleSize.z * 0.25f;

            if (radius2 <  obstacleRadius2) {
                radius2 = obstacleRadius2;
            }
        }

        float radius = Mathf.Sqrt(radius2);

        if (meshAgent != null) {

            float agentRadius = meshAgent.radius;

            if (radius <  agentRadius) {
                radius = agentRadius;
            }
        }

        return radius;
    }

    /**
    * All clones refer to the same GameUnitCharacteristics instance
    * Each clone has its own avatar instance
     */
    public ClonnableGameUnit CreateClone(Vector3 position, int id = 0, string descr = "") {

        GameObject newAvatar = GameObject.Instantiate(this.Characteristics.AvatarPrefab, position, this.Characteristics.AvatarPrefab.transform.rotation);

        return new ClonnableGameUnit(id, descr, characteristics, newAvatar);

    }

    /**
    * Creates copy of prototype with the same avatar GameObject,
    * but with different instance of GameUnitCharacteristics
     */
    public ClonnableGameUnit CreateCopy() {

        GameUnitCharacteristics characteristicsCopy = characteristics.CreateCopy();


        return new ClonnableGameUnit(id, description, characteristicsCopy, avatar);
    }


    //////////  DEATH  //////////////////////////////////////////////

//    private static GameObject NoAvatar = null;
    private static GameObject NoAvatar = new GameObject();

    private static GameUnitCharacteristics NoCharacteristics =
        new GameUnitCharacteristics(null, null,0,0,0,0,0,0,0,0,0, NoActions);

    private static List<RTSActionType> NoActions = new List<RTSActionType>();


    public override void Nullify() {

        this.radius = 0;
        this.id = 0;
        this.description = "";
        this.characteristics = NoCharacteristics;

        this.avatar.SetActive(false);
        this.avatar = NoAvatar;
//            GameObject.Destroy(unit.Avatar);

        this.currentHP = 0;
        this.currentMP = 0;

//        this.actionsList = NoActions;

        this.IsActive = false;
    }

    public override bool IsDead() {
        if (Avatar == NoAvatar || CurrentHP <= 0) {
//            Debug.Log("##################################### Unit is dead ###########################");
            return true;
        }
        if (Avatar == null) {
//        if (Avatar == null || !Avatar.activeSelf) {
//            Debug.Log("##################################### Unit is dead ###########################");
            return true;
        }

        return false;
    }

}
