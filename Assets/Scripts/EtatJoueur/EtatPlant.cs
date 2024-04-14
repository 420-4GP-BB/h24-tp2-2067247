using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatPlant : EtatJoueur
{
    public EtatPlant(ComportementJoueur joueur, GameObject cible) : base(joueur, cible)
    {
    }

    public override void Enter()
    {

        AgentMouvement.enabled = false;
        Controller.enabled = false;
        Animateur.SetBool("Plant", true);
    }

    public override void Handle()
    {
        if (Animateur.GetCurrentAnimatorStateInfo(0).IsName("Planter") &&
        Animateur.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f)
        {
            Joueur.ChangerEtat(new EtatIdle(Joueur, null));
        }
    }

    public override void Leave()
    {
        AgentMouvement.enabled = false;
        Controller.enabled = true;
        Animateur.SetBool("Plant", false);
    }
    public override bool AllowInput()
    {
        return false;
    }
}
