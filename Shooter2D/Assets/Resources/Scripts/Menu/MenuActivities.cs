using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuActivities : MonoBehaviour {

    public List<Button> buttonList;
    MyGameEvents gameEvents;

    // Use this for initialization
    void Start () {
        gameEvents = new MyGameEvents();
        for (int i = 0; i < buttonList.Count; i++){
            var newi = i;
            UnityAction<int> action = new UnityAction<int>(ButtonAddPower);
            buttonList[i].onClick.AddListener(delegate { action.Invoke(newi); });
        }
	}
	
    void changeLevel(int power, string itemName, float powerValue){
        int pv = (int)powerValue;
        Color color = gameEvents.WeaponColor(pv);
        buttonList[power].transform.parent.Find("Button").Find("Text").GetComponent<Text>().color = color;
        buttonList[power].transform.parent.Find("Title").GetComponent<Text>().color = color;
        buttonList[power].transform.parent.Find("Title").GetComponent<Text>().text = itemName + powerValue;
        if (powerValue >= 5) {
            buttonList[power].transform.parent.Find("Button").GetComponent<Button>().interactable = false;
            buttonList[power].transform.parent.Find("Button").Find("Text").GetComponent<Text>().text = "Max";
            buttonList[power].transform.parent.Find("Button").Find("Text").GetComponent<Text>().color = gameEvents.WeaponColor(99);
        }
    }

    void ButtonAddPower(int power)
    {
        switch(power){
            case 0:
                PlayerStats.HealBox++;
                changeLevel(power, "Cura Lv.",PlayerStats.HealBox);
                break;
            case 1:
                PlayerStats.Shooter++;
                changeLevel(power, "Cañon Lv.",PlayerStats.Shooter);
                break;
            case 2:
                PlayerStats.Helper++;
                changeLevel(power, "Soporte Lv.",PlayerStats.Helper);
                break;
            case 3:
                PlayerStats.Misile_Power++;
                changeLevel(power, "Misil Lv.",PlayerStats.Misile_Power);
                break;
            default:
                Debug.Log("NO DEFINIDO");
                break;
        }
    }
}
