using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EtatMarche : EtatJoueur
{
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
        
        string tagCible = Cible.tag;
        if (!AgentMouvement.pathPending && AgentMouvement.remainingDistance < 0.2f)
        {
            Animateur.SetBool("Walk", false);  
            DetermineActionparTag(Cible.tag);

        }
    }


    private void DetermineActionparTag(string tag)
    {
        switch (tag)
        {
            case "Oeuf":
            case "ChouPret":
                Debug.Log("Le mode pick up est activé");
                cibleAtteinte?.Invoke();
                Joueur.ChangerEtat(new EtatPickUp(Joueur, Cible));
                break;
            case "Chou":
                
                cibleAtteinte?.Invoke();
                Joueur.ChangerEtat(new EtatPlant(Joueur, Cible));
                break;
            default:
                Debug.LogError("Error: Tag non-pris en cimpte: " + tag);
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

    public override bool AllowInput()
    {
        return false;
    }
}
