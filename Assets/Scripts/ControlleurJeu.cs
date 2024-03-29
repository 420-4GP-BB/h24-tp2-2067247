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
    
    private int qtOr;
    private int qtOeuf;
    private int qtChoux=0;
    private int qtGraines;
    float deltaMinutes = 0;
    int heure1 = 8;
    int jour1 = 1;
    // Start is called before the first frame update
    void Start()
    {
        nomJoueur.text = Parametres.Instance.Nom;
        IntialiserInventaire();
    }

    // Update is called once per frame
    void Update()
    {
        if (soleil != null)
        {
            deltaMinutes += soleil.DeltaMinutesEcoulees;
          //  Debug.Log($"Delta Minutes Ecoulees: {deltaMinutes}");
            if (deltaMinutes >= 59)
            {
                heure1 += 1;
                deltaMinutes =0;
            }
            if(heure1>=23 && deltaMinutes >= 59)
            {
                jour1 += 1;
                heure1 = 0;
                deltaMinutes = 0;
            }
            jour.text = $"Jour {jour1}";
            heure.text = $"{heure1}:{((int)deltaMinutes)}";
            Debug.Log($"Heures Écoulées : {heure1} Minutes Ecoulees: {deltaMinutes}");
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
