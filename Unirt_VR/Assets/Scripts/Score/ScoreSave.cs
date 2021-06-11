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

        filePath = "/score.json"; // json 파일 이름

        if(File.Exists(folderPath + filePath)) //파일이 존재할때 json파일 로드해서 불러오기
        {
            var data = File.ReadAllText(folderPath + filePath);
            if (data != null)
            {
                GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);
            }
        }          

        DataFormat temp = new DataFormat(GameManager.Instance.Scoreinfo.Count+1, GameManager.Instance.Score);

        GameManager.Instance.Scoreinfo.Add(temp); // 넘버와 점수 list에 추가

        var score = JsonConvert.SerializeObject(GameManager.Instance.Scoreinfo);
        

        File.WriteAllText(folderPath + filePath, score); //파일에 저장
    }
}
