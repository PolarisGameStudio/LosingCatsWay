using UnityEngine;

public class MvcBehaviour : MonoBehaviour
{
    private MyApplication app;

    protected MyApplication App
    {
        get 
        {
            if (app == null)
            {
                app = FindObjectOfType<MyApplication>();
            }

            return app; 
        }
    }

    public delegate void Callback();
    public delegate void CallbackValue(object value);
    public delegate void CallbackValueToValue(object first, object second);
}