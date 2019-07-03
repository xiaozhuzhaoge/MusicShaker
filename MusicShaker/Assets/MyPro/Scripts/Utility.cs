using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SimpleJson;
using System.Text;
using System.Security.Cryptography;
using System.Linq;


public delegate bool BoolAction<T>(T obj);

public delegate bool BoolAction();

public delegate float FloatAction();

public delegate T TAction<T>();

public delegate IEnumerator CoroutineAction();

public delegate IEnumerator CoroutineAction_Params(params object[] args);

public delegate IEnumerator CoroutineAction<T>(T obj);

public delegate IEnumerator CoroutineAction<T1, T2>(T1 obj1, T2 obj2);


public class Utility : MonoBehaviour
{
    public static Utility instance;

    void Awake()
    {
        instance = this;
    }

    public static void EditorDebug(params object[] args)
    {
#if UNITY_EDITOR
        var output = "";
        foreach (var arg in args)
        {
            output += "===";
            output += arg.ToString();
        }
        Debug.Log(output);
#endif

    }

    /// <summary>
    /// Sync Load perfabs
    /// </summary>
    /// <param name="path"></param>
    /// <param name="Callback"></param>
    public void LoadGameObject(string path, Action<UnityEngine.GameObject> Callback)
    {
        Load(path, (asset) =>
        {
            if (asset == null)
            {
                throw new UnityException("can not load game object:" + path);
            }
            GameObject go = (GameObject)Instantiate(asset);
            Callback(go);
        });
    }

    /// <summary>
    /// Async Load perfabs
    /// </summary>
    /// <param name="path"></param>
    /// <param name="Callback"></param>
    public void LoadGameObjectAsync(string path, Action<UnityEngine.GameObject> Callback)
    {
        //if (SceneManager.couroutineObj == null)
        //{
        //    Debug.Log(" 场景切换中，终止场景内的异步加载执行！");
        //    return;
        //}
        //CoroutineObj obj = SceneManager.couroutineObj.GetComponent<CoroutineObj>();
        //obj.StartCoroutine(LoadAsync(path, (asset) =>
        //{
        //    if (asset == null)
        //    {
        //        throw new UnityException("can not load game object:" + path);
        //    }
        //    GameObject go = (GameObject)Instantiate(asset);
        //    Callback(go);
        //}));
    }

    IEnumerator LoadAsync(string name, Action<UnityEngine.Object> Callback)
    {
        ResourceRequest request = Resources.LoadAsync(name);
        yield return request;
        yield return new WaitForSeconds(0.03f);
        Callback(request.asset);
    }

    public void LoadObject<T>(string path, Action<T> Callback = null) where T : UnityEngine.Object
    {
        Load(path, (asset) =>
        {
            if (asset == null)
            {
                throw new UnityException("can not load object:" + path);
            }
            if (Callback != null)
            {
                Callback(asset as T);
            }
        });
    }

    void Load(string name, Action<UnityEngine.Object> Callback)
    {
        Callback(Resources.Load(name));
    }

    public static void SetActiveChildren(Transform parent, bool active)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject go = parent.GetChild(i).gameObject;
            go.SetActive(active);
        }
    }

    public void SequenceInterval(float interval, params Action[] actions)
    {
        StartCoroutine(IntervalExecutes(interval, actions));
    }


    IEnumerator IntervalExecutes(float sec, Action[] actions)
    {
        int index = 0;
        foreach (var action in actions)
        {
            yield return new WaitForSeconds(sec);
            if (action != null)
            {
                action();
            }
            index++;
        }
    }

    public void WaitForSecond(float sec, Action cb)
    {
        StartCoroutine(Wait(sec, cb));
    }

    public IEnumerator Wait(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        cb();
    }

    public void WaitForFrame(int frameNum, Action cb)
    {
        StartCoroutine(Wait(frameNum, cb));
    }

    public IEnumerator Wait(int frameNum, Action cb)
    {
        do
        {
            yield return new WaitForEndOfFrame();
            frameNum--;
        } while (frameNum > 0);
        cb();
    }

    public static string FormatTime(int sec, int length = 3)
    {
        string hour = "00";
        string mins = "00";
        string secs = "00";
        TimeString(sec / 3600, ref hour);
        TimeString(sec % 3600 / 60, ref mins);
        TimeString(sec % 60, ref secs);

        if (length == 2)
            return String.Format("{0}:{1}", mins, secs);
        if (length == 1)
            return String.Format("{0}", secs);
        return String.Format("{0}:{1}:{2}", hour, mins, secs);
    }

    static void TimeString(int time, ref string output)
    {
        output = time.ToString();
        if (output.Length == 1)
        {
            output = "0" + output;
        }
    }

    public static long ClampLong(long value, long min, long max)
    {
        value = Math.Min(value, max);
        value = Math.Max(value, 0);
        return value;
    }

    public static void DestroyAllChildren(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
        parent.transform.DetachChildren();
    }

    public static Vector3 GetUIPosByWorldPos(Camera uicamera, Vector3 worldPos)
    {
        Vector3 mainCameraPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector3 pos = uicamera.ScreenToWorldPoint(mainCameraPos);
        return pos;
    }


    public static void ClearArray<T>(T[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = default(T);
        }
    }

    public static Vector3 GetCenterFromPoints(IList<Vector3> vecs)
    {
        if (vecs.Count == 0)
        {
            return Vector3.zero;
        }
        if (vecs.Count == 1)
        {
            return vecs[0];
        }
        Bounds bounds = new Bounds(vecs[0], Vector3.zero);
        for (var i = 1; i < vecs.Count; i++)
        {
            bounds.Encapsulate(vecs[i]);
        }
        return bounds.center;
    }

    /// <summary>
    /// Spawns the mesh from bottom points.
    /// </summary>
    /// <returns>center of bottom</returns>
    /// <param name="posList">Position list.</param>
    public static void SpawnMeshFromBottomPoints(List<Vector3> posList, GameObject target)
    {
        Vector3 center = Utility.GetCenterFromPoints(posList);
        target.transform.localPosition = center;
        List<Vector3> vecs = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int i = 0; i < posList.Count; i++)
        {
            Vector3 vec2_t, vec1_t, vec2, vec1 = posList[i] - center;
            if (i + 1 == posList.Count)
            {
                vec2 = posList[0] - center;
            }
            else
            {
                vec2 = posList[i + 1] - center;
            }
            vec1_t = vec1 + new Vector3(0, 50, 0);
            vec2_t = vec2 + new Vector3(0, 50, 0);
            vecs.Add(vec1);
            vecs.Add(vec1_t);
            vecs.Add(vec2);
            vecs.Add(vec2_t);
            triangles.Add(i * 4 + 0);
            triangles.Add(i * 4 + 1);
            triangles.Add(i * 4 + 2);
            triangles.Add(i * 4 + 1);
            triangles.Add(i * 4 + 3);
            triangles.Add(i * 4 + 2);
        }
        MeshFilter filter = target.AddComponent<MeshFilter>();
        filter.mesh = new Mesh();
        filter.mesh.vertices = vecs.ToArray();
        filter.mesh.triangles = triangles.ToArray();
        filter.mesh.RecalculateBounds();
        filter.mesh.RecalculateNormals();
        MeshCollider col = target.AddComponent<MeshCollider>();
        col.convex = true;

    }

    private static void optimized_Destory(GameObject go,float time)
    {
        DestorySelf des = go.AddComponent<DestorySelf>();
        des.delay = time;
    }

    public static CoroutineObj StartSceneCoroutineTimer(CoroutineAction co)
    {
        GameObject go = new GameObject();
        CoroutineObj obj = go.AddComponent<CoroutineObj>();
        obj.StartCoroutine(co());
        go.name = "_Timer" + co.Target.ToString();
        return obj;
    }


    /// <summary>
    /// 会随着场景切换而清理的coroutine
    /// </summary>
    /// <param name="co">Co.</param>
    public static CoroutineObj StartSceneCoroutine(CoroutineAction co,bool destory = true)
    {
        GameObject go = new GameObject();
        CoroutineObj obj = go.AddComponent<CoroutineObj>();
        obj.StartCoroutine(co());
        go.name = "_co" + co.Target.ToString();
        if(destory)
            optimized_Destory(go,3f);
        //else 
        //    SceneManager.instance.clearOnChangeScene.Add(go);
        return obj;
    }

    public static CoroutineObj StartSceneCoroutine(CoroutineAction_Params co,bool destory = true, params object[] args)
    {
        GameObject go = new GameObject();
        CoroutineObj obj = go.AddComponent<CoroutineObj>();
        obj.StartCoroutine(co(args));
        go.name = "_co" + co.Target.ToString();
        if (destory)
            optimized_Destory(go, 3f);
        //else
        //    SceneManager.instance.clearOnChangeScene.Add(go);
        return obj;
    }

    public static CoroutineObj StartSceneCoroutine<T1>(CoroutineAction<T1> co, T1 t1,bool destory = true)
    {
        GameObject go = new GameObject();
        CoroutineObj obj = go.AddComponent<CoroutineObj>();
        obj.StartCoroutine(co(t1));
        go.name = "_co" + co.Target.ToString();
        if (destory)
            optimized_Destory(go, 3f);
        //else
        //    SceneManager.instance.clearOnChangeScene.Add(go);
        return obj;
    }


    /// <summary>
    /// 会随着场景切换而清理的coroutine
    /// </summary>
    /// <param name="co">Co.</param>
    public static CoroutineObj StartSceneCoroutine<T1, T2>(CoroutineAction<T1, T2> co, T1 t1, T2 t2, bool destory = true)
    {
        GameObject go = new GameObject();
        CoroutineObj obj = go.AddComponent<CoroutineObj>();
        obj.StartCoroutine(co(t1, t2));
        go.name = "_co" + co.Target.ToString();
        if (destory)
            optimized_Destory(go, 3f);
        //else
        //    SceneManager.instance.clearOnChangeScene.Add(go);
        return obj;
    }

    public static bool Roll(float rate)
    {
        return UnityEngine.Random.value <= rate;
    }

    public static void ResetTransform(Transform tran)
    {
        tran.localPosition = Vector3.zero;
        tran.localRotation = Quaternion.identity;
        tran.localScale = Vector3.one;
    }

    Dictionary<string, GameObject> timeoutHandlers = new Dictionary<string, GameObject>();

    /// <summary>
    /// 手动清理的coroutine ; destroy gameObject 以停止
    /// </summary>
    /// <param name="co">Co.</param>

    public static float GetTimeByFixedFrame(float frames)
    {
        return frames * Time.fixedDeltaTime;
    }


    public static void SortDictionary<T1, T2>(ref Dictionary<T1, T2> members, Comparison<KeyValuePair<T1, T2>> comparer)
    {
        List<KeyValuePair<T1, T2>> list = new List<KeyValuePair<T1, T2>>();
        foreach (var kv in members)
        {
            list.Add(kv);
        }
        members.Clear();

        list.Sort(comparer);

        foreach (var kv in list)
        {
            members.Add(kv.Key, kv.Value);
        }
    }


    public static Quaternion RotateToDir(Transform transform, Vector3 dir)
    {
        return Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, 2, 0));
    }

    public static Quaternion RotateToTarget_onlyY(Transform transform, Vector3 pos)
    {
        var dir = new Vector3(pos.x, transform.position.y, pos.z) - transform.position;
        return RotateToDir(transform, dir);
    }


    public static void SetLayerWithChildren(GameObject go, int layer)
    {
        foreach (var t in go.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = layer;
        }
    }


    public static void GetActiveChildren(List<GameObject> array, ref List<GameObject> activeArray)
    {
        activeArray.Clear();
        foreach (GameObject item in array)
        {
            if (item.activeSelf)
                activeArray.Add(item);
        }
    }

    static float GetDisToTrans(Vector3 curPos, Transform trans, bool world)
    {
        float distance = Vector3.Distance(curPos, trans.localPosition);
        if (world)
            distance = Vector3.Distance(curPos, trans.position);
        return distance;
    }



    public static int FindItemCount<T>(List<T> lists, T value)//
    {
        int count = 0;
        foreach (var item in lists)
        {
            if (value.Equals(item))
                count++;
        }
        return count;
    }

    public static List<int> GetListByString(string _content, string _symbol)
    {
        List<int> _list = new List<int>();
        string[] _temp = _content.Split(_symbol.ToCharArray());
        foreach (string _item in _temp)
        {
            if (_item == "")
                continue;
            _list.Add(Convert.ToInt32(_item));
        }

        return _list;
    }

    public static bool GetKeyValueByIndex<T1, T2>(Dictionary<T1, T2> _dic, ref T2 _value, int _index)
    {
        int i = 1;
        foreach (var _item in _dic)
        {
            if (i == _index)
            {
                _value = _item.Value;
                return true;
            }
            i++;
        }
        return false;
    }

    public static void DicCopy<T1, T2>(Dictionary<T1, T2> _original, ref Dictionary<T1, T2> _target)
    {
        _target.Clear();
        foreach (var _item in _original)
        {
            _target.Add(_item.Key, _item.Value);
        }
    }

    public static bool CheckUiCanInstance(MonoBehaviour ui, GameObject obj)
    {
        if (ui != null)
        {
            Utility.DestroyObject(obj);
            return false;
        }
        return true;
    }

    public static void DicConn<T1, T2>(ref Dictionary<T1, T2> _original, Dictionary<T1, T2> _target)
    {
        foreach (var _item in _target)
        {
            _original.Add(_item.Key, _item.Value);
        }
    }

    public static void ListCopy<T1>(List<T1> _original, ref List<T1> _target)
    {
        _target.Clear();
        foreach (var _item in _original)
        {
            _target.Add(_item);
        }
    }

    public static long GetSecTimeByString(string _time, string _symbol)
    {
        long _timeSec = 0;
        if (_time == "0")
            return _timeSec;
        List<int> _list = Utility.GetListByString(_time, _symbol);

        _timeSec += _list[0] * 3600;
        _timeSec += _list[1] * 60;
        _timeSec += _list[2];
        return _timeSec;
    }

    public static string Serialize(string originalStr)
    {
        byte[] originalStrAsBytes = Encoding.UTF8.GetBytes(originalStr);
        byte[] keys = new byte[] {
			0xC8,
			0xAA,
			0xFD,
			0xC9,
			0xBB,
			0xFA,
			0xCA,
			0xCC,
			0xAF,
			0xBF,
			0xDD,
			0xC6,
			0xBC,
			0xBC
		};
        using (MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length))
        {
            for (int i = 0; i < originalStrAsBytes.Length; i++)
            {
                byte x = originalStrAsBytes[i];
                x = (byte)(x ^ keys[i % keys.Length]);
                x = (byte)(~x);
                memStream.WriteByte(x);
            }

            originalStrAsBytes = memStream.ToArray();
        }
        return Convert.ToBase64String(originalStrAsBytes);
    }

    public static string Parse(string serializedStr)
    {
        byte[] serializedStrAsBytes = Convert.FromBase64String(serializedStr);
        byte[] keys = new byte[] {
			0xC8,
			0xAA,
			0xFD,
			0xC9,
			0xBB,
			0xFA,
			0xCA,
			0xCC,
			0xAF,
			0xBF,
			0xDD,
			0xC6,
			0xBC,
			0xBC
		};
        using (MemoryStream memStream = new MemoryStream(serializedStrAsBytes.Length))
        {
            for (int i = 0; i < serializedStrAsBytes.Length; i++)
            {
                byte x = serializedStrAsBytes[i];
                x = (byte)(~x);
                x = (byte)(x ^ keys[i % keys.Length]);
                memStream.WriteByte(x);
            }

            serializedStrAsBytes = memStream.ToArray();
        }
        return Encoding.UTF8.GetString(serializedStrAsBytes);
    }

    public static int GetCircleSectorIndex(int sectorCount, Vector3 originPos, Vector3 forward, Vector3 targetPos)
    {
        float sectorAngle = 360f / sectorCount;
        float halfSector = sectorAngle / 2;
        Vector3 origin = forward;
        origin.y = 0;

        Vector3 toTarget = targetPos - originPos;
        toTarget.y = 0;

        float angle = Vector3.Angle(origin, toTarget);
        if (angle < halfSector)
        {
            return 0;
        }

        var index = Mathf.RoundToInt(Mathf.CeilToInt(angle / halfSector) / 2f);

        Vector3 cross = Vector3.Cross(origin, toTarget);
        if (cross.y < 0)
        {
            index = sectorCount - index;
        }
        return index;
    }

    public static string CTString(object obj)
    {
        return Convert.ToString(obj);
    }

    public static float CTFloat(object obj)
    {
        return Convert.ToSingle(obj);
    }

    public static int CTInt(object obj)
    {
        return Convert.ToInt32(obj);
    }

    public static double CTDouble(object obj)
    {
        return Convert.ToDouble(obj);
    }

    public static long CTLong(object obj)
    {
        return Convert.ToInt64(obj);
    }

    /// <summary>
    /// 创建单一对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="itemPre"></param>
    /// <param name="switchFlag"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject createObj(Transform parent, GameObject itemPre, bool switchFlag, string name)
    {
        GameObject gameO = (GameObject)GameObject.Instantiate(itemPre);
        gameO.transform.parent = parent;
        gameO.transform.localScale = Vector3.one;
        gameO.transform.localPosition = Vector3.one;
        gameO.transform.localRotation = Quaternion.identity;

        gameO.SetActive(switchFlag);
        gameO.name = name;
        return gameO;
    }
    /// <summary>
    /// 带回调的创建对象
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="scale"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="itemPre"></param>
    /// <param name="switchFlag"></param>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public static void createObjCallBack(Transform parent, Vector3 scale, Vector3 position, Quaternion rotation, GameObject itemPre, bool switchFlag, string name, Action<GameObject> callback = null)
    {
        GameObject gameO = (GameObject)GameObject.Instantiate(itemPre);
        gameO.transform.parent = parent;
        gameO.transform.localScale = scale;
        gameO.transform.localPosition = position;
        gameO.transform.localRotation = rotation;

        gameO.SetActive(switchFlag);
        gameO.name = name;
        if (callback != null)
            callback(gameO);
    }

    /// <summary>
    /// 创建格子
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="itemPre"></param>
    /// <param name="switchFlag"></param>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    public static void createObjCallBack(Transform parent, GameObject itemPre, bool switchFlag, string name, Action<GameObject> callback = null)
    {
        GameObject gameO = (GameObject)GameObject.Instantiate(itemPre);
        gameO.transform.parent = parent;
        gameO.transform.localScale = Vector3.one;
        gameO.transform.localPosition = Vector3.zero;
        gameO.transform.localRotation = Quaternion.identity;

        gameO.SetActive(switchFlag);
        gameO.name = name;
        if (callback != null)
            callback(gameO);
    }



    /// <summary>
    /// 生成格子
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="number"></param>
    /// <param name="itemPre"></param>
    /// <param name="list"></param>
    /// <param name="switchFlag"></param>
    /// <param name="callback"></param>
    /// 
    public static void IntilizationBlocks<T>(T parent, int number, GameObject itemPre, List<GameObject> list, bool switchFlag = true, Action<T> callback = null) where T : MonoBehaviour
    {
        for (int i = 0; i < number; i++)
        {
            list.Add(createObj(parent.transform, itemPre, switchFlag, i.ToString()));
        }
        if (callback != null)
        {
            callback(parent);
        }
    }
    /// <summary>
    /// 激活传递GameObjects
    /// </summary>
    /// <param name="collection"></param>
    public static void ActiveAllObjects<T>(T collection, bool active) where T: ICollection<GameObject>
    {
        foreach (var o in collection)
            o.gameObject.SetActive(active);
    }
    /// <summary>
    /// 刷新传递的UIGrid
    /// </summary>
    /// <param name="collection"></param>
    public static void ResetAllGrids<T>(T collection) where T : ICollection<UIGrid>
    {
        foreach(var o in collection)
        {
            o.repositionNow = true;
            o.Reposition();
        }
    }
    /// <summary>
    /// 刷新传递的UIScrollView
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    public static void ResetAllScrollView<T>(T collection) where T : ICollection<UIScrollView>
    {
        foreach (var o in collection)
            o.ResetPosition();
    }
    /// <summary>
    /// 刷新Tab状态
    /// </summary>
    /// <param name="tabs"></param>
    /// <param name="cTabs"></param>
    public static void UpdateTabs(List<GameObject> tabs, List<GameObject> cTabs)
    {
        int cuu = 0;
        try
        {
            cuu = Convert.ToInt32(UICamera.lastHit.collider.name);
        }
        catch (Exception exp)
        {
            Debug.Log(exp);
            cuu = 1;
        }
        for (int i = 0; i < cTabs.Count; i++)
        {
            if (i == cuu)
                cTabs[i].gameObject.SetActive(true);
            else
                cTabs[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化Tab
    /// </summary>
    /// <param name="tabs"></param>
    /// <param name="cTabs"></param>
    /// <param name="current"></param>
    public static void IntilizationTabs(List<GameObject> tabs, List<GameObject> cTabs, int current)
    {
        for (int i = 0; i < cTabs.Count; i++)
        {
            if (i == current)
                cTabs[i].gameObject.SetActive(true);
            else
                cTabs[i].gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// 开启选中ScrollView
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sws"></param>
    /// <param name="currentIndex"></param>
    public static void RefreshScrollViews<T>(T sws, int currentIndex) where T : ICollection<UIScrollView>
    {
        foreach (var s in sws)
            s.gameObject.SetActive(false);
        sws.ToList<UIScrollView>()[currentIndex].gameObject.SetActive(true);
    }

    /// <summary>
    /// 刷新ScrollViews
    /// </summary>
    /// <param name="sws"></param>
    public static void SetOriginalStateForScrollViews<T>(T sws) where T : ICollection<UIScrollView>
    {
        foreach (var s in sws)
            s.ResetPosition();
    }


    public static void ResetGameItemColor(UISprite back)
    {
        back.color = Color.white;
    }
    /// <summary>
    /// 右移ScrollView
    /// </summary>
    /// <param name="scrollView"></param>
    /// <param name="grid"></param>
    public static void rightShiftScrollView(UIScrollView scrollView, UIGrid grid)
    {
        float weight = grid.cellWidth;
        Vector3 currentPosition = scrollView.transform.localPosition;
        scrollView.transform.localPosition = new Vector3(currentPosition.x + weight, currentPosition.y, currentPosition.z);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector3(currentPosition.x + weight, currentPosition.y, currentPosition.z);
    }

    /// <summary>
    /// 左移ScrollView
    /// </summary>
    /// <param name="scrollView"></param>
    /// <param name="grid"></param>
    public static void leftShiftScrollView(UIScrollView scrollView, UIGrid grid)
    {
        float weight = grid.cellWidth;
        Vector3 currentPosition = scrollView.transform.localPosition;
        scrollView.transform.localPosition = new Vector3(currentPosition.x - weight, currentPosition.y, currentPosition.z);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector3(currentPosition.x - weight, currentPosition.y, currentPosition.z);
    }
    /// <summary>
    /// 在Widget上发送射线
    /// </summary>
    /// <param name="rayOrignal"></param>
    /// <param name="camera"></param>
    /// <param name="rayCollider"></param>
    /// <returns></returns>
    public static GameObject RaysEvent(UIWidget rayOrignal, Camera camera, string rayCollider)
    {
        Ray center = camera.ScreenPointToRay(camera.WorldToScreenPoint(rayOrignal.transform.position));
        RaycastHit[] centerContant;
        centerContant = Physics.RaycastAll(center, 20, 1 << 8);
        foreach (var o in centerContant)
            if (o.collider.name == rayCollider)
                return o.collider.gameObject;
        return null;
    }

    /// <summary>
    /// 设置按键四态
    /// </summary>
    /// <param name="button"></param>
    /// <param name="spriteName"></param>
    public static void SetAllSpriteForButton(UIButton button, string spriteName)
    {
        button.normalSprite = spriteName;
        button.pressedSprite = spriteName;
        button.hoverSprite = spriteName;
        button.disabledSprite = spriteName;
    }

    /// <summary>
    /// 查找子节点
    /// </summary>
    public static Transform FindDeepChild(GameObject _target, string _childName)
    {
        Transform resultTrs = null;
        resultTrs = _target.transform.Find(_childName);
        if (resultTrs == null)
        {
            foreach (Transform trs in _target.transform)
            {
                resultTrs = Utility.FindDeepChild(trs.gameObject, _childName);
                if (resultTrs != null)
                    return resultTrs;
            }
        }

        return resultTrs;
    }

    /// <summary>
    /// 查找子节点脚本
    /// </summary>
    public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
    {
        Transform resultTrs = Utility.FindDeepChild(_target, _childName);
        if (resultTrs != null)
            return resultTrs.gameObject.GetComponent<T>();
        return (T)((object)null);
    }


    /// <summary>
    /// 根据最小depth设置目标所有Panel深度，从小到大
    /// </summary>
    /// 
    private class CompareSubPanels : IComparer<UIPanel>
    {
        public int Compare(UIPanel left, UIPanel right)
        {
            return left.depth - right.depth;
        }
    }

    public static void SetTargetMinPanel(GameObject obj, int depth)
    {
        List<UIPanel> lsPanels = GetPanelSorted(obj, true);
        if (lsPanels != null)
        {
            int i = 0;
            while (i < lsPanels.Count)
            {
                lsPanels[i].depth = depth + i;
                i++;
            }
        }
    }

    /// <summary>
    /// 获得指定目标最大depth值
    /// </summary>
    public static int GetMaxTargetDepth(GameObject obj, bool includeInactive = false)
    {
        int minDepth = -1;
        List<UIPanel> lsPanels = GetPanelSorted(obj, includeInactive);
        if (lsPanels != null)
            return lsPanels[lsPanels.Count - 1].depth;
        return minDepth;
    }

    /// <summary>
    /// 返回最大或者最小Depth界面
    /// </summary>
    public static GameObject GetPanelDepthMaxMin(GameObject target, bool maxDepth, bool includeInactive)
    {
        List<UIPanel> lsPanels = GetPanelSorted(target, includeInactive);
        if (lsPanels != null)
        {
            if (maxDepth)
                return lsPanels[lsPanels.Count - 1].gameObject;
            else
                return lsPanels[0].gameObject;
        }
        return null;
    }

    private static List<UIPanel> GetPanelSorted(GameObject target, bool includeInactive = false)
    {
        UIPanel[] panels = target.transform.GetComponentsInChildren<UIPanel>(includeInactive);
        if (panels.Length > 0)
        {
            List<UIPanel> lsPanels = new List<UIPanel>(panels);
            lsPanels.Sort(new CompareSubPanels());
            return lsPanels;
        }
        return null;
    }

    /// <summary>
    /// 添加子节点
    /// </summary>
    public static void AddChildToTarget(Transform target, Transform child)
    {
        child.parent = target;
        child.localScale = Vector3.one;
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;

        ChangeChildLayer(child, target.gameObject.layer);
    }

    /// <summary>
    /// 修改子节点Layer  NGUITools.SetLayer();
    /// </summary>
    public static void ChangeChildLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; ++i)
        {
            Transform child = t.GetChild(i);
            child.gameObject.layer = layer;
            ChangeChildLayer(child, layer);
        }
    }

    /// <summary>
    /// 给目标添加Collider背景
    /// </summary>
    public static void AddColliderBgToTarget(GameObject target, string maskName, UIAtlas altas, bool isTransparent)
    {
        // 添加UIPaneldepth最小上面
        // 保证添加的Collider放置在屏幕中间
        Transform windowBg = Utility.FindDeepChild(target, "Bg");
        if (windowBg == null)
        {
            GameObject targetParent = GetPanelDepthMaxMin(target, false, true);
            if (targetParent == null)
                targetParent = target;

            windowBg = (new GameObject("Bg")).transform;
            AddChildToTarget(targetParent.transform, windowBg);
        }

        Transform bg = Utility.FindDeepChild(target, "WindowColliderBg");
        if (bg == null)
        {
            // add sprite or widget to ColliderBg gameobject
            UIWidget widget = null;
            if (!isTransparent)
                widget = NGUITools.AddSprite(windowBg.gameObject, altas, maskName);
            else
                widget = NGUITools.AddWidget<UIWidget>(windowBg.gameObject);

            widget.name = "WindowColliderBg";
            bg = widget.transform;

            // fill the screen
            UIStretch stretch = bg.gameObject.AddComponent<UIStretch>();
            stretch.style = UIStretch.Style.Both;
            // set relative size bigger
            stretch.relativeSize = new Vector2(1.5f, 1.5f);

            // set a lower depth
            widget.depth = -5;

            // set alpha
            widget.alpha = 0.6f;

            // add collider
            NGUITools.AddWidgetCollider(bg.gameObject);

        }
    }

}
