using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerData pdata;
    //public MonsterData mdata;
    //public PlayerData p_Data;

    GameObject[] monster;
    Rigidbody2D rb;

    public bool Back = false;
    public bool Home = true;

    public Vector3 originalPos;

    Animator animator;
    public float moveSpeed = 20f;


    public GameObject magicAura;
    public Transform t_MagicAura;
    public GameObject explosion;

    public GameObject DamageCanvas;
    Text DamageText;

    void Start()
    {
        //p_Data = pdata;
        monster = GameObject.FindGameObjectsWithTag("Monster");
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        animator = GetComponent<Animator>();
    }

    public void NormalAttack()
    {
        if (GameManager.instance.CurrTurn == false && Home)
        {
            StartCoroutine("NormalAttackCT");
        }
    }

    IEnumerator NormalAttackCT()
    {
        monster = GameObject.FindGameObjectsWithTag("Monster");
        Back = false;

        // 몬스터가 없으면 오류가 나므로 예외처리 추가
        if (monster.Length == 0) yield break;

        int r = UnityEngine.Random.Range(0, monster.Length);

        while (true)
        {
            if (monster[r] != null)
            {
                Home = false;
                rb.MovePosition(Vector3.Lerp(transform.position, monster[r].transform.position, moveSpeed * Time.deltaTime));

                // 2. 도착 확인
                if (Vector3.Distance(transform.position, monster[r].transform.position) <= 0.5f)
                {
                    // 공격처리 (애니메이션, 사운드, 데미지)
                    animator.SetTrigger("Attack");
                    monster[r].GetComponent<Monster>().Damage(pdata.attack);
                    Sound();
                    Debug.Log("공격!");
                    yield return new WaitForSeconds(0.3f);
                    Back = true;
                    break;
                    // ===============================================
                }
                yield return null;
            }
            else
            {
                break;
            }
        }
    }

    private void Sound()
    {
        if(pdata.job == "검사")
        {
            SoundManager.instance.PlayAttackSound(8);
        }
        if(pdata.job == "사제")
        {
            SoundManager.instance.PlayAttackSound(4);
        }
        if (pdata.job == "마법사")
        {
            SoundManager.instance.PlayAttackSound(2);
        }
    }

    public void Damage(int Attack)
    {
        pdata.hp -= Attack;
        animator.SetTrigger("TakeDamage");

        GameObject go = Instantiate(DamageCanvas, transform.position, Quaternion.identity);
        //go.transform.parent = transform;

        DamageText = go.GetComponentInChildren<Text>();
        DamageText.text = Attack.ToString();

        //플레이어
        if (pdata.hp <= 0)
        {
            GameManager.instance.D_Player.Remove(gameObject.name);
            Destroy(gameObject);
        }
    }

    public void Die()
    {
        Debug.Log("Die");
    }

    // Update is called once per frame
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

    public void SpecialAttack()
    {
        if (GameManager.instance.CurrTurn == false && Home)
        {
            StartCoroutine("SpecialAttackCT");
        }
    }

    IEnumerator SpecialAttackCT()
    {
        int r = UnityEngine.Random.Range(0, monster.Length);
        Instantiate(magicAura,t_MagicAura.position, t_MagicAura.rotation);
        yield return new WaitForSeconds(1.5f);

        if(monster[r] != null)
        {
            if (!pdata.job.Equals("사제"))
            {
                //몬스터 위치에 스킬
                Instantiate(explosion, monster[r].transform.position,quaternion.identity);
                monster[r].GetComponent<Monster>().Damage(pdata.attack * 10);
            }
            else
            {
                //아군 위치에 스킬
                GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
                int i = UnityEngine.Random.Range(0, player.Length);
                Instantiate(explosion, player[i].transform.position + Vector3.up * 0.8f, quaternion.identity);
                player[i].GetComponent<Player>().pdata.hp += (pdata.attack * 10);
            }
        }
    }
}
