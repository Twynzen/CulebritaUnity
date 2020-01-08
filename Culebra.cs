using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

   //creamos una casilla que permita  definit si un tipo de dato esta vacio o con un obstaculo
   private enum TipoCasilla{

       Vacio, Obstaculo
   }

    //Creamos un array bidimincional separando con coma 
   private TipoCasilla[,] mapa;

   private void Awake(){

       //creamos el array que tendrá las posiciones que queremos darle colisión
       mapa = new TipoCasilla[Ancho, Alto]; 
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
    //obtenemos la casilla de mapa que colisione
   private TipoCasilla ObtenerMapa(Vector3 posicion){

       return mapa[Mathf.RoundToInt(posicion.x), Mathf.RoundToInt(posicion.y)];
   }

   private void EstablecerMapa(Vector3 posicion, TipoCasilla valor){

       mapa[Mathf.RoundToInt(posicion.x), Mathf.RoundToInt(posicion.y)] = valor;

   }


    //Esto es una corrutina
   private IEnumerator Movimiento(){

       //Definimos una vartiable local
       WaitForSeconds espera = new WaitForSeconds(0.15f);
       while (true)
       {
           //calculamos la nueva posicion del objeto donde esta la cola
           Vector3 nuevaPosicion = cabeza.transform.position + direccion;
        
           //esta variable tipocasilla revisa si esta vacia o con obstaculo 
           TipoCasilla casillaAOcupar = ObtenerMapa(nuevaPosicion);
           if (casillaAOcupar == TipoCasilla.Obstaculo)
           {
               Debug.Log("Muerto!");
               yield return new WaitForSeconds (5);
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
               yield break;
           }
           else
           {
           /*obtenemos el último elemento o el que más tiempo lleva en la 
           lista de los que componen el cuerpo*/
           GameObject parteCuerpo = cuerpo.Dequeue();
           //establecemos la posición antes de moverse
           EstablecerMapa(parteCuerpo.transform.position, TipoCasilla.Vacio);
           parteCuerpo.transform.position =nuevaPosicion;
           //aquí ponemos pared ya con el obstaculo al haberse movido
           EstablecerMapa(nuevaPosicion, TipoCasilla.Obstaculo);

           cuerpo.Enqueue(parteCuerpo);

           cabeza = parteCuerpo;

           yield return espera;
           }


          
       }

   }

   private GameObject NuevoBloque (float x, float y){

       //Instanciamos  la creacion de los bloques que aparecen como hijos de culebra
       GameObject nuevo = Instantiate(Bloque, new Vector3(x, y), 
       Quaternion.identity, this.transform );
       //Metemos el nuevo bloque con enqueue en cuerpo
       cuerpo.Enqueue(nuevo);
       //
       EstablecerMapa(nuevo.transform.position, TipoCasilla.Obstaculo);
       return nuevo;

   }

   private void CrearMuros(){

       for(int x=0; x<Ancho; x++){

           for(int y=0; y<Alto; y++){


            if(x == 0 || x == Ancho-1 || y == 0 || y== Alto-1){ 

                Vector3 posicion = new Vector3(x, y);
                Instantiate(Bloque, posicion, Quaternion.identity, Escenario.transform);
                //insertamos la creación de mapa colisionable
                EstablecerMapa(posicion, TipoCasilla.Obstaculo);
           
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
