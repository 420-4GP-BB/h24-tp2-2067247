using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private TMP_Text MsgBienvenue;
    [SerializeField] private Soleil soleil;
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelDormir;
    [SerializeField] private GameObject panelJeuPerdu;
    [SerializeField] private GameObject panelMenuMaison;
    [SerializeField] private GameObject panelApresOeuf;
    [SerializeField] private GameObject panelApresChoux;
    [SerializeField] private GameObject Poule;
    [SerializeField] private GameObject BoutonAcheterOeufs;
    [SerializeField] private GameObject BoutonAcheterPoule;
    [SerializeField] private GameObject BoutonAcheterGraines;
    [SerializeField] private GameObject BoutonVendreChoux;
    [SerializeField] private GameObject BoutonManger;
    [SerializeField] private MagasinSujet EntreeMagasin;
    [SerializeField] private MaisonSujet EntreeMaison;
    [SerializeField] public GameObject Oeuf;
    [SerializeField] private GameObject Joueur;


    private bool menuActif = false;
    private bool aManger = false;
    private int qtOr;
    private int qtOeuf;
    private int qtChoux = 0;
    private int qtGraines;
    private ComportementJoueur comportementJoueur;
    int jour1 = 1;
    private float ValeurEnergie = 100;
    private TimeSpan periodeManger = new TimeSpan(0, 0, 0);
    private TimeSpan time = new TimeSpan(8, 0, 0);
    private bool endormi = false;
    private TimeSpan tempsDormir = new TimeSpan(0, 0, 0);
    private const int TEMPS_SOMMEIL_NECCESSAIRE = 10;
    private bool immortel = false;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        panelJeuPerdu.SetActive(false);
        panelDormir.SetActive(false);
        panelApresChoux.SetActive(false);
        panelApresOeuf.SetActive(false);
        Poule.SetActive(false);
        Oeuf.SetActive(false);
        panelMenu.SetActive(false);
        panelMenuMaison.SetActive(false);
        energie.text = ValeurEnergie + " %";
        nomJoueur.text = Parametres.Instance.Nom;
        MsgBienvenue.text = $"Bonjour {Parametres.Instance.Nom}, que puis-je faire pour toi aujourd'hui ?";
        comportementJoueur = Joueur.GetComponent<ComportementJoueur>();
        IntialiserInventaire();
        EntreeMagasin.ZoneAtteinteHandler += ActiverMenu;
        EntreeMaison.ZoneAtteinteHandler += ActiverMenu;


    }

    // Update is called once per frame
    void Update()
    {//update de l amethode dormir si le joueur dort deja 
        if (endormi)
        {
            Dormir();
        }
        //mise à jour de l,nergie
        UpdateEnergie();
        //affichage du temps et prise en note de si le joueur a mangé dans les 12 dernieres heures
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
            periodeManger = periodeManger.Add(TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees));
            if (periodeManger.Hours > 12)
            {
                aManger = false;
                periodeManger = new TimeSpan(0, 0, 0);
            }
        }
        //acceleration avec tab
        if (!menuActif)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Time.timeScale = 45;
            }
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                Time.timeScale = 1;
            }
        }

        Tricher();


        //Les boutons sont cliquables seulement si le joueur a assez de ressources.
        BoutonAcheterOeufs.GetComponent<Button>().interactable = qtOr >= 25;
        BoutonAcheterPoule.GetComponent<Button>().interactable = qtOr >= 100;
        BoutonAcheterGraines.GetComponent<Button>().interactable = qtOr >= 3;
        BoutonVendreChoux.GetComponent<Button>().interactable = qtChoux >= 1;
        BoutonManger.GetComponent<Button>().interactable = VerifierManger();

        // logique pour planter/ rammasser oeufs et choux quand l'utilisateur click sur un ibject
        if (Input.GetMouseButtonDown(0))
        {

            GameObject oeuf = Utilitaires.DeterminerClic("Oeuf");
            GameObject chouVide = Utilitaires.DeterminerClic("Chou");
            GameObject ChouPret = Utilitaires.DeterminerClic("ChouPret");
            //seulement l'animation pour ramasser des oeufs fonctionne

            if (oeuf != null)
            {
                comportementJoueur.ChangerEtat(new EtatMarche(comportementJoueur, oeuf, () =>
                {
                    AquerirUnOeuf();
                    oeuf.SetActive(false);
                }));

            }
            if (chouVide != null && VerifierChouVide(chouVide) && qtGraines > 0)
            {
                comportementJoueur.ChangerEtat(new EtatMarche(comportementJoueur, chouVide, () =>
                {
                    GameObject child = chouVide.transform.Find("Petit").gameObject;
                    PlanterUnChoux();
                    child.SetActive(true);
                }));
            }
            if (ChouPret != null)
            {
                comportementJoueur.ChangerEtat(new EtatMarche(comportementJoueur, ChouPret, () =>
                {
                    GameObject parentObject = ChouPret.transform.parent.gameObject;
                    parentObject.SetActive(false);
                    CueillirUnChoux();
                }));

            }
        }
        // gestion du game over
        if (ValeurEnergie < 1)
        {
            Time.timeScale = 0;
            panelJeuPerdu.SetActive(true);
        }
        // gestion de l acouleur de l'energie
        if (ValeurEnergie < 21)
        {
            energie.color = Color.red;
        }
        if (ValeurEnergie > 20)
        {
            energie.color = Color.white;
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
        menuActif = false;
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
    /// Methode pour actualiser Valeur et l'energie lorsqu'on mange un oeuf
    /// </summary>
    private void MangerUnOeuf()
    {
        ValeurEnergie += ConstantesJeu.GAIN_ENERGIE_MANGER_OEUF * 100;
        if (ValeurEnergie > 100)
        {
            ValeurEnergie = 100;
        }

        qtOeuf -= 1;
        nombreOeuf.text = qtOeuf.ToString();
    }
    /// <summary>
    /// Methode pour actualiser Valeur lorsqu'on cueille un chou
    /// </summary>
    private void CueillirUnChoux()
    {
        qtChoux += 1;
        nombreChoux.text = qtChoux.ToString();
    }

    /// <summary>
    /// Methode pour actualiser Valeur et l'energie lorsqu'on mange un chou
    /// </summary>
    private void MangerUnChoux()
    {
        ValeurEnergie += ConstantesJeu.GAIN_ENERGIE_MANGER_CHOU * 100;
        if (ValeurEnergie > 100)
        {
            ValeurEnergie = 100;
        }
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
    /// mettre à jour l'energie en utilisant CalculerEnergie(int Facteur) et depedemment de si c'est la nuit ou le jour pour le facteur
    /// </summary>
    private void UpdateEnergie()
    {
        
        if (soleil.EstNuit)
        {
            float energieCout = CalculerEnergie(2);
            ValeurEnergie -= energieCout;
            energie.text = ((int)ValeurEnergie) + " %";
            corrigerEnergie();
        }
        else
        {
            float energieCout = CalculerEnergie(1);
            ValeurEnergie -= energieCout;
            energie.text = ((int)ValeurEnergie) + " %";
            corrigerEnergie();
        }

       
    }


    /// <summary>
    /// fonction pout calculer l'energie
    /// </summary>
    /// <param name="Facteur"> utile pour quand c'est la nuit et que je joueur perd deux fois plus dénergie</param>
    /// <returns></returns>
    private float CalculerEnergie(int Facteur)
    {
        if (!immortel)
        {


            if (endormi)
            {
                return CalculEnergieSommeil();
            }
            else if(comportementJoueur.EnMarche)
            {
                return ConstantesJeu.COUT_MARCHER * (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) ? 200 : 100) * Facteur * soleil.DeltaMinutesEcoulees;
            }
            else
            {
                switch (comportementJoueur._etat.getName())
                {
                    case "Marche":
                        return ConstantesJeu.COUT_MARCHER * (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) ? 200 : 100) * Facteur * soleil.DeltaMinutesEcoulees;
                    case "PickUp":
                        return ConstantesJeu.COUT_CUEILLIR * 100 * Facteur * soleil.DeltaMinutesEcoulees;
                    case "Plante":
                        return ConstantesJeu.COUT_PLANTER * 100 * Facteur * soleil.DeltaMinutesEcoulees;
                    default:
                        return ConstantesJeu.COUT_IMMOBILE * 100 * Facteur * soleil.DeltaMinutesEcoulees;
                }
            }

        }
        else
        {// si immortel, il ne pperd pas d'énergie
            return 0f;
        }
    }
   
    /// <summary>
    /// calcul de l'energie acquise ou perdu durant le sommeil dépendemment de si le joueur a mangé
    /// </summary>
    /// <returns></returns>
    private float CalculEnergieSommeil()
    {
        if (aManger)
        {
            return ConstantesJeu.GAIN_ENERGIE_SOMMEIL * 100 * soleil.DeltaMinutesEcoulees;
        }
        else
        {
            return ConstantesJeu.COUT_IMMOBILE * 100 * soleil.DeltaMinutesEcoulees;
        }
    }

    /// <summary>
    /// Activer le menu du magasin
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ActiverMenu(object sender, EventArgs e)
    {
        if (sender == EntreeMagasin)
        {
            Time.timeScale = 0;
            panelMenu.SetActive(true);



        }
        if (sender == EntreeMaison)
        {
            Time.timeScale = 0;
            panelMenuMaison.SetActive(true);



        }
        menuActif = true;
    }
    /// <summary>
    /// donne le temps actuel pour les autres classes
    /// </summary>
    /// <returns> return the temps de heu</returns>
    public TimeSpan GetTime()
    {
        return time;
    }
    /// <summary>
    ///  verifier si les choux sont "inactifs", pour ne pas planter plus que un chou par endroit 
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

    //verification de si le joueur a de la nourriture à manger
    private bool VerifierManger()
    {
        if (qtOeuf > 0 || qtChoux > 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// logique pour manger oeuf ou chou
    /// </summary>
    public void Manger()
    {
        if (qtOeuf > 0)
        {
            MangerUnOeuf();
            panelApresOeuf.SetActive(true);
        }
        else if (qtChoux > 0)
        {
            MangerUnChoux();
            panelApresChoux.SetActive(true);
        }
        menuActif = true;

        periodeManger = new TimeSpan(0, 0, 0);
        aManger = true;
    }
    //bouton pour sortir du menu maison
    public void SortirMaison()
    {
        Time.timeScale = 1;
        panelMenuMaison.SetActive(false);
        menuActif = false;
    }
    //bounton okay apres avoir manger
    public void okayManger()
    {

        if (panelApresOeuf.activeSelf)
        {
            panelApresOeuf.SetActive(false);
        }
        else
        {
            panelApresChoux.SetActive(false);
        }



    }
    //retourner au menu 
    public void SortirDuJeu()
    {
        SceneManager.LoadScene("Menu");
       
    }
    /// <summary>
    /// fonction quand on click le bouton dormir
    /// </summary>
    public void Dormir()
    {
        panelDormir.SetActive(true);
        endormi = true;
        Time.timeScale = 90;
        tempsDormir += TimeSpan.FromMinutes(soleil.DeltaMinutesEcoulees);
        if (tempsDormir.TotalHours >= TEMPS_SOMMEIL_NECCESSAIRE)
        {
           
            Time.timeScale = 1;
            endormi = false;
            tempsDormir = new TimeSpan(0, 0, 0);
            panelDormir.SetActive(false);
            SortirMaison();
        }
    }

    /// <summary>
    /// Pour garder l'energie en dessous de 100;
    /// </summary>
    public void corrigerEnergie() {
        if (ValeurEnergie > 100)
        {
            ValeurEnergie = 100;
            energie.text = ((int)ValeurEnergie) + " %";
        }
        
    }
    /// <summary>
    /// codes de triche
    /// </summary>

    public void Tricher()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            qtOeuf += 100;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            qtOr += 100;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            qtChoux += 10;
           
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            qtGraines += 10;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            qtOeuf = 0;
            qtOr = 0;
            qtChoux = 0;
            qtGraines = 0;

        }

        nombreOr.text = qtOr.ToString();
        nombreOeuf.text = qtOeuf.ToString();
        nombreGraines.text = qtGraines.ToString();
        nombreChoux.text = qtChoux.ToString();

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (immortel)
            {
                immortel = false;
            }
            else
            {
                immortel = true;
                ValeurEnergie = 100;
               energie.text= ValeurEnergie + " %";
            }
           
        }

        
    }
    /// <summary>
    /// fonction pour determiner si l'energie est suffisante pour pouvoir courrir
    /// </summary>
    /// <returns></returns>
    public bool PeutCourir()
    {
        if (ValeurEnergie > 20)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}
