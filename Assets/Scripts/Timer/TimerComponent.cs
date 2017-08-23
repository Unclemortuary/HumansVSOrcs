using System;
using UnityEngine;

public class TimerComponent : MonoBehaviour {


    private Action act;
    private float timeSpan;
    private float timeVar;
    private int repetitionsNumber;

    public Action ActionToDo {
        get { return act; }
        set { act = value; }
    }

    public int RepetitionsNumber
    {
        get
        {
            return repetitionsNumber;
        }
        set
        {
            repetitionsNumber = value;
        }
    }




    public float TimeToWait
    {
        get
        {
            return timeSpan;
        }

        set
        {
            timeVar = value;
            timeSpan = value;
        }
    }


    void Update() {

        if (repetitionsNumber == 0)
        {
            Destroy(this);
        }
        else
        {
            timeVar -= Time.deltaTime;

            if (timeVar <= 0)
            {
                act();
                repetitionsNumber--;
                timeVar = timeSpan;
            }

        }
    }


}