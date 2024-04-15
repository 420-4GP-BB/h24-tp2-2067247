using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EtatMarche : EtatJoueur
{/// <summary>
/// Action pour determiner si la cible est atteinte, J,ai utilisé chat gpt pour cet element ici et dans DetermineActionparTag
/// </summary>
    private Action cibleAtteinte;
    public EtatMarche(ComportementJoueur joueur, GameObject cible, Action atteinte) : base(joueur, cible)
    {
        cibleAtteinte = atteinte;
    }

    public override void Enter()
    {
        AgentMouvement.enabled = true;
        Controller.enabled = false;
        Animateur.SetBool("Walk", true);
        AgentMouvement.destination=Cible.transform.position;
        
    }

    public override void Handle()
    {

        float Proximite = DeterminerProximite(Cible.tag);
        if (!AgentMouvement.pathPending && AgentMouvement.remainingDistance < Proximite)
        {
            Animateur.SetBool("Walk", false);  
            DetermineActionparTag(Cible.tag);
        }
    }

    /// <summary>
    /// Déterminer l'action d'après le tag de l'objet à proximité
    /// </summary>
    /// <param name="tag"></param>
    private void DetermineActionparTag(string tag)
    {
        switch (tag)
        {
            case "Oeuf":
            case "ChouPret":
                cibleAtteinte?.Invoke();
                Joueur.ChangerEtat(new EtatPickUp(Joueur, Cible));
                break;
            case "Chou":
                
                cibleAtteinte?.Invoke();
                Joueur.ChangerEtat(new EtatPlant(Joueur, Cible));
                break;
            default:
                Joueur.ChangerEtat(new EtatIdle(Joueur, null));  
                break;
        }
    }
    public override void Leave()
    {
        Animateur.SetBool("Walk", false);
        AgentMouvement.enabled = false;
        Controller.enabled = true;
        Joueur.transform.rotation= Quaternion.Euler(0,Joueur.transform.rotation.eulerAngles.y, 0);
    }
    /// <summary>
    /// Détermine la proximité d'apres la nature de l'objet
    /// </summary>
    /// <param name="tag"> tag de l'objet</param>
    /// <returns></returns>
    private float DeterminerProximite(string tag)
    {
        switch (tag)
        {
            case "Oeuf":
                return 0.2f;
            case "ChouPret":
                return 1.5f;
            case "Chou":
                return 0.2f; 
            default:
                return 0.2f; 
        }
    }
    /// <summary>
    ///  
    /// </summary>
    /// <returns>un bool pour decider si le input est permis</returns>
    public override bool AllowInput()
    {
        return false;
    }

    public override string getName()
    {
        return "Marche";
    }
}
