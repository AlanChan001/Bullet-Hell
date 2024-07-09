using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private float timeBetweenStaminaRecover = 3f;

    private Transform staminaContainer;
    public int CurrentStamina { get; private set; }
    public int maxStamina = 3;
    const string Stamina_Container_Text = "Stamina Container";

    private void Start()
    {
        staminaContainer = GameObject.Find(Stamina_Container_Text).transform;
    }

    protected override void Awake()
    {
        base.Awake();
        CurrentStamina = maxStamina;
    }

    public void UseStamina()
    {
        CurrentStamina--;
        UpdateStaminaImages();
        StopAllCoroutines();
        StartCoroutine(recoverStaminaRoutine());
    }

    public void RecoverStamina()
    {
        if (CurrentStamina >= maxStamina && PlayerHealth.Instance.IsDead) { return; }
        CurrentStamina++;
        UpdateStaminaImages();
    }

    public void ReplenishStamina()
    {
        CurrentStamina = maxStamina;
        UpdateStaminaImages();
    }

    private IEnumerator recoverStaminaRoutine()
    {
        while (true)
        { 
            yield return new WaitForSeconds(timeBetweenStaminaRecover);
            RecoverStamina();
        }
        
    }

    private void UpdateStaminaImages()
    {
        for (int i = 0; i < maxStamina; i++)
        {
            if (i + 1 <= CurrentStamina)
            {
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
            }
            else
            { 
                staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
            }
        }
    }
}
