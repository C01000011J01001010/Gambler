using PublicSet;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicInteractableBase : InteractableObjectBase
{
    
    private eTextScriptFile _currentFile;
    protected eTextScriptFile defaultFile;
    public eTextScriptFile currentFile
    {
        get 
        { 
            if (_currentFile == eTextScriptFile.None) return _currentFile = defaultFile;
            else return _currentFile;
        }
        protected set => _currentFile = value;
    }


    public static Dictionary<string, DynamicInteractableBase> dynamicInteractableDict = new();

    public void InitDict()
    {
        if (dynamicInteractableDict == null) dynamicInteractableDict = new();

        if (dynamicInteractableDict.ContainsKey(gameObject.name) == false) dynamicInteractableDict.Add(gameObject.name, this);
        else dynamicInteractableDict[gameObject.name] = this;
    }

    /// <summary>
    /// 게임객체와 저장될 파일명을 묶어서 반환
    /// </summary>
    /// <returns></returns>
    public static string GetStringToSave()
    {
        List<string> IndividualSave = new List<string>(dynamicInteractableDict.Count);

        foreach(string key in dynamicInteractableDict.Keys)
        {
            IndividualSave.Add($"{key}:{(int)dynamicInteractableDict[key].currentFile}"); // 파일번호를 객체명과 함께 저장
        }

        string result = string.Join(",", IndividualSave);

        return result;
    }

    public static void SetFileNumToLoad(string savedData)
    {
        string[] IndividualSave = savedData.Split(",");
        
        for(int i = 0;  i < IndividualSave.Length; i++)
        {
            string[] Data = IndividualSave[i].Split(":");

            if(Data.Length == 2 &&
               int.TryParse(Data[1], out int intEnum) &&
               Enum.IsDefined(typeof(eTextScriptFile), intEnum)
               )
            {
                eTextScriptFile SavedTextFile = (eTextScriptFile)intEnum;
                if (SavedTextFile != eTextScriptFile.None)
                {
                    dynamicInteractableDict[Data[0]].gameObject.layer = InteractableLayer;
                    dynamicInteractableDict[Data[0]].currentFile = (eTextScriptFile)intEnum;
                    Debug.Log("데이터 로딩 성공");
                }
                
            }
        }
    }
}
