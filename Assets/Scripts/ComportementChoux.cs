using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementChoux : MonoBehaviour
{
    [SerializeField] private Soleil soleil;
    private TimeSpan time = new TimeSpan(0, 0, 0);
    private GameObject petitChoux;
    private GameObject moyenChoux;
    private GameObject pretChoux;
    // Start is called before the first frame update
    void Start()
    {
        petitChoux = transform.GetChild(0).gameObject;
        moyenChoux = transform.GetChild(1).gameObject;
        pretChoux = transform.GetChild(2).gameObject;

    }

    // Update is called once per frame
    void Update()
    {

        if (petitChoux.activeSelf)
        {
            time = time.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));

            if (time.TotalHours >= 24)
            {
                petitChoux.SetActive(false);
                moyenChoux.SetActive(true);

            }

        }


        if (moyenChoux.activeSelf)
        {
            time = time.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));

            if (time.TotalHours >= 48)
            {
                moyenChoux.SetActive(false);
                pretChoux.SetActive(true);

            }
        }

        if (pretChoux.activeSelf)
        {
            time = new TimeSpan(0, 0, 0);

        }
    }
    public bool verifierPret()
    {
        Debug.Log("Verification du chou : " + pretChoux.activeSelf);
        return pretChoux.activeSelf;


    }
}
