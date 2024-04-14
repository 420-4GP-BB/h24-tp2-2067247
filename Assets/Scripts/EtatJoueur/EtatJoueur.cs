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
    public EtatJoueur(ComportementJoueur joueur, GameObject cible)
    {
        Joueur = joueur;
        Cible = cible;
        Animateur=joueur.GetComponent<Animator>();
        Controller = joueur.GetComponent<CharacterController>();
        AgentMouvement= joueur.GetComponent<NavMeshAgent>();

    }
  
    
    public virtual bool AllowInput()
    {
        return true;  
    }

    public abstract void Enter();
    public abstract void Handle();
    public abstract void Leave();

}
