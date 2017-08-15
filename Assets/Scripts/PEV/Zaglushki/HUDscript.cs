using UnityEngine;
using UnityEngine.UI;

public static class HUDscript {


    private static Canvas miniMapPanel;
    private static Canvas commandsPanel;
    private static Canvas objectInfoPanel;
    private static Text objectInfoText;

    private static Image floatingPanel;
    private static Text floatingText;

    static HUDscript() {

//        miniMapPanel = GameObject.Find("HUD Canvas/Minimap Panel").GetComponent<Canvas>();
//        commandsPanel = GameObject.Find("HUD Canvas/Commands Panel").GetComponent<Canvas>();
//        objectInfoPanel = GameObject.Find("HUD Canvas/ObjectInfo Panel").GetComponent<Canvas>();
        objectInfoText = GameObject.Find("HUD Canvas/ObjectInfo Panel/InfoText").GetComponent<Text>();

        floatingPanel = GameObject.Find("HUD Canvas/Floating Panel").GetComponent<Image>();
        floatingText = GameObject.Find("HUD Canvas/Floating Panel/FloatingText").GetComponent<Text>();
        HideFloatingPanel();
    }


    public static void Message(string str) {
        //Text txt = objectInfoPanel.gameObject.AddComponent<Text>();

        objectInfoText.text = str;
    }


    public static void HideFloatingPanel() {
        floatingPanel.enabled = false;
        floatingText.enabled = false;
    }

    public static void ShowFloatingPanelAt(float x, float y, string message) {
        floatingPanel.enabled = true;
        floatingText.enabled = true;

        floatingText.text = message;

//        Debug.Log("x=" + x + ", y=" + y + ", FLpos=" + floatingPanel.transform.position + ", FLlocal=" + floatingPanel.transform.localPosition);

        floatingPanel.transform.localPosition = new Vector3(x - Screen.width/2, y - Screen.height/2, floatingPanel.transform.localPosition.z);
//        Debug.Log("After:: x=" + x + ", y=" + y + ", FLpos=" + floatingPanel.transform.position + ", FLlocal=" + floatingPanel.transform.localPosition);
    }

}
