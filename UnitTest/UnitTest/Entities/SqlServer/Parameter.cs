public class Parameter : ColumnBase
{
    private int parameterId;
    private bool output;
    private bool cursorRef;
    private bool hasDefaultValue;
    private object defaultValue;

    public int ParameterId
    {
        get { return parameterId; }
        set { parameterId = value; }
    }

    public bool Output
    {
        get { return output; }
        set { output = value; }
    }

    public bool CursorRef
    {
        get { return cursorRef; }
        set { cursorRef = value; }
    }

    public bool HasDefaultValue
    {
        get { return hasDefaultValue; }
        set { hasDefaultValue = value; }
    }

    public object DefaultValue
    {
        get { return defaultValue; }
        set { defaultValue = value; }
    }
}