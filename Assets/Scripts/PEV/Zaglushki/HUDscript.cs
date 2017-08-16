using UnityEngine;
using UnityEngine.UI;

public static class HUDscript {


    private static Canvas miniMapPanel;
    private static Canvas commandsPanel;
    private static Canvas objectInfoPanel;
    private static Text objectInfoText;

    private static Image floatingPanel;
    private static Text floatingText;

    private static float standardScreenWidth = 1280;
    private static float standardScreenHeight = 800;

    static HUDscript() {

//        miniMapPanel = GameObject.Find("HUD Canvas/Minimap Panel").GetComponent<Canvas>();
//        commandsPanel = GameObject.Find("HUD Canvas/Commands Panel").GetComponent<Canvas>();
//        objectInfoPanel = GameObject.Find("HUD Canvas/ObjectInfo Panel").GetComponent<Canvas>();
        objectInfoText = GameObject.Find("HUD Canvas/ObjectInfo Panel/InfoText").GetComponent<Text>();

        floatingPanel = GameObject.Find("HUD Canvas/Floating Panel").GetComponent<Image>();
        floatingText = GameObject.Find("HUD Canvas/Floating Panel/FloatingText").GetComponent<Text>();
        HideFloatingPanel();

        RectTransform rt = GameObject.Find("HUD Canvas").GetComponent<RectTransform>();
        standardScreenHeight = rt.rect.height;
        standardScreenWidth = rt.rect.width;
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

        floatingPanel.transform.localPosition = new Vector3( (x / Screen.width -0.5f) * standardScreenWidth,
                                                                (y / Screen.height - 0.5f) * standardScreenHeight,
                                                                            floatingPanel.transform.localPosition.z);
//        Debug.Log("After:: x=" + x + ", y=" + y + ", FLpos=" + floatingPanel.transform.position + ", FLlocal=" + floatingPanel.transform.localPosition
//            + "scrWidth=" + Screen.width + ", scrHeight=" + Screen.height);
    }

}
