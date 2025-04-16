using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class SimplePopUpBase : MonoBehaviour
{
    [SerializeField]protected Text mainDescription;

    public virtual void UpdateMainDescription(List<string> descriptionList)
    {
        if(descriptionList.Count > 0)
            mainDescription.text = descriptionList[0];

        // 2개 이상의 문자열인 경우 줄바꿈을 적용함
        for (int i = 1; i<descriptionList.Count; i++)
        {
            mainDescription.text += $"\n{descriptionList[i]}";
        }
    }

    public virtual void UpdateMainDescription(string description)
    {
        mainDescription.text = description;
    }
}
