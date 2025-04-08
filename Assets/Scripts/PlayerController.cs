using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //start med references
    public Rigidbody2D rb;
    public float moveSpeed;

    public InputActionReference moveInput, actionInput;

    public Animator anim;

    public enum ToolType
    {
        plough,
        wateringCan,
        seeds,
        basket
    }

    public ToolType currentTool;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIController.instance.SwitchTool((int)currentTool); //sætter min plough som default start tool, når man starter spillet 
    }

    // Update is called once per frame
    void Update()
    {
        //rb.linearVelocity = new Vector2(moveSpeed, 0f); 
        rb.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

        //ændre retningen på vores sprite så den matcher den vej vi går
        if (rb.linearVelocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.linearVelocity.x > 0f)
        {
            transform.localScale = Vector3.one;
        }


        bool hasSwitchedTool = false;

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            currentTool++;

            if ((int)currentTool >= 4)
            {
                currentTool = ToolType.plough;
            }

            hasSwitchedTool = true;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentTool = ToolType.plough;
            hasSwitchedTool = true;

        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentTool = ToolType.wateringCan;
            hasSwitchedTool = true;

        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentTool = ToolType.seeds;
            hasSwitchedTool = true;

        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            currentTool = ToolType.basket;
            hasSwitchedTool = true;

        }

        if (hasSwitchedTool == true)
        {
            //FindFirstObjectByType<UIController>().SwitchTool((int)currentTool); //fungerer fint, men hvis man har mange objekter at løbe igennem kan det tage rigtig lang tid for den
            UIController.instance.SwitchTool((int)currentTool); //nu kalder vi bare vores singleton instance fra UIcontroller scriptet i stedet 
        }

        if (actionInput.action.WasPressedThisFrame())
        {
            UseTool();
        }

        anim.SetFloat("speed", rb.linearVelocity.magnitude);

    }

    void UseTool()
    {
        GrowBlock block = null;

        block = FindFirstObjectByType<GrowBlock>(); //find objektet der har en grow block script på sig

        //block.PloughSoil();

        if (block != null)
        {
            switch (currentTool) //skifter imellem mine tools, så jeg fx ikke kan pløje jorden når jeg holder vandkanden
            {
                case ToolType.plough:

                    block.PloughSoil();

                    break;

                case ToolType.wateringCan:

                    break;

                case ToolType.seeds:

                    break;

                case ToolType.basket:

                    break;

            }
        }
    }
}