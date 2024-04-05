using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlleurJeu : MonoBehaviour
{
    [SerializeField] private TMP_Text nomJoueur;
    [SerializeField] private TMP_Text energie;
    [SerializeField] private TMP_Text heure;
    [SerializeField] private TMP_Text jour;
    [SerializeField] private TMP_Text nombreOr;
    [SerializeField] private TMP_Text nombreOeuf;
    [SerializeField] private TMP_Text nombreChoux;
    [SerializeField] private TMP_Text nombreGraines;
    [SerializeField] private Soleil soleil;
    [SerializeField] private GameObject panelMenu;

    private int qtOr;
    private int qtOeuf;
    private int qtChoux = 0;
    private int qtGraines;
    int jour1 = 1;
    private float ValeurEnergie = 100;
    TimeSpan time = new TimeSpan(8, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        panelMenu.SetActive(false);
        energie.text = ValeurEnergie + " %";
        nomJoueur.text = Parametres.Instance.Nom;
        IntialiserInventaire();

    }

    // Update is called once per frame
    void Update()
    {
        ValeurEnergie -= ConstantesJeu.COUT_IMMOBILE*100*soleil.DeltaMinutesEcoulees;
        energie.text = ((int)ValeurEnergie) + " %";
        if (soleil != null)
        {
            time = time.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));
            heure.text = time.ToString(@"hh\:mm");
            if (time.TotalHours >= 24)
            {
                jour1 += 1;
                time = time.Subtract(new TimeSpan(24, 0, 0));
            }
            jour.text = $"Jour {jour1}";

        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Time.timeScale = 45;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Time.timeScale = 0;
            panelMenu.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            Time.timeScale = 1;
            panelMenu.SetActive(false);
        }

    }

    private void IntialiserInventaire()
    {
        nombreChoux.text = qtChoux.ToString();
        if (Parametres.Instance.Niveau == 0)
        {
            qtOr = 200;
            qtOeuf = 5;
            qtGraines = 5;
        }
        else if (Parametres.Instance.Niveau == 1)
        {
            qtOr = 100;
            qtOeuf = 3;
            qtGraines = 2;
        }
        else if (Parametres.Instance.Niveau == 2)
        {
            qtOr = 50;
            qtOeuf = 0;
            qtGraines = 2;
        }

        nombreOr.text = qtOr.ToString();
        nombreOeuf.text = qtOeuf.ToString();
        nombreGraines.text = qtGraines.ToString();

    }
}
