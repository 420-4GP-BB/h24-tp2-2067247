using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatIdle : EtatJoueur
{
    public EtatIdle(ComportementJoueur joueur, GameObject cible) : base(joueur, cible)
    {
    }

    public override void Enter()
    {
        
        Animateur.SetBool("Walk", false);
        Animateur.SetBool("Pickup", false);
        Animateur.SetBool("Plant", false); 

       AgentMouvement.enabled = false;
        Controller.enabled = true;  
    }

    public override void Handle()
    {
    }

    public override void Leave()
    {
        
    }

    public override bool AllowInput()
    {
        
        return true;  
    }

    public override string getName()
    {
        return "Idle";
    }
}
