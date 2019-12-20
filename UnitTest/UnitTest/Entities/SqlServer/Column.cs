using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public class Column:ColumnBase
{
    private bool nullable;
    private bool identity;
    private int columnId;

    public int ColumnId
    {
        get { return columnId; }
        set { columnId = value; }
    }

    public bool Nullable
    {
        get { return nullable; }
        set { nullable = value; }
    }

    public bool Identity
    {
        get { return identity; }
        set { identity = value; }
    }


}
