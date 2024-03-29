﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //ジャンプ処理用
    public float maxJampPower = 10;  //最高ジャンプ力
    float jampPower;                 //ジャンプ力調節
    float y;                         //スペースキーon(+1)、off(0)
    float currentDropSpeed = 0;      //現在の落下速度
    public int maxJampCount = 2;
    int jampCount = 0;               //ジャンプの回数

    //Player移動用
    float playerPositionX; //PlayerのX座標
    int ceilPlayPosiX;     //切り上げ後

    float x = 0; //正方向に加える力
    float currentSpeed = 0;  //現在の速度

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }   

    // Update is called once per frame
    void Update()
    {
        currentSpeed = GetComponent<Rigidbody>().velocity.x; //速度ベクトル（横）
        currentDropSpeed = GetComponent<Rigidbody>().velocity.y; //速度ベクトル（縦）
        playerPositionX = this.transform.position.x; //Playerのx座標取得
        ceilPlayPosiX = Mathf.CeilToInt(playerPositionX);  //切り上げてint型に変換

        jampPower = maxJampPower;

        //ジャンプ処理
        if (jampCount < maxJampCount)//ジャンプの回数が上限を超えていないなら
        {
            if (Input.GetKeyDown(KeyCode.Space))// スペースキーが押されたら
            {
                y = 1;

                jampPower = jampPower - currentDropSpeed; //落下速度に応じてジャンプ力調節

                //ジャンプ力がマイナスなら0に
                if (jampPower <= 0)
                {
                    jampPower = 0;
                }

                jampCount += 1;
            }
        }
            //初期座標より左側なら
            if (ceilPlayPosiX < 0)
        {
            x = 1;
        }

        //現在の速度が正方向なら
        if (currentSpeed >= 0)
        {
            if (ceilPlayPosiX == 0) //目標の座標なら
            {
                x = currentSpeed * (-1);
            }
        }

    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(x * 20, y * jampPower * 50 - 50, 0); //[-50]は重力
        y = 0;
    }

    //地面と触れたらジャンプ回数をリセット
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground") //地面なら
        {
            jampCount = 0;
        }
    }
}
