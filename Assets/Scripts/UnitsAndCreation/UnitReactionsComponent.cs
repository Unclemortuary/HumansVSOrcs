public class UnitReactionsComponent : RTSMonoBehaviour {

    private AbstractGameUnit thisUnit;

    public void SetGameUnit(AbstractGameUnit unit) {
        thisUnit = unit;
    }

    void Start() {
        // Подписка на события здесь
    }

    public void IAmClicked() {
        // Отправка сообщения
        // unit -- в качестве аргумента
    }


}