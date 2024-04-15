using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtatIdle : EtatJoueur
{/// <summary>
/// constructeur de l'etta de base
/// </summary>
/// <param name="joueur"> script du joueur "Mathurin"</param>
/// <param name="cible">Cible visée</param>
    public EtatIdle(ComportementJoueur joueur, GameObject cible) : base(joueur, cible)
    {
    }

    public override void Enter()
    {
        
        Animateur.SetBool("Walk", false);
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
