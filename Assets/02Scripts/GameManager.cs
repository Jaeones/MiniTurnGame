using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player1; // �˻� ������Ʈ ����
    public GameObject player2; // ���� ������Ʈ ����
    public GameObject player3; // ������ ������Ʈ ����

    public Dictionary<string, GameObject> D_Player = new Dictionary<string, GameObject>();

    private Player p1Script, p2Script, p3Script;

    public GameObject[] status;

    private Text[] swordmanText;
    private Text[] priestText;
    private Text[] witchText;

    public Image turn;
    public Text turnTxt;
    public float turnTime = 5f;

    CoolTime ct;

    public bool PlayerTurn = true;
    public bool MonsterTurn = false;
    public bool CurrTurn = false;

    //Monster
    public List<GameObject> L_Monster = new List<GameObject>();
    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Monster3;

    public GameObject win;
    public GameObject lose;

    private void Awake()
    {
        if (instance == null) instance = this;
        Time.timeScale = 1f;

        ct = new CoolTime();
    }

    private void Start()
    {
        p1Script = player1.GetComponent<Player>();
        p2Script = player2.GetComponent<Player>();
        p3Script = player3.GetComponent<Player>();

        if (player1 != null && !D_Player.ContainsKey(player1.name))
            D_Player.Add(player1.name, player1);

        if (player2 != null && !D_Player.ContainsKey(player2.name))
            D_Player.Add(player2.name, player2);

        if (player3 != null && !D_Player.ContainsKey(player3.name))
            D_Player.Add(player3.name, player3);


        if (status != null && status.Length >= 3)
        {
            swordmanText = status[0].GetComponentsInChildren<Text>();
            priestText = status[1].GetComponentsInChildren<Text>();
            witchText = status[2].GetComponentsInChildren<Text>();
        }

        L_Monster.Add(Monster1);
        L_Monster.Add(Monster2);
        L_Monster.Add(Monster3);
    }

    private void Update()
    {
        turn.fillAmount = ct.Timer(turnTime);
        if (D_Player.Count > 0) StatusShow();
        if (turn.fillAmount == 0)
        {
            PlayerTurn = !PlayerTurn;
            CurrTurn = !PlayerTurn;

            if (PlayerTurn)
            {
                turnTxt.text = "Player";
                MonsterTurn = false;
            }
            else
            {
                MonsterTurn=true;
                turnTxt.text = "Monster";
                StartCoroutine(MonsterAttack());
                //PlayerTurn = false;
            }
        }


        if (L_Monster.Count <= 0)
        {
            win.SetActive(true);
            Time.timeScale = 0f;
        }

        if (D_Player.Count <= 0)
        {
            lose.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }

    public void Win()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void Lose()
    {
        SceneManager.LoadScene("Ingame");
    }

    private void StatusShow()
    {
        // P1 (�˻�) ���� ����
        if (p1Script != null && swordmanText != null)
        {
            swordmanText[0].text = p1Script.pdata.job;
            swordmanText[1].text = "Level: " + p1Script.pdata.level;
            swordmanText[2].text = "Exp: " + p1Script.pdata.exp;
            swordmanText[3].text = "HP: " + p1Script.pdata.hp + "/" + p1Script.pdata.maxhp;
            swordmanText[4].text = "MP: " + p1Script.pdata.mp + "/" + p1Script.pdata.maxmp;
        }

        // P2 (����) ���� ���� - [���� ������]
        if (p2Script != null && priestText != null)
        {
            priestText[0].text = p2Script.pdata.job;     // swordmanText -> priestText�� ����
            priestText[1].text = "Level: " + p2Script.pdata.level;
            priestText[2].text = "Exp: " + p2Script.pdata.exp;
            priestText[3].text = "HP: " + p2Script.pdata.hp + "/" + p2Script.pdata.maxhp;
            priestText[4].text = "MP: " + p2Script.pdata.mp + "/" + p2Script.pdata.maxmp;
        }

        // P3 (������) ���� ���� - [���� ������]
        if (p3Script != null && witchText != null)
        {
            witchText[0].text = p3Script.pdata.job;      // swordmanText -> witchText�� ����
            witchText[1].text = "Level: " + p3Script.pdata.level;
            witchText[2].text = "Exp: " + p3Script.pdata.exp;
            witchText[3].text = "HP: " + p3Script.pdata.hp + "/" + p3Script.pdata.maxhp;
            witchText[4].text = "MP: " + p3Script.pdata.mp + "/" + p3Script.pdata.maxmp;
        }
    }

    IEnumerator MonsterAttack()
    {
        int i = 0;

        //���� ���͸� ������ ������ �ð� ����
        float delay = turnTime / L_Monster.Count;
        while (MonsterTurn)
        {
            if(L_Monster.Count != 0)
            {
                L_Monster[(i++) % L_Monster.Count].GetComponent<Monster>().NormalAttack();
            }

            yield return new WaitForSeconds(delay);

            if(L_Monster.Count == 0)
            {
                break;
            }
        }
    }
}