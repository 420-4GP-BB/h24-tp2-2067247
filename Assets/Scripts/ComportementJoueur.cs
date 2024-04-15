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
    private float vitesse = 7.0f;
    private float rotationSpeed = 120.0f;
    [SerializeField] private Transform magasin;
    [SerializeField] private Transform maison;
    [SerializeField] private ControlleurJeu controlleurJeu;

    public bool EnMarche
    {
        private set;
        get;
    }
    public EtatJoueur _etat
    {
        private set;
        get;
    }


    void Start()
    {
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
    /// <summary>
    /// methode pour gerer les inputs si ils sont permis
    /// </summary>
    private void GererInput()
    {
        if (!AutoriserInput()) return;  // empeche le input si l'etat ne permet pas 

        // gerer rotation
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // gerer movement
        float vertical = Input.GetAxis("Vertical");
        float vitesseMouvement;
        if (vertical != 0)
        {
            //verification du pourcentage d'energie 
            if (controlleurJeu.PeutCourir())
            {

                //la vitesse varie dépendamment de l'etat du shif button
                vitesseMouvement = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? vitesse * 2 : vitesse;

            }
            else
            {
                vitesseMouvement = vitesse/2;

            }

            Vector3 move = transform.forward * vertical * vitesseMouvement;
            _controller.SimpleMove(move);
            //J'accede a l'animateur directement pour ne pas causer de confusion avec l'agent nav mesh
            _animator.SetBool("Walk", true);
            EnMarche = true;
        }
        else
        {
            _animator.SetBool("Walk", false);
            EnMarche = false;
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
    /// <summary>
    /// methode pour verifier si l'input est autorise
    /// </summary>
    /// <returns> un boolean</returns>
    private bool AutoriserInput()
    {

        return _etat?.AllowInput() ?? true;
    }
    /// <summary>
    /// methode pour naviguer entre les etats 
    /// </summary>
    /// <param name="nouvelEtat">Nouvel etat que l'on veut acceder</param>
    public void ChangerEtat(EtatJoueur nouvelEtat)
    {
        _etat?.Leave();
        _etat = nouvelEtat;
        _etat.Enter();
    }

}