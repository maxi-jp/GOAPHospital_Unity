using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    private Vector3 screenPosition;
    private GUIStyle guiStyle;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);

        guiStyle = new GUIStyle();
        guiStyle.normal.textColor = Color.black;
    }

    private void Update()
    {
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
    }
    
    private void OnGUI()
    {
        GUI.Label(new Rect(screenPosition.x - 40, Screen.height - screenPosition.y - 20, 100, 20), currentAction.actionName, guiStyle);
    }
    
}
