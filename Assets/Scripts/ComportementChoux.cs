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
        //si le choux est petit
        if (petitChoux.activeSelf)
        {
            time = time.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));

            if (time.TotalHours >= 24)
            {
                petitChoux.SetActive(false);
                moyenChoux.SetActive(true);

            }

        }
        //si le chou est moyen

        if (moyenChoux.activeSelf)
        {
            time = time.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));

            if (time.TotalHours >= 72)
            {
                moyenChoux.SetActive(false);
                pretChoux.SetActive(true);

            }
        }
        //reinitialisation du temps à zero
        if (pretChoux.activeSelf)
        {
            time = new TimeSpan(0, 0, 0);

        }
    }
    /// <summary>
    /// retourne si le choux et pret à etre cueilli.
    /// </summary>
    /// <returns></returns>
    public bool verifierPret()
    {
   
        return pretChoux.activeSelf;

    }
}
