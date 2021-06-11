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
        
        if (File.Exists(filePath)) //������ �����Ҷ�
        {
            var data = File.ReadAllText(filePath); // ���� �ҷ�����
            GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //var descListOb = GameManager.Instance.Scoreinfo.OrderBy(x => x.score);
            //var json = JsonConvert.SerializeObject(descListOb);

            Score.text = GameManager.Instance.Scoreinfo[GameManager.Instance.Scoreinfo.Count-1].score.ToString(); // ���� ���� ǥ��

            var sortedProducts = (from prod in GameManager.Instance.Scoreinfo // �������� ���� (���� ���� �����)
                                  orderby prod.score descending
                                  select prod).ToList();

            for (int i = 0; i < 5; i++) // ��ŷ ���� 5�� ǥ���ϱ�
            {
                ScoreRank[i].text = sortedProducts[i].score.ToString();
            }
        }
    }
}
