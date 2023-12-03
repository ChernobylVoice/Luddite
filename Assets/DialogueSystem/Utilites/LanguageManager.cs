using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    // 싱글톤 instance
    public static LanguageManager Instance { get; private set; }
    private Hashtable _hashtable;
    private string _filePath;

    // singleton 선언용 Awake. start랑 분리함.
    private void Awake()
    {
        // Instance가 존재시 처리.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        // Scene이 변경되었을 때, 유지되도록.
        DontDestroyOnLoad(this);
    }

    // 초기 선언
    private void Start()
    {
        _hashtable = new Hashtable();
        _filePath = Application.dataPath + "/DialogueSystem/Localization/";
        LoadXml();
    }

    // 언어 파일 불러오기.
    private void LoadXml()
    {
        var xmlDoc = new XmlDocument();
        // 후에 english or korean 정하기 위한 확장성
        xmlDoc.Load(_filePath+"korean.xml");

        var xmlList = xmlDoc.GetElementsByTagName("content");

        // _hashtable에 데이터 저장.
        foreach (XmlNode item in xmlList)
            if (item.Attributes != null) _hashtable.Add(item.Attributes["id"].Value, item.InnerText);
    }
    
    /// <summary>
    /// 각 component에서 필요한 text를 key값을 받아 _hashtable에 저장된 text를 return
    /// </summary>
    /// <param name="key">text key</param>
    /// <returns>text</returns>
    public string GetString(string key)
    {
        return _hashtable.ContainsKey(key) ? _hashtable[key].ToString() : null;
    }

    #region editorDialogueSystem
    /// <summary>
    /// editor의 node based Dialogue System을 위한 코드.
    /// </summary>
    public void SaveXml()
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(_filePath+"korean.xml");

        var xmlElement = xmlDoc.CreateElement("content");
        
    }
    
    /// <summary>
    /// Dialogue Data만 return, editor의 node based Dialogue System을 위한 코드.
    /// </summary>
    /// <returns>list : key, string 반복</returns>
    public List<string> GetDialogueData()
    {
        var list = new List<string>();
        foreach (var key in _hashtable.Keys)
        {
            var temp = key.ToString();
            if (temp[0] != 'D') continue;
            list.Add(key.ToString());
            list.Add(_hashtable[key].ToString());
        }
        return list;
    }
    #endregion
}
