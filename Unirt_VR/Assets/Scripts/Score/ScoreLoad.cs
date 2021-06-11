using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;

public class ScoreLoad : MonoBehaviour
{
    [SerializeField]
    Text Score;
    [SerializeField]
    Text[] ScoreRank;
    string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/SaveFile" + "/score.json";
        GameManager.Instance.LoadOperate += LoadScore;
    }

    public void LoadScore()
    {
        
        if (File.Exists(filePath)) //파일이 존재할때
        {
            var data = File.ReadAllText(filePath); // 파일 불러오기
            GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //var descListOb = GameManager.Instance.Scoreinfo.OrderBy(x => x.score);
            //var json = JsonConvert.SerializeObject(descListOb);

            Score.text = GameManager.Instance.Scoreinfo[GameManager.Instance.Scoreinfo.Count-1].score.ToString(); // 현재 점수 표시

            var sortedProducts = (from prod in GameManager.Instance.Scoreinfo // 내림차순 정렬 (점수 높은 순대로)
                                  orderby prod.score descending
                                  select prod).ToList();

            for (int i = 0; i < 5; i++) // 랭킹 상위 5명 표시하기
            {
                ScoreRank[i].text = sortedProducts[i].score.ToString();
            }
        }
    }
}
