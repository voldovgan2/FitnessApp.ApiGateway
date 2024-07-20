namespace FitnessApp.ApiGateway.Configuration;

public class CollectionApiClientSettings
{
    public string ApiName { get; set; }
    public string Url { get; set; }
    public string Scope { get; set; }
    public string GetItemsMethodName { get; set; }
    public string AddItemMethodName { get; set; }
    public string EditItemMethodName { get; set; }
    public string RemoveItemMethodName { get; set; }
}
