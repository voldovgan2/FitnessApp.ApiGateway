using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

var authMethod = new TokenAuthMethodInfo("dev-only-token");
#pragma warning disable S1075 // URIs should not be hardcoded
var config = new VaultClientSettings("http://localhost:8200", authMethod);
#pragma warning restore S1075 // URIs should not be hardcoded
var vaultClient = new VaultClient(config);
var data = new Dictionary<string, object>();
data.Add("Minio:SecretKey", "minio_password");
if (data.Count == 1)
    await vaultClient.V1.Secrets.KeyValue.V1.WriteSecretAsync("fitness-app", data);
var savaTest = await vaultClient.V1.Secrets.KeyValue.V1.ReadSecretAsync("fitness-app");
Console.WriteLine(savaTest);
Console.ReadLine();
