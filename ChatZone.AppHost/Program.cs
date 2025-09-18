var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ChatZone_API>("apiservice-chat");
builder.AddProject<Projects.ChatZone_MessageBroker>("apiservice-message");

builder.Build().Run();
