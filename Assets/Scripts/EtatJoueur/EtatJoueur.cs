using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EtatJoueur 
{
    public ComportementJoueur Joueur
    {
        set;
        get;
    }
    public NavMeshAgent AgentMouvement
    {
        set;
        get;
    }

    public Animator Animateur
    {
        set;
        get;
    }

    public GameObject Cible
    {
        set;
        get;

    }

    public CharacterController Controller
    {
        set;
        get;
    }
    /// <summary>
    /// constructeur
    /// </summary>
    /// <param name="joueur"> script joueur affecté par l'état</param>
    /// <param name="cible"> objet à rammasser ou planter</param>
    public EtatJoueur(ComportementJoueur joueur, GameObject cible)
    {
        Joueur = joueur;
        Cible = cible;
        Animateur=joueur.GetComponent<Animator>();
        Controller = joueur.GetComponent<CharacterController>();
        AgentMouvement= joueur.GetComponent<NavMeshAgent>();

    }
  
    /// <summary>
    ///     retourne 
    /// </summary>
    /// <returns>un bool pour decider si le input est permis</returns>
    public virtual bool AllowInput()
    {
        return true;  
    }
    // retourne le nom de l'etat en cours
    public abstract string getName();
    public abstract void Enter();
    public abstract void Handle();
    public abstract void Leave();

}
