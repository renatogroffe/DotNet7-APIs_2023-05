using Microsoft.AspNetCore.Builder;

namespace Groffe.AspNetCore.ApiKeyChecking;

public class ApiKeyCheckingPipeline
{
    public void Configure(IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseApiKeyChecking();
    }
}