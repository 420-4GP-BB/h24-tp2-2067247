using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

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
    [SerializeField] private GameObject Poule;
    [SerializeField] private GameObject BoutonAcheterOeufs;
    [SerializeField] private GameObject BoutonAcheterPoule;
    [SerializeField] private GameObject BoutonAcheterGraines;
    [SerializeField] private GameObject BoutonVendreChoux;
    [SerializeField] private MagasinSujet EntreeMagasin;
    [SerializeField] public GameObject Oeuf;
    [SerializeField] private GameObject Joueur;


   
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
        
        Poule.SetActive(false);
        Oeuf.SetActive(false);
        panelMenu.SetActive(false);
        energie.text = ValeurEnergie + " %";
        nomJoueur.text = Parametres.Instance.Nom;
        IntialiserInventaire();
        EntreeMagasin.ZoneAtteinteHandler += ActiverMenu;

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

      

        //Les boutons sont cliquables seulement si le joueur a assez de ressources.
        BoutonAcheterOeufs.GetComponent<Button>().interactable = qtOr >= 25;
        BoutonAcheterPoule.GetComponent<Button>().interactable = qtOr >= 100;
        BoutonAcheterGraines.GetComponent<Button>().interactable = qtOr >= 3;
        BoutonVendreChoux.GetComponent<Button>().interactable = qtChoux >= 1;


        if (Input.GetMouseButtonDown(0))
        {
           
            GameObject objet =  Utilitaires.DeterminerClic("Oeuf");
            GameObject chouVide = Utilitaires.DeterminerClic("Chou");
            GameObject ChouPret = Utilitaires.DeterminerClic("ChouPret");


            if (objet != null)
            {
      
               AquerirUnOeuf();
               objet.SetActive(false);

              
            }
            if (chouVide != null)
            {
                if(VerifierChouVide(chouVide) && qtGraines > 0)

                {
                    GameObject child = chouVide.transform.Find("Petit").gameObject;
                    child.SetActive(true);
                    PlanterUnChoux();
                }

                
            }
            if (ChouPret!= null)
            {
                Debug.Log("le chou EST PRET");
                
                GameObject parentObject = ChouPret.transform.parent.gameObject;
                parentObject.SetActive(false);
                CueillirUnChoux();
            }
        }
            
            
            

        }
    /// <summary>
    /// Achat de poule
    /// </summary>
    public void AcheterPoulet()
    {
        effectuerPaiement(100);
        if (Poule.activeSelf)
        {
            GameObject.Instantiate(Poule);
        }
        else { Poule.SetActive(true); }

    }
    //ponte des oeuf
    public void PondreOeuf(Vector3 position)
    {
       
        if (Oeuf.activeSelf)
        {
            GameObject.Instantiate(Oeuf);
        }
        else { Oeuf.SetActive(true); }
        Oeuf.transform.position = position;

    }
    /// <summary>
    /// Achat d'oeuf
    /// </summary>
    public void AcheterOeuf()
    {
        effectuerPaiement(25);
        AquerirUnOeuf();


    }
    /// <summary>
    /// Achat de graines
    /// </summary>
    public void AcheterGraines()
    {
        effectuerPaiement(3);
        qtGraines += 1;
        nombreGraines.text = qtGraines.ToString();
    }
    /// <summary>
    /// sortie du menu
    /// </summary>
    public void sortirMenu()
    {
        Time.timeScale = 1;
        panelMenu.SetActive(false);
    }
    /// <summary>
    /// methode pour initialiser l'inventaire dapres les valeur stockées dans le singleton
    /// </summary>
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
    /// <summary>
    /// Methode pour actualiser Valeur lorsqu'on effectue un paiement
    /// </summary>
    private void effectuerPaiement(int montant)
    {
        qtOr -= montant;
        nombreOr.text = qtOr.ToString();
    }
    /// <summary>
    /// Methode pour actualiser Valeur lorsqu'on rammasse un oeuf
    /// </summary>
    private void AquerirUnOeuf()
    {
        qtOeuf += 1;
        nombreOeuf.text = qtOeuf.ToString();
    }
    /// <summary>
    /// Methode pour actualiser Valeur lorsqu'on cueille un chou
    /// </summary>
    private void CueillirUnChoux()
    {
        qtChoux -= 1;
        nombreChoux.text = qtChoux.ToString();
    }
    /// <summary>
    /// Methode pour actualiser Valeur lorsqu'on plante un chou
    /// </summary>
    private void PlanterUnChoux()
    {

        qtGraines -= 1;
        nombreGraines.text = qtGraines.ToString();
    }
    /// <summary>
    /// Activer le menu du magasin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ActiverMenu(object sender, EventArgs e)
    {
        if (sender == EntreeMagasin )
        {
            Time.timeScale = 0;
            panelMenu.SetActive(true);



        }
    }
    /// <summary>
    /// donne le temps actuel pour les autres classes
    /// </summary>
    /// <returns> return the temps de heu</returns>
    public  TimeSpan GetTime()
    {
        return time;
    }
    /// <summary>
    /// Pour verifier que les choux sont "inactifs", pour ne pas planter plus que un chou par endroit 
    /// </summary>
    /// <param name="chou">prend un gameobject chou</param>
    /// <returns></returns>
    public bool VerifierChouVide(GameObject chou)
    {
        foreach (Transform child in chou.transform)
        {
            if (child.gameObject.activeSelf)  // Verifie si le child est actif
            {
                // si oui return false
                return false;
            }
        }
        // si non retourne true
        return true;
    }

    

}
