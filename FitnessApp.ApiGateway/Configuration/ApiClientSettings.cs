namespace FitnessApp.ApiGateway.Configuration
{
    public class ApiClientSettings
    {
        public string ApiName { get; set; }
        public string Url { get; set; }
        public string Scope { get; set; }
        public string GetItemsMethodName { get; set; }
        public string GetItemMethodName { get; set; }
        public string CreateItemMethodName { get; set; }
        public string UpdateItemMethodName { get; set; }
        public string DeleteItemMethodName { get; set; }
    }
}
