using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;

    public GameObject[] status;
    Text[] swordmanText, pristText, witchText;

    public Dictionary<string , GameObject> D_Player = new Dictionary<string , GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        D_Player.Add("검사", player1);
        D_Player.Add("사제", player2);
        D_Player.Add("마법사", player3);

        status = GameObject.FindGameObjectsWithTag("Stats");

        swordmanText = status[0].GetComponentsInChildren<Text>();
        pristText = status[1].GetComponentsInChildren<Text>();
        witchText = status[2].GetComponentsInChildren<Text>();
    }

    private void Update()
    {
        StatusShow();
    }

    private void StatusShow()
    {
        if (D_Player.ContainsKey("검사"))
        {
            Player P = D_Player["검사"].GetComponent<Player>();
            if (P != null)
            {
                swordmanText[0].text = P.pdata.job;
                swordmanText[1].text = "Level: " + P.pdata.level;
                swordmanText[2].text = "Exp: " + P.pdata.exp;
                swordmanText[3].text = "HP: " + P.pdata.hp + "/" + P.pdata.maxhp;
                swordmanText[4].text = "MP: " + P.pdata.mp + "/" + P.pdata.maxmp;
            }
        }
    }
}
