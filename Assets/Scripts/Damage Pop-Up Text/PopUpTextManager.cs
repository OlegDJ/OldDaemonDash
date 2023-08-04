using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextManager : MonoBehaviour
{
    #region Singleton
    public static PopUpTextManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    #endregion

    [SerializeField] private GameObject damagePopUpTextPrefab;
    [SerializeField] private GameObject dodgedPopUpTextPrefab;
    [SerializeField] private GameObject deathPopUpTextPrefab;

    public void DisplayDamagePopUpText(string text, Transform textSpawnPosition)
    {
        GameObject damagePopUpText = Instantiate(damagePopUpTextPrefab, textSpawnPosition.transform.position, Quaternion.identity);
        damagePopUpText.GetComponent<PopUpTextController>().SetUp(text);
    }

    public void DisplayDodgedPopUpText(Transform textSpawnPosition)
    {
        GameObject dodgedPopUpText = Instantiate(dodgedPopUpTextPrefab, textSpawnPosition.transform.position, Quaternion.identity);
        dodgedPopUpText.GetComponent<PopUpTextController>().SetUp("Dodged!");
    }

    public void DisplayDeathPopUpText(Transform textSpawnPosition)
    {
        GameObject deathPopUpText = Instantiate(deathPopUpTextPrefab, textSpawnPosition.transform.position, Quaternion.identity);
        deathPopUpText.GetComponent<PopUpTextController>().SetUp("Dead...");
    }
}
