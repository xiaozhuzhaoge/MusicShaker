using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerBase : MonoBehaviour {
    public static Dictionary<string, GameObject> CurrentUIPlane = new Dictionary<string, GameObject>();
    private static GameObject UIPos;
	// Use this for initialization
	public void Init () 
    {
		UIPos = GameObject.FindGameObjectWithTag ("UIParent").gameObject;
		ShowUI ("MainPanel");
    }
	
    //// Update is called once per frame
    //void Update () 
    //{
		
    //}
    /// <summary>
    /// 显示UI界面
    /// </summary>
    /// <param name="UIName"></param>
    /// <param name="LocalTransForm"></param>
    public void ShowUI(string UIName)
    {
        if (CurrentUIPlane.ContainsKey(UIName))
        {
            foreach(GameObject _UIPlane in CurrentUIPlane.Values)
            {
                _UIPlane.SetActive(false);
            }

            CurrentUIPlane[UIName].SetActive(true);

        }
        else
        {
            foreach (GameObject _UIPlane in CurrentUIPlane.Values)
            {
                _UIPlane.SetActive(false);
            }

            //GameObject _NowUIPlane = (GameObject)Instantiate(Resources.Load(UIName,typeof(GameObject)));
			GameObject _NowUIPlane = ResourceMgr.LoadResource("UIPrefabs",UIName);

            _NowUIPlane.transform.SetParent(UIPos.transform.root);
//            _NowUIPlane.transform.localPosition = UIPos.transform.localPosition;
//            _NowUIPlane.transform.localRotation = UIPos.transform.localRotation;
//            _NowUIPlane.transform.localScale = UIPos.transform.localScale;
			_NowUIPlane.transform.localPosition = Vector3.zero;
			_NowUIPlane.transform.localRotation = Quaternion.identity;
			_NowUIPlane.transform.localScale = Vector3.one;
            CurrentUIPlane.Add(UIName, _NowUIPlane);
        }
    }
    /// <summary>
    /// 隐藏UI界面
    /// </summary>
    /// <param name="UIName"></param>
    public void HideUI(string UIName)
    {
        if (CurrentUIPlane.ContainsKey(UIName))
        {
             CurrentUIPlane[UIName].SetActive(false);
        }
    }

    /// <summary>
    /// 隐藏所有UI界面
    /// </summary>
    public void HideAllUI()
    {
         foreach (GameObject _UIPlane in CurrentUIPlane.Values)
        {
            _UIPlane.SetActive(false);
        }
    }

    /// <summary>
    ///界面初始化
    /// </summary>
    /// <param name="UIName"></param>
    /// <param name="UIPlane"></param>
    public void InitUIManager(string UIName, GameObject UIPlane)
    {
        CurrentUIPlane.Add(UIName, UIPlane);
    }

    public void OnDestroy()
    {
        CurrentUIPlane.Clear();
    }
}
