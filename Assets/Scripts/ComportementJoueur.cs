using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementJoueur : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;
    private Transform _transform;

    public float speed = 30.0f; // Vitesse de déplacement du joueur
    public float rotationSpeed = 120.0f;

    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _transform = GetComponent<Transform>();
        _transform.position = new Vector3(_transform.position.x, 0f, _transform.position.z);
    }


    void Update()
    {
        // rotation du joueur avec les flèches gauche et droite
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        //  mouvement avant et arrière du joueur avec W et S ou les flèches haut et bas
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.forward * vertical;
        _controller.SimpleMove(move * speed);

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
        
    }
}