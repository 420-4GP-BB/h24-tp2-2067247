using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class Controleur : MonoBehaviour
{
    /// <summary>
    /// Le champ de saisie du nom
    /// </summary>
    [SerializeField] private TMP_InputField saisieNom;

    /// <summary>
    /// La liste déroulante des niveaux
    /// </summary>
    [SerializeField] private TMP_Dropdown choixNiveau;

    /// <summary>
    /// Les ressources
    /// </summary>
    [SerializeField] private TMP_Text nombreOr;
    [SerializeField] private TMP_Text nombreOeuf;
    [SerializeField] private TMP_Text nombreGraines;

    public void Start()
    {
       
        choixNiveau.value=Parametres.Instance.Niveau;
      
    }
   

    public void Quitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ChargerJeu()
    {
        ChangerNom();
        ChangerNiveau();
        SceneManager.LoadScene("Ferme");
    }

    public void ChangerNom()
    {
        Parametres.Instance.Nom = saisieNom.text;
    }

    public void ChangerNiveau()
    {if (choixNiveau.value == 0)
        {
            nombreOr.text = "200";
            nombreOeuf.text = "5";
            nombreGraines.text = "5";
        }else if (choixNiveau.value == 1)
        {
            nombreOr.text = "100";
            nombreOeuf.text = "3";
            nombreGraines.text = "2";
        }
        else if (choixNiveau.value == 2)
        {
            nombreOr.text = "50";
            nombreOeuf.text = "0";
            nombreGraines.text = "2";
        }

        Parametres.Instance.Niveau = choixNiveau.value;
    }
}