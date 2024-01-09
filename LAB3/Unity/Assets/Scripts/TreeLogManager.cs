using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TreeLogManager : MonoBehaviour
{

    public TMP_Text logText;
    public int logCount = 0;

    public TMP_Text buildInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        logText.text = ": " + logCount.ToString();
    }

    public void showBuildInfo(bool isVisible, string objectName, int cost)
    {
        buildInfo.text = "press Y to build " + objectName + " (" + cost.ToString() + " logs)";
        buildInfo.gameObject.SetActive(isVisible);
    }
}
