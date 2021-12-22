using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;

public class TransformInfo
{
    public Vector3 position;
    public Quaternion rotation;

}
public class LSystem : MonoBehaviour
{
    [SerializeField] public float length = 10f;
    [SerializeField] public float angle = 10f;
    [SerializeField] public int iterations;
    [SerializeField] public string axRule = "[FX][-FX][+FX]";
    [SerializeField] public GameObject branch;
    private const string axiom = "X";

    private Stack<TransformInfo> transformStack;
    private Dictionary<char, string> rules;
    private string currentString = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<TransformInfo>();
        rules = new Dictionary<char, string>
        {
            {'X', axRule},
            {'F', "FF"}
        };

        GenerateTree();
    }

    private void GenerateTree()
    {
        currentString = axiom;
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i< iterations; i++)
        {
            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
            }
            currentString = sb.ToString();
            sb = new StringBuilder();
        }
        
        foreach (char i in currentString)
        {
            switch (i)
            {
                
                case 'F':
                    Vector3 initialPosition = transform.position;
                    transform.Translate(Vector3.up * length);
                    GameObject treeSegment = Instantiate(branch);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    treeSegment.GetComponent<LineRenderer>().SetPosition(1, transform.position);
                    break;
                case 'X':
                    break; 
                case '+': //Clockwise
                    transform.Rotate(Vector3.back * angle);
                    break;
                case '-': //Anti-Clockwise
                    transform.Rotate(Vector3.forward * angle);
                    break;
                case '[': //Save Initial State
                    transformStack.Push(new TransformInfo()
                    {
                        position = transform.position,
                        rotation = transform.rotation
                    });
                    break;
                case ']': //Save Final State
                    TransformInfo ti = transformStack.Pop();
                    transform.position = ti.position;
                    transform.rotation = ti.rotation;
                    break;
                default:
                    throw new InvalidOperationException("Invalid L-Tree operation");
            }
        }
    }    
}
