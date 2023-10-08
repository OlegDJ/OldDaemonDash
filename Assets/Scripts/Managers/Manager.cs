using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TimeManager))]
[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(NavMeshManager))]
[RequireComponent(typeof(PostProcessingManager))]
[RequireComponent(typeof(PopUpTextManager))]
public class Manager : MonoBehaviour
{
    //#region Singleton
    //public static PopUpTextManager instance;

    //private void Awake()
    //{
    //    if (instance == null) instance = this;
    //    else Destroy(gameObject);
    //}
    //#endregion

    public static Manager mngr;

    [HideInInspector] public GameObject player;

    [HideInInspector] public GameObject mngrObj;
    [HideInInspector] public TimeManager time;
    [HideInInspector] public UIManager ui;
    [HideInInspector] public NavMeshManager navMesh;
    [HideInInspector] public PostProcessingManager postProc;
    [HideInInspector] public PopUpTextManager popUpTxt;
    
    public float deltaTimeMultiplier = 62.5f;

    public float GetDeltaTime()
    {
        return Time.deltaTime * deltaTimeMultiplier;
    }

    public float GetUnscaledDeltaTime()
    {
        return Time.unscaledDeltaTime * deltaTimeMultiplier;
    }

    public float GetFixedDeltaTime()
    {
        return Time.fixedDeltaTime * deltaTimeMultiplier;
    }

    public float GetFixedUnscaledDeltaTime()
    {
        return Time.fixedUnscaledDeltaTime * deltaTimeMultiplier;
    }

    private void Awake()
    {
        if (mngr != null) Destroy(mngr);
        else mngr = this;

        player = GameObject.FindGameObjectWithTag("Player");

        mngrObj = gameObject;
        time = mngrObj.GetComponent<TimeManager>();
        navMesh = mngrObj.GetComponent<NavMeshManager>();
        postProc = mngrObj.GetComponent<PostProcessingManager>();
        popUpTxt = mngrObj.GetComponent<PopUpTextManager>();
        ui = mngrObj.GetComponent<UIManager>();

        navMesh.GenerateNavMeshSurface();
    }

    private void Update()
    {
        postProc.UpdateFunction();
        time.UpdateFunction();
    }
}
