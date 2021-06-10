using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class DataFormat
{
    public int number;
    public int score;
    //public List<DataFormat> scoreInfo;

    public DataFormat(int number, int score)
    {
        this.number = number;
        this.score = score;
    }
}