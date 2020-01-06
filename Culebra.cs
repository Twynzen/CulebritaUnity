using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culebra : MonoBehaviour
{
   public GameObject Bloque;
   public GameObject Escenario;
   public int Ancho, Alto;

    //queue es una lista que elmina números de acuerdo al orden de entrada
   private Queue<GameObject> cuerpo = new Queue<GameObject>();
   private GameObject cabeza;
   
   //ira siempre a a la derecha en primera instancia
   private Vector3 direccion = Vector3.right;

   private void Awake(){

       CrearMuros();
       int posicionIncialX = Ancho/2;
       int posicionIncialY = Alto/2;

        //Este bloque instancia nuevos bloques que compondran el cuerpo de la culebra
       for(int c=15; c>0; c--){
           //apareceran los bloques de la última posicion a la cabeza
           NuevoBloque(posicionIncialX-c, posicionIncialY);
       }
       
       cabeza = NuevoBloque(posicionIncialX, posicionIncialY);
       //iniciamos la corrutina
       StartCoroutine(Movimiento());
    
   }


    //Esto es una corrutina
   private IEnumerator Movimiento(){

       //Definimos una vartiable local
       WaitForSeconds espera = new WaitForSeconds(0.15f);
       while (true)
       {
           //calculamos la nueva posicion del objeto donde esta la cola
           Vector3 nuevaPosicion = cabeza.transform.position + direccion;
           /*obtenemos el último elemento o el que más tiempo lleva en la 
           lista de los que componen el cuerpo*/
           GameObject parteCuerpo = cuerpo.Dequeue();
           parteCuerpo.transform.position =nuevaPosicion;

           cuerpo.Enqueue(parteCuerpo);

           cabeza = parteCuerpo;

           yield return espera;
       }

   }

   private GameObject NuevoBloque (float x, float y){

       //Instanciamos  la creacion de los bloques que aparecen como hijos de culebra
       GameObject nuevo = Instantiate(Bloque, new Vector3(x, y), 
       Quaternion.identity, this.transform );
       //Metemos el nuevo bloque con enqueue en cuerpo
       cuerpo.Enqueue(nuevo);
       return nuevo;

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
       
       //Creo que esto detecta la direccion en la que iría la culebra
       float horizontal = Input.GetAxisRaw("Horizontal");
       float vertical = Input.GetAxisRaw("Vertical");
       Vector3 direccionSeleccionada = new Vector3(horizontal, vertical);

       if(direccionSeleccionada != Vector3.zero){

           direccion = direccionSeleccionada;
       }
   }
}
