var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.multiple_Agents>("multiple-agents");

builder.Build().Run();
