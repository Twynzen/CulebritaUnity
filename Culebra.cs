using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culebra : MonoBehaviour
{
   public GameObject Bloque;
   public GameObject Escenario;
   public int Ancho, Alto;

    //queue es una lista que elmina númeross de acuerdo al orden de entrada
   private Queue<GameObject> cuerpo = new Queue<GameObject>();
   private GameObject cabeza;
   
   private Vector3 direccion = Vector3.right;

   private void Awake(){

       CrearMuros();
       int posicionIncialX = Ancho/2;
       int posicionIncialY = Alto/2;
       NuevoBloque(posicionIncialX, posicionIncialY);

   }

   private void NuevoBloque (float x, float y){

       GameObject nuevo = Instantiate(Bloque, new Vector3(x, y), Quaternion.identity, this.transform);

   }

   private void CrearMuros(){

       for(int x=0; x<Ancho; x++){

           for(int y=0; y<Alto; y++){


            if(x == 0 || x == Ancho-1 || y == 0 || y== Alto-1){ 

                Vector3 posicion = new Vector3(x, y);
                Instantiate(Bloque, posicion, Quaternion.identity, Escenario.transform);
           
             }

           }

       }

   }

   private void Update(){
       
       float horizontal = Input.GetAxisRaw("Horizontal");
       float vertical = Input.GetAxisRaw("Vertical");
       Vector3 direccionSeleccionada = new Vector3(horizontal, vertical);

       if(direccionSeleccionada != Vector3.zero){

           direccion = direccionSeleccionada;
       }
   }
}
