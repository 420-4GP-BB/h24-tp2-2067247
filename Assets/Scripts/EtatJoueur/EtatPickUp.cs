using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatPickUp : EtatJoueur
{
    public EtatPickUp(ComportementJoueur joueur, GameObject cible) : base(joueur, cible)
    {
    }

    public override void Enter()
    {

        AgentMouvement.enabled = false;
        Controller.enabled = false;
        Animateur.SetTrigger("PickupTrigger");
    }

    public override void Handle()
    {
        if (Animateur.GetCurrentAnimatorStateInfo(0).IsName("PickingUp") &&
        Animateur.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
        {
            Joueur.ChangerEtat(new EtatIdle(Joueur, null));
        }else
        if (Animateur.GetCurrentAnimatorStateInfo(0).IsName("Idle") )
        {
            Joueur.ChangerEtat(new EtatIdle(Joueur, null));
        }


    }

    public override void Leave()
    {
        AgentMouvement.enabled = false;
        Controller.enabled = true;
    }
    public override bool AllowInput()
    {
        return false;
    }
    public override string getName()
    {
        return "PickUp";
    }
}