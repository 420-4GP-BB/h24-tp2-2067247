using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MouvementPoules : MonoBehaviour
{
    [SerializeField] private Transform[] _pointsdestination;
    private NavMeshAgent _agent;
    private int _indexPatrouille;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        DirigerPoule();


    }

    // Update is called once per frame
    void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _animator.SetBool("Walk", false);
            DirigerPoule();
        }
    }

    private void DirigerPoule()
    {
       
        if (_pointsdestination.Length == 0) return;
        _animator.SetBool("Walk", true);
        // Selectionne un point au hazard
        _indexPatrouille = Random.Range(0, _pointsdestination.Length);
        _agent.destination = _pointsdestination[_indexPatrouille].position;
    }
}
