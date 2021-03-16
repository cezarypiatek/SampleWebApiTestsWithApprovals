# SampleWebApiTestsWithApprovals

This is sample project for article [Testing WebAPI with ApprovalTests.NET](https://cezarypiatek.github.io/post/testing-web-api-with-approval-tests/)


```cs
[Test]
public async Task should_fetch_newly_created_user()
{
    using (ApprovalResults.ForScenario("First User"))
    {
        // Create new users
        var createResponse = await apiClient.PostAsJsonAsync("/user/", new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@gmail.com"
        });

        var createResult = await createResponse.Content.ReadAsAsync<EntityCreatedResult>();

        // Fetch newly created user
        var getUserResponsePayload = await apiClient.GetStringAsync("/user/" + createResult.Id);

        // Verify the expectations
        Approvals.VerifyJson(getUserResponsePayload.WithIgnores("$.id"));
    }

    using (ApprovalResults.ForScenario("Second User"))
    {
        // Create new users
        var createResponse = await apiClient.PostAsJsonAsync("/user/", new CreateUserDTO
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "john.doe@gmail.com"
        });

        var createResult = await createResponse.Content.ReadAsAsync<EntityCreatedResult>();

        // Fetch newly created user
        var getUserResponsePayload = await apiClient.GetStringAsync("/user/" + createResult.Id);

        // Verify the expectations
        Approvals.VerifyJson(getUserResponsePayload.WithIgnores("$.id"));
    }

    using (ApprovalResults.ForScenario("All users"))
    {   
        // Fetch all users
        var getAllUsersResponsePayload = await apiClient.GetStringAsync("/user/");

        // Verify the expectations
        Approvals.VerifyJson(getAllUsersResponsePayload.WithIgnores("$[*].id"));
    }
}
```
