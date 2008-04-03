/// <summary>
/// Summary description for QueryParser
/// </summary>
public class InitializerProperties
{
    public InitializerProperties(
        string graphicsResourceName,
        string dataSourceType,
        string dataSourceDefinition,
        string host
        )
    {
        this.graphicsResourceName = graphicsResourceName;
        this.dataSourceType = dataSourceType;
        this.dataSourceDefinition = dataSourceDefinition;
        this.host = host;
        this.identity = "";
    }

    private string graphicsResourceName;
    public string GraphicsResourceName
    {
        get { return graphicsResourceName; }
    }

    private string dataSourceType;
    public string DataSourceType
    {
        get { return dataSourceType; }
    }

    private string dataSourceDefinition;
    public string DataSourceDefinition
    {
        get { return dataSourceDefinition; }
    }

    private string identity;
    public string Identity
    {
        get { return identity; }
        set { this.identity = value; }
    }

    private string host;
    public string Host
    {
        get { return host; }
    }
}
