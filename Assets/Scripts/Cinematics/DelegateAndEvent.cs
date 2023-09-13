using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  public class DelegateAndEvent : MonoBehaviour
{
    public delegate void ConsoleHandler();
    public event ConsoleHandler testEvent;

    public void Name()
    {
        print("Berkay");
    }

    public void Surname()
    {
        print("Guneri");
    }

    private void Start()
    {
        /*ConsoleHandler delege = new ConsoleHandler(Name);
        delege += new ConsoleHandler(Surname); 

        delege();*/

        testEvent += new ConsoleHandler(Name);
        testEvent +=new ConsoleHandler(Surname);
        testEvent();

    }
}
   

