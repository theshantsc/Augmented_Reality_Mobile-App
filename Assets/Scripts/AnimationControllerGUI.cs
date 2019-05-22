using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerGUI : MonoBehaviour {

    public GUIAnimFREE[] generalMenu;

    private void Start()
    {
        foreach(GUIAnimFREE ui in generalMenu)
        {
            
            ui.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        }
    }
}
