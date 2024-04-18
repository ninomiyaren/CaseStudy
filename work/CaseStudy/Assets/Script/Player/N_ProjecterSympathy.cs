using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ProjecterSympathy : MonoBehaviour
{
    [Header("広がるスピード"), SerializeField]
    private float fSpreadSpeed = 1.0f;

    [Header("最大半径"), SerializeField]
    private float fMaxRadius = 4.0f;

    [Header("線の太さ"), SerializeField]
    private float fLineWidth = 0.1f;

    [Header("円周上の点の数"), SerializeField]
    private int iSegments = 20;

    [Header("マテリアル"), SerializeField]
    private Material material;

    // 半径
    private float fRadius = 0.0f;

    // 共鳴中か
    private bool isSympathy = false;

    private LineRenderer lineRenderer;

    private Transform trans_Player;

    private Vector3 SympathyPosition = Vector3.zero;

    // スタート関数実行後に最初に実行される
    private bool isInitialized = false;

    // 投影機のスクリプトを格納するリスト
    private List<N_ProjectHologram> list = new List<N_ProjectHologram>();

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = material;

        trans_Player = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 最初だけ実行
        if (!isInitialized)
        {
            // ステージに配置された投影機の親オブジェクト取得
            Transform ParentTrans = GameObject.Find("Projecters").gameObject.GetComponent<Transform>();

            int i = 0;
            // 子オブジェクト（各プロジェクター）のスクリプトを格納
            foreach (Transform child in ParentTrans)
            {
                // リストに入れる
                list.Add(child.GetComponent<N_ProjectHologram>());
                i++;
            }

            isInitialized = true;
        }

        // キー入力があったら共鳴開始
        if(Input.GetKeyDown(KeyCode.E)/* || Input.GetButtonDown("Sympathy")*/)
        {
            if(isSympathy == false)
            {
                isSympathy = true;
                // 表示
                lineRenderer.enabled = true;
                SympathyPosition = trans_Player.position;

                // リストに格納された各オブジェクト
                foreach (N_ProjectHologram SC_Holo in list)
                {
                    // 初期化
                    SC_Holo.Initialize();
                }
            }
        }

        // 共鳴中
        if (isSympathy)
        {
            // 時間経過で広がる円を描画

            // 半径を広げる
            fRadius += fSpreadSpeed * Time.deltaTime;
            
            // 最後のループ
            if(fRadius >= fMaxRadius)
            {
                fRadius = fMaxRadius;

                isSympathy = false;
            }

            // 円を描画する処理
            lineRenderer.widthMultiplier = fLineWidth;
            lineRenderer.positionCount = iSegments + 1;

            float deltaTheta = (2f * Mathf.PI) / iSegments;
            float theta = 0f;

            for (int i = 0; i < iSegments + 1; i++)
            {
                float x = fRadius * Mathf.Cos(theta);
                float y = fRadius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(SympathyPosition.x + x,SympathyPosition.y + y, 0f);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }

            // リストに格納されたオブジェクトを総当たりで当たり判定
            foreach(N_ProjectHologram SC_Holo in list)
            {
                SC_Holo.CheckAreaSympathy(SympathyPosition,fRadius);
            }

            // 最後のループに入る
            if (!isSympathy)
            {
                fRadius = 0.0f;
                lineRenderer.enabled = false;
            }
            
        }
    }
}
