using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuController : MonoBehaviour {
    
    [SerializeField]
    public Stack<Menu> MenuExecutionStack;
    [SerializeField]
    public Stack<int> SelectionStack;

    public void PushMenu(Menu x, int selection = -1)
    {
        MenuExecutionStack.Push(x);
        if (selection > -1)
            SelectionStack.Push(selection);

        x.transform.SetAsLastSibling();
        if (!x.gameObject.activeInHierarchy)
        {
            x.MenuRoot.gameObject.SetActive(true);
        }
        x.selection = 0;
    }

    public void PopMenu()
    {
        MenuExecutionStack.Pop();
        if (MenuExecutionStack.Count > 0)
        {
            MenuExecutionStack.Peek().MenuRoot.SetAsLastSibling();
            if (SelectionStack.Count > 0)
                MenuExecutionStack.Peek().selection = SelectionStack.Pop();
        }
    }


}
