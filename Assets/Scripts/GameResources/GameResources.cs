using UnityEngine;

[System.Serializable]
public class GameResources {

    private const float placesPerHouse = 5f;



    public enum ResourceType {
        GOLD, WOOD, FOOD, STONE, MEN, LIVING_HOUSES,
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

    public static GameResources ZERO  = new GameResources();


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
    }


    public GameResources CreateCopy() {
        GameResources resources = new GameResources();

        resources.gold = this.gold;
        resources.wood = this.wood;
        resources.food = this.food;
        resources.stone = this.stone;
        resources.men = this.men;
        resources.livingHouses = this.livingHouses;

        return  resources;
    }


    public float GetLivingPlacesNumber() {
        return livingHouses * placesPerHouse;
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
        }

        if (gold < 0) gold = 0;
        if (wood < 0) wood = 0;
        if (food < 0) food = 0;
        if (stone < 0) stone = 0;
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
        }
        return -1;
    }


    public bool HaveEnoughResources(GameResources price) {
        return ( ( (this.gold >= price.gold) && (this.wood >= price.wood) )
        && ( (this.food >= price.food) && (this.stone >= price.stone) ) )
        && (this.men >= price.men && this.livingHouses >= price.livingHouses);
    }

    public void SpendResources(GameResources price) {
        this.gold -= price.gold;
        this.wood -= price.wood;
        this.stone -= price.stone;
        this.food -= price.food;

//        this.men -= price.men;
//        this.livingHouses -= price.livingHouses;
    }



}