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
    //[SerializeField]
    //List<DataFormat> Scoreinfo = new List<DataFormat>();
    string filePath;

    // Start is called before the first frame update
    private void Start()
    {
        filePath = Application.persistentDataPath + "/SaveFile" + "/score.json";
        GameManager.Instance.LoadOperate += LoadScore;
        //GameManager.Instance.LoadRank += LoadRanked;
    }

    public void LoadScore()
    {
        
        if (File.Exists(filePath))
        {

            var data = File.ReadAllText(filePath);
            GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

            //var descListOb = GameManager.Instance.Scoreinfo.OrderBy(x => x.score);
            //var json = JsonConvert.SerializeObject(descListOb);

            Score.text = GameManager.Instance.Scoreinfo[GameManager.Instance.Scoreinfo.Count-1].score.ToString();

            var sortedProducts = (from prod in GameManager.Instance.Scoreinfo
                                  orderby prod.score descending
                                  select prod).ToList();

            for (int i = 0; i < 5; i++)
            {
                ScoreRank[i].text = sortedProducts[i].score.ToString();
            }
        }
    }

    //public void LoadRanked()
    //{
    //    if (File.Exists(filePath))
    //    {
    //        var data = File.ReadAllText(filePath);

    //        Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);
    //        var descListOb = Scoreinfo.OrderBy(x => x.score);
    //        var json = JsonConvert.SerializeObject(descListOb);

    //        var sortedProducts = (from prod in Scoreinfo
    //                             orderby prod.score descending
    //                             select prod).ToList();

    //        for(int i=0; i<5; i++)
    //        {
    //            Score1.text = Scoreinfo[i].number + " : " + sortedProducts[i].score + "\n";
    //            Score1.text = Scoreinfo[i].number + " : " + sortedProducts[i].score + "\n";
    //        }

    //    }
    //}
}
