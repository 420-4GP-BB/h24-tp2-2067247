using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parametres 
{
    /// <summary>
    /// L'instance statique du singleton
    /// </summary>
    private static Parametres _instance = new Parametres();

    /// <summary>
    /// Propriete pour obtenir l'instance
    /// </summary>
    public static Parametres Instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Le nom du joueur
    /// </summary>
    public string Nom
    {
        set;
        get;
    }

    /// <summary>
    /// Le niveau choisi
    /// </summary>
    public int Niveau
    {
        set;
        get;
    }

    /// <summary>
    /// C'est un singleton. Le constructeur est privé
    /// </summary>
    private Parametres()
    {
        Nom = "";
        Niveau = 0;
    }
}
