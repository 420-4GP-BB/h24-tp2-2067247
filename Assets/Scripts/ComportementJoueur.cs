using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementJoueur : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
_animator.SetBool("Walk", true);
        }
        if (Input.GetKeyUp(KeyCode.W))
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
            _animator.SetBool("Walk", false);
            _animator.SetBool("Pickup", true);
        }
    }
}
