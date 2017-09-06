using UnityEngine;

[System.Serializable]
public class GameResources {

    private const float placesPerLivingHouse = 5f;
    private const float placesPerGeneralHouse = 10f;



    public enum ResourceType {
        GOLD, WOOD, FOOD, STONE, MEN, LIVING_HOUSES, GENERAL_HOUSES,
    }

    [SerializeField]
    private float gold;
    [SerializeField]
    private float wood;
    [SerializeField]
    private float food;
    [SerializeField]
    private float stone;

    [SerializeField]
    private float men;
    [SerializeField]
    private float livingHouses;
    [SerializeField]
    private float generalHouses;

    public static GameResources ZERO  = new GameResources();


    public GameResources(float gold, float wood, float food, float stone) {
        this.gold = gold;
        this.wood = wood;
        this.food = food;
        this.stone = stone;
    }

    public GameResources() {
        ZeroValues();
    }

    private void ZeroValues() {
        gold = 0;
        wood = 0;
        food = 0;
        stone = 0;
        men = 0;
        livingHouses = 0;
        generalHouses = 0;
    }


    public GameResources CreateCopy() {
        GameResources resources = new GameResources();

        resources.gold = this.gold;
        resources.wood = this.wood;
        resources.food = this.food;
        resources.stone = this.stone;
        resources.men = this.men;
        resources.livingHouses = this.livingHouses;
        resources.generalHouses = this.generalHouses;

        return  resources;
    }


    public float GetLivingPlacesNumber() {
        return livingHouses * placesPerLivingHouse + generalHouses * placesPerGeneralHouse;
    }

    public bool HaveFreeLivingPlaces() {
        return GetLivingPlacesNumber() > men;
    }


    public void ChangeResourceAmount(ResourceType type, float shift) {
        switch(type) {
            case ResourceType.GOLD:
                gold += shift;
                break;
            case ResourceType.WOOD:
                wood += shift;
                break;
            case ResourceType.FOOD:
                food += shift;
                break;
            case ResourceType.STONE:
                stone += shift;
                break;
            case ResourceType.MEN:
                men += shift;
                break;
            case ResourceType.LIVING_HOUSES:
                livingHouses += shift;
                break;
            case ResourceType.GENERAL_HOUSES:
                generalHouses += shift;
                break;
        }

        if (gold < 0) gold = 0;
        if (wood < 0) wood = 0;
        if (food < 0) food = 0;
        if (stone < 0) stone = 0;
        if (men < 0) men = 0;
        if (livingHouses < 0) livingHouses = 0;
        if (generalHouses < 0) generalHouses = 0;
    } // ChangeResourceAmount //

    public float GetResourceAmount(ResourceType type) {
        switch(type) {
            case ResourceType.GOLD:
                return gold;
//                break;
            case ResourceType.WOOD:
                return wood;
//                break;
            case ResourceType.FOOD:
                return food;
//                break;
            case ResourceType.STONE:
                return  stone;
//                break;
            case ResourceType.MEN:
                return men;
//                break;
            case ResourceType.LIVING_HOUSES:
                return livingHouses;
//                break;
            case ResourceType.GENERAL_HOUSES:
                return generalHouses;
        }
        return -1;
    }


    public bool HaveEnoughResources(GameResources price) {
        return ( ( (this.gold >= price.gold) && (this.wood >= price.wood) )
            && ( (this.food >= price.food) && (this.stone >= price.stone) ) )
            && ( (this.men >= price.men && this.livingHouses >= price.livingHouses)
            && (this.generalHouses >= price.generalHouses) );
    }

    public void SpendResources(GameResources price) {
        this.gold -= price.gold;
        this.wood -= price.wood;
        this.stone -= price.stone;
        this.food -= price.food;

//        this.men -= price.men;
//        this.livingHouses -= price.livingHouses;
    }


    public void AddResources(GameResources profit) {
        this.gold += profit.gold;
        this.wood += profit.wood;
        this.stone += profit.stone;
        this.food += profit.wood;
    }

}