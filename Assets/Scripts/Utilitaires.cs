using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Utilitaires
{
    /// <summary>
    /// Classe inspirée des notes de cours: prend un tag en paramètre et le compare avce le tag de l'objet cliqué
    /// </summary>
    /// <param name="tag"> prend le tag en paramèrtre</param>
    /// <returns></returns>

    public static GameObject DeterminerClic(string tag)
    {
        Vector3 positionSouris = Input.mousePosition;
   
        GameObject clickedObject=null;

        // Trouver le lien avec la caméra
        Ray ray = Camera.main.ScreenPointToRay(positionSouris);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            // Vérifier quel objet est touché.
            if (hit.collider.tag == tag)
            {
                clickedObject = hit.collider.gameObject;
            }
        }
        return clickedObject;
    }
}

