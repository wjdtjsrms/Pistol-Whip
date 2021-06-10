using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;

//일반 형태 파일 저장

public class ScoreSave : MonoBehaviour
{
    //[SerializeField]
    //List<DataFormat> Scoreinfo = new List<DataFormat>();


    string folderPath;
    string filePath;

    // Start is called before the first frame update
    private void Start()
    {
        folderPath = Application.persistentDataPath + "/SaveFile";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        GameManager.Instance.SaveOperate += SaveInformationFile;
    }

    public void SaveInformationFile()
    {
        //for(int i=0; i<count; i++)
        //{
        //    Scoreinfo.Add(new DataFormat(count, GameManager.Instance.Score));
        //}

        filePath = "/score.json";

        //var data = File.ReadAllText(filePath);

        //Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);

        //Scoreinfo.Add(new DataFormat(GameManager.Instance.Count, GameManager.Instance.Score));
        //GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<GameManager>>(File.ReadAllText(filePath));

        if(File.Exists(folderPath + filePath))
        {
            var data = File.ReadAllText(folderPath + filePath);
            if (data != null)
            {
                GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);
            }
        }          

        DataFormat temp = new DataFormat(GameManager.Instance.Scoreinfo.Count+1, GameManager.Instance.Score);

        GameManager.Instance.Scoreinfo.Add(temp);

        var score = JsonConvert.SerializeObject(GameManager.Instance.Scoreinfo);
        

        File.WriteAllText(folderPath + filePath, score);
    }
}
