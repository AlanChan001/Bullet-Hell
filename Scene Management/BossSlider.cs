using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSlider : Singleton<BossSlider>
{
    [SerializeField] GameObject healthSlider;

    // Start is called before the first frame update
    private void Start()
    {
        healthSlider.SetActive(false);
    }

    public void ActivateSlider()
    {
        if (SceneManager.GetActiveScene().name == "Boss_GiantStorm")
        {
            healthSlider.SetActive(true);
        }
    }

}
