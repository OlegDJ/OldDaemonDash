using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [SerializeField, Range(0f, 1f)] private float slowedTimeScale = 0.2f;
    [SerializeField] private float changeSpeed = 0.1f;
    private float targetTimeScale;

    private float GetDeltaTime() { return 1.0f / Time.unscaledDeltaTime * Time.deltaTime; }

    private void Start() { targetTimeScale = 1f; }

    private void Update()
    {
        if (Time.timeScale != targetTimeScale)
        {
            Time.timeScale = Mathf.Clamp01(
                Mathf.MoveTowards(Time.timeScale, targetTimeScale, changeSpeed * GetDeltaTime()));
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    public void SlowDownTime() { targetTimeScale = slowedTimeScale; }

    public void SpeedUpTime() { targetTimeScale = 1f; }
}
