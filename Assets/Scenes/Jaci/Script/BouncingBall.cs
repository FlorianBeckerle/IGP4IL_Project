using UnityEngine;


//Auf 3 Bällen drauf
public class BouncingBall: MonoBehaviour
{

  public bool isMoving = false;
  public bool isInTarget = false;
  
  private void OnTriggerEnter(Collider other) // Kontrolliert ob Ball im Cylinder ist
  {
      if (other.CompareTag("Target"))
      {
          isInTarget = true;    
  
      }
  
  }

  private void OnTriggerExit(Collider other) // Kontrolliert ob Ball nicht im Cylinder ist
  {
      if (other.CompareTag("Target"))
      {
          isInTarget = false;    
      }
  }
}