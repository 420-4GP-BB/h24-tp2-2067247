using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaisonSujet : MonoBehaviour
{
    public event Action<object, EventArgs> ZoneAtteinteHandler;
    [SerializeField] private GameObject Joueur;

    /// <summary>
    /// methode pour allerter l'observateur
    /// </summary>
    /// <param name="other">objet qui rentre en contact avec la zone</param>

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Joueur)
        {
            ZoneAtteinteHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
