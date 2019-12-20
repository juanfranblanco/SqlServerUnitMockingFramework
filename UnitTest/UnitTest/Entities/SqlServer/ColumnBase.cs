public class ColumnBase
{
    private int objectId;
    private string typeName;
    private string name;
    private int systemTypeId;
    private int userTypeId;
    private int maxLength;
    private int precission;
    private int scale;
    private bool isXmlDocument;
    private int xmlCollectionId;

    public int ObjectId
    {
        get { return objectId; }
        set { objectId = value; }
    }

    public string TypeName
    {
        get { return typeName; }
        set { typeName = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int SystemTypeId
    {
        get { return systemTypeId; }
        set { systemTypeId = value; }
    }

    public int UserTypeId
    {
        get { return userTypeId; }
        set { userTypeId = value; }
    }

    public int MaxLength
    {
        get { return maxLength; }
        set { maxLength = value; }
    }

    public int Precission
    {
        get { return precission; }
        set { precission = value; }
    }

    public int Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    public bool IsXmlDocument
    {
        get { return isXmlDocument; }
        set { isXmlDocument = value; }
    }

    public int XmlCollectionId
    {
        get { return xmlCollectionId; }
        set { xmlCollectionId = value; }
    }
}