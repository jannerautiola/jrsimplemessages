# Simple message service


# Configuration files

Add ApiKeys.json file to SimpleMessagesSignalR project:
```sh
{
  "AzureSignalRConnectionString": "YourSignalRServiceEndpointHere"
}
```

Add ApiKeys.json file to SimpleMessagesWebApi:
```sh
{
  "AzureCosmosDbEndpoint": "YourCosmosDBEnpointHere",
  "AzureCosmosDbKey": "YourCosmosDBKeyHere"
}
```

Add local.settings.json file to SimpleMessagesSignalRFunction:
```sh
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "YourAzureWebJobsStorageHere",
    "AzureWebJobsDashboard": "YourAzureWebJobsDashboardHere",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "CosmosDBConnectionString": "YourCosmosDBConnectionStringHere"
  }
}
```

Also add the same values to Azure functions Application Settings.
