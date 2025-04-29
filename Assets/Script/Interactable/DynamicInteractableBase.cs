using PublicSet;
using System;
using System.Collections.Generic;

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

    

    private static Dictionary<string, DynamicInteractableBase> _dynamicInteractableDict = new();
    public static Dictionary<string, DynamicInteractableBase> dynamicInteractableDict => _dynamicInteractableDict;

    private void Awake()
    {
        currentFile = eTextScriptFile.None;
        dynamicInteractableDict.Add(gameObject.name, this);
        //if(dynamicInteractableObjectList.Contains(this))
        //{
        //    Debug.LogWarning($"dynamicInteractableObjectList.count == {dynamicInteractableObjectList.Count}");
        //}
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
                eTextScriptFile currentFile = (eTextScriptFile)intEnum;
                if (currentFile != eTextScriptFile.None)
                {
                    dynamicInteractableDict[Data[0]].tag = "Interactable";
                    dynamicInteractableDict[Data[0]].currentFile = (eTextScriptFile)intEnum;
                }
                
            }
        }
    }
}
