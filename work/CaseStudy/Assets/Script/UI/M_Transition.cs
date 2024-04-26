﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_Transition : MonoBehaviour
{
    private Image image;
    private Material material;

    [Header("イージング関数"),SerializeField]
    private M_Easing.Ease ease;

    /// <summary>
    /// シーン遷移先の名前
    /// </summary>
    public string sceneName;

    /// <summary>
    /// シーン遷移開始
    /// </summary>
    private bool isTransition;

    /// <summary>
    /// 時間計測
    /// </summary>
    private float fTime = 0.0f;

    /// <summary>
    /// 現在のトランジションの進行度
    /// </summary>
    private float val = 1.0f;

    [Header("Animation Duration")]
    [SerializeField] private float duration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        material = image.material;
        this.material.SetFloat("_Val", val);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTransition)
        {
            fTime += Time.deltaTime;

            //関数登録
            var func = M_Easing.GetEasingMethod(ease);
          
            float t = Mathf.Clamp01(fTime / duration);

            // 値を1から-1に減少させる
            val = 1.0f - func(t) * 2f;

            Debug.Log(val);

            this.material.SetFloat("_Val", val);

            if(val <= -1.0f)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }    

    public void LoadScene()
    {
        isTransition = true;
    }
}
