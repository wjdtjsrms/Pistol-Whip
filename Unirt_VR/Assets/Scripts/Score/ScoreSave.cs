using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Linq;

public class ScoreSave : MonoBehaviour
{
    string folderPath;
    string filePath;

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

        filePath = "/score.json"; // json ���� �̸�

        if(File.Exists(folderPath + filePath)) //������ �����Ҷ� json���� �ε��ؼ� �ҷ�����
        {
            var data = File.ReadAllText(folderPath + filePath);
            if (data != null)
            {
                GameManager.Instance.Scoreinfo = JsonConvert.DeserializeObject<List<DataFormat>>(data);
            }
        }          

        DataFormat temp = new DataFormat(GameManager.Instance.Scoreinfo.Count+1, GameManager.Instance.Score);

        GameManager.Instance.Scoreinfo.Add(temp); // �ѹ��� ���� list�� �߰�

        var score = JsonConvert.SerializeObject(GameManager.Instance.Scoreinfo);
        

        File.WriteAllText(folderPath + filePath, score); //���Ͽ� ����
    }
}
