using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    public EtatJoueur _etat
    {
        private set;
        get;
    }
    

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
        ChangerEtat(new EtatIdle(this, null));
        
    }


    void Update()
    {
        _etat.Handle();
        GererInput();
       
       
    }
    void GererInput()
    {
        if (!AutoriserInput()) return;  // Prevent input if the current state disallows it

        // gerer rotation
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // gerer movement
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0)
        {
            float vitesseMouvement = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? vitesse * 2 : vitesse;
            // Player has input for movement, thus move the character
            Vector3 move = transform.forward * vertical * vitesseMouvement;
            _controller.SimpleMove(move);

            
            _animator.SetBool("Walk", true);
            
        }
        else
        {
          
            _animator.SetBool("Walk", false);
        }

        // Cheat codes
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _transform.position = magasin.position;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            _transform.position = maison.position;
        }
    }
   private bool AutoriserInput()
    {
        
        return _etat?.AllowInput() ?? true;
    }

    public void ChangerEtat(EtatJoueur nouvelEtat)
    {
        _etat?.Leave();
        _etat = nouvelEtat;
        _etat.Enter();
    }

   
}