using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Random = UnityEngine.Random;

public class MouvementPoules : MonoBehaviour
{
    [SerializeField] private Transform[] _pointsdestination;
    [SerializeField] private GameObject gameManager;
    private NavMeshAgent _agent;
    private int _indexPatrouille;
    private Animator _animator;
    private ControlleurJeu controlleurJeu;
    private bool pondre = false;
    private bool pondreOeuf = false;
    private bool decider = false;
    private TimeSpan tempsPonte;
    private int heurePonte;
    private int minPonte;
    // Start is called before the first frame update
    void Start()
    {
        decider = true;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        controlleurJeu = gameManager.GetComponent<ControlleurJeu>();
        _agent.speed = 1;
        DirigerPoule();
       


    }

    // Update is called once per frame
    void Update()
    {
       
        if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
        {
            _animator.SetBool("Walk", false);
            DirigerPoule();
        }
        //décider si la poule va pondre à minuit
        if (controlleurJeu.GetTime().Hours == 00 && controlleurJeu.GetTime().Minutes >= 00 && decider == true)
        {
            decider = false;
            int nombre = Random.Range(0, 2);
            if (nombre == 0)
            {
                pondre = false;

            }
            else
            {
                pondre = true;
            }
        }
        if (controlleurJeu.GetTime().Hours == 08 && controlleurJeu.GetTime().Minutes >= 00)
        {
            decider = true;
            
        }

        if (pondre)
        {
            pondreOeuf = true;
            pondre = false;


            tempsPonte = GenererHeureRandom();
            heurePonte = tempsPonte.Hours;
            minPonte = tempsPonte.Minutes;




        }
        if (controlleurJeu.GetTime().Hours == heurePonte && controlleurJeu.GetTime().Minutes >= minPonte && pondreOeuf)
        {
            controlleurJeu.PondreOeuf(transform.position);
            pondreOeuf = false;
        }
            
         
    }
    /// <summary>
    /// methode pour diriger la poule sur un point au hazard
    /// </summary>
    private void DirigerPoule()
    {

        if (_pointsdestination.Length == 0) return;
        _animator.SetBool("Walk", true);
        // Selectionne un point au hazard
        _indexPatrouille = Random.Range(0, _pointsdestination.Length);
        _agent.destination = _pointsdestination[_indexPatrouille].position;

    }
    /// <summary>
    /// generer un heure de ponte random
    /// </summary>
    /// <returns></returns>
    private TimeSpan GenererHeureRandom()
    {

        // Generer une heure au hazard entre 0 et 23
        int heure = Random.Range(0, 24);

        // Generer une minute au hazard entre 0 et 59
        int minute = Random.Range(0, 60);

        // Creer un timespan pour representer le moment 
        TimeSpan momentPonte = new TimeSpan(heure, minute, 0);
        pondreOeuf = true;
        return momentPonte;

    }
    
}
