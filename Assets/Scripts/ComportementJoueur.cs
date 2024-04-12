using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementJoueur : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;
    private Transform _transform;
    private NavMeshAgent _agent;
    private float vitesse = 7.0f; 
    private float rotationSpeed = 120.0f;
    [SerializeField] private Transform magasin;
    [SerializeField] private Transform maison;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
       
    }


    void Update()
    {
        // rotation du joueur avec les flèches gauche et droite
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        //  mouvement avant et arrière du joueur avec W et S ou les flèches haut et bas
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * vertical;
        _controller.SimpleMove(move * vitesse);

        // Vérifier si le joueur se déplace pour déclencher l'animation de marche
        if (vertical != 0)
        {
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _animator.SetBool("Plant", true);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            _animator.SetBool("Plant", false);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            _animator.SetBool("Pickup", true);
        }
        //code de triche pour teleporter le joueur devant le magasin
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _transform.localPosition = magasin.position;
        }
        //code de triche pour teleporter le joueur devant la ferme
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            _transform.localPosition = maison.position;
            
        }
    }
    public void dirigerAgentJoueur(Vector3 destination)
    {
        _agent.enabled = true;
        _controller.enabled = false;
        _animator.SetBool("Walk", true);
        _agent.destination=destination;
        if(!_agent.pathPending && _agent.remainingDistance < 0.1f)
        {
            _animator.SetBool("Walk",false);
        }
        _controller.enabled = true;
        _agent.enabled = false;
    }
}