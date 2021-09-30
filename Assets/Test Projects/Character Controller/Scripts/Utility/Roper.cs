using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Roper : MonoBehaviour
{
    public Transform Head;
    public Transform HandL;
    public Transform HandR;
    void RopeEmUp() 
    {
        //Loop from head to tail
        //For Each Add Rigidbody
        //Iterative add Hinge Joints to the Rigid Body above.
        //Continue from Tail by getting its parent until you reach the head.

        //Left
        Transform last = null;
        Transform pointer = HandL;
        while(last != Head)
        {
            if (!pointer.GetComponent<Rigidbody>()) {
                pointer.gameObject.AddComponent<Rigidbody>();
            }
            if (last) {
                last.gameObject.AddComponent<ConfigurableJoint>();
                last.GetComponent<ConfigurableJoint>().connectedBody = pointer.GetComponent<Rigidbody>();
            }
            last = pointer;
            pointer = pointer.parent;
        }


        //Right
        last = null;
        pointer = HandR;
        while (last != Head)
        {
            if (!pointer.GetComponent<Rigidbody>())
            {
                pointer.gameObject.AddComponent<Rigidbody>();
            }
            if (last)
            {
                last.gameObject.AddComponent<ConfigurableJoint>();
                last.GetComponent<ConfigurableJoint>().connectedBody = pointer.GetComponent<Rigidbody>();
            }
            last = pointer;
            pointer = pointer.parent;
        }
    }

    [CustomEditor(typeof(Roper))]
    public class RoperEditorExtension : Editor
    {
        public override void OnInspectorGUI()
        {
            Roper script = (Roper)target;
            if (GUILayout.Button("Rope Em Up"))
            {
                script.RopeEmUp();
            }
            DrawDefaultInspector();
        }
    }
}
