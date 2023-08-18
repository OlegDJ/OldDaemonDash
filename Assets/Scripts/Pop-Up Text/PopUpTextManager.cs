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

    [SerializeField] private GameObject damagePopUpTextPrefab, dodgedPopUpTextPrefab, deathPopUpTextPrefab;

    public void DisplayDamagePopUpText(int damage, Vector3 textSpawnPosition)
    {
        GameObject damagePopUpText = Instantiate(damagePopUpTextPrefab, textSpawnPosition, Quaternion.identity);
        damagePopUpText.GetComponent<PopUpTextController>().SetUp("-" + damage.ToString());
    }

    public void DisplayDodgedPopUpText(Vector3 textSpawnPosition)
    {
        GameObject dodgedPopUpText = Instantiate(dodgedPopUpTextPrefab, textSpawnPosition, Quaternion.identity);
        dodgedPopUpText.GetComponent<PopUpTextController>().SetUp("Dodged!");
    }

    public void DisplayDeathPopUpText(Vector3 textSpawnPosition)
    {
        GameObject deathPopUpText = Instantiate(deathPopUpTextPrefab, textSpawnPosition, Quaternion.identity);
        deathPopUpText.GetComponent<PopUpTextController>().SetUp("Dead...");
    }
}
