using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicaPersonaje1 : MonoBehaviour
{

    public float velocidadMovimiento = 5.0f;
    private Animator anim;
    public float x,y; //input
    public Rigidbody rb; //el cuerpo afectado por las fisicas
    public CapsuleCollider capsuleCollider;
    public float fuerzaSalto;
    public bool grounded = false;
    public bool doubleJump = false;

    public Text debug;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        fuerzaSalto = 200.0f;
    }

    void Awake() {
        capsuleCollider = transform.GetComponent<CapsuleCollider> ();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        

        anim.SetFloat("VelX",x);
        anim.SetFloat("VelY",y);

        groundCheck();
        if (grounded){
            doubleJump = true;
            if (Input.GetButtonDown("Jump")){
                anim.SetBool("salte",true);
                rb.AddForce(new Vector3(0,fuerzaSalto,0),ForceMode.Impulse);
                //otro metodo para hacer el salto
                //rb.velocity += (Vector3.up * fuerzaSalto);  //este metodo funcionaria mejor para el doble salto creo yo que seria como el de genji
            }
        } else if(doubleJump && Input.GetButtonDown("Jump")){
            rb.AddForce(new Vector3(0,fuerzaSalto,0),ForceMode.Impulse);
            doubleJump = false;
        }
      
    }

    void FixedUpdate() {
        //assuming we only using the single camera:
        var camera = Camera.main;
 
        //camera forward and right vectors:
        var forward = camera.transform.forward;
        var right = camera.transform.right;
 
        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
 
        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * y + right * x;
 
        //now we can apply the movement:
        transform.Translate(desiredMoveDirection * velocidadMovimiento * Time.deltaTime,Space.World);
        if (desiredMoveDirection != Vector3.zero){
            transform.forward = desiredMoveDirection;

            //para hacer que no sea tran brusco el cambio de direccion (comentar la linea de encima):
            //Quaternion toRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, velocidadRotacion * Time.deltaTime);

        }
    }

    void groundCheck(){
        if (Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f)){
            debug.text = "Grounded";
            grounded = true;
            anim.SetBool("grounded",true);
            anim.SetBool("salte",false);
        }
        else{
            debug.text = "Not Grounded";
            grounded = false;
            anim.SetBool("grounded",false);
            //anim.SetBool("salte",false);
        }
    }
}
