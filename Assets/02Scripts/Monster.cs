using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData mdata;

    GameObject[] player;
    Rigidbody2D rb;

    public Vector3 originalPos;
    Animator animator;

    public bool Back = false;
    public bool Home = true;

    public int hp;
    public int maxHp;
    public int Attack = 5;

    public GameObject DamageCanvas;
    Text DamageText; 

    public float moveSpeed = 20f;

    void Start()
    {
        //p_Data = pdata;
        player = GameObject.FindGameObjectsWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        animator = GetComponent<Animator>();

        hp = mdata.hp;
        maxHp = mdata.maxhp;
        hp = maxHp;
    }

    public void NormalAttack()
    {
        if (GameManager.instance.CurrTurn == true && Home)
        {
            StartCoroutine("NormalAttackCT");
        }
    }

    IEnumerator NormalAttackCT()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        Back = false;

        // 몬스터가 없으면 오류가 나므로 예외처리 추가
        if (player.Length == 0) yield break;

        int r = Random.Range(0, player.Length);

        while (true)
        {
            if (player[r] != null)
            {
                Home = false;
                rb.MovePosition(Vector3.Lerp(transform.position, player[r].transform.position, moveSpeed * Time.deltaTime));

                // 2. 도착 확인
                if (Vector3.Distance(transform.position, player[r].transform.position) <= 0.5f)
                {
                    // 공격처리 (애니메이션, 사운드, 데미지)
                    animator.SetTrigger("Attack");
                    player[r].GetComponent<Player>().Damage(mdata.Attack);

                    Debug.Log("공격!");
                    yield return new WaitForSeconds(0.3f);
                    Back = true;
                    break;
                }
                yield return null;
            }
            else
            {
                break;
            }
        }
    }
    void Update()
    {
        if (Back == true)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, originalPos, moveSpeed * Time.deltaTime));

            if (Vector3.Distance(transform.position, originalPos) <= 0.5f)
            {
                transform.position = originalPos;
                Home = true;
                Back = false;
            }
        }
    }

    public void Damage(int attack)
    {
        hp -= attack;
        animator.SetTrigger("TakeDamage");
       
        GameObject go = Instantiate(DamageCanvas, transform.position, Quaternion.identity);
        //go.transform.parent = transform;

        DamageText = go.GetComponentInChildren<Text>();
        DamageText.text = attack.ToString();

        if (hp <= 0)
        {
            GameManager.instance.L_Monster.Remove(gameObject);
            Destroy(gameObject);
        }
    }

}
