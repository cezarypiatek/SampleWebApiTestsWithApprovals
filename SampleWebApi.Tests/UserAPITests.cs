using System.Net.Http;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Namers;
using NUnit.Framework;
using SampleWebApi.Users;

namespace SampleWebApi.Tests
{
    public class UserApiTests
    {
        [Test]
        public async Task should_be_able_to_fetch_new_user_v1()
        {
            // Setup application
            var applicationFactory = new SampleApplicationFactory();
            await applicationFactory.Install();
            var apiClient = applicationFactory.CreateClient();


            // Create new users
            var createResponse = await apiClient.PostAsJsonAsync("/user/", new CreateUserDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com"
            });
            var createResult = await createResponse.Content.ReadAsAsync<EntityCreatedResult>();

            // Fetch newly created user
            var getUserResponse = await apiClient.GetAsync("/user/" + createResult.Id);
            var foundUser = await getUserResponse.Content.ReadAsAsync<UserDTO>();

            // Verify the expectations
            Assert.Multiple(() =>
            {
                Assert.AreEqual(createResult.Id, foundUser.Id, "User Id different than expected");
                Assert.AreEqual("John", foundUser.FirstName, "User FirstName different than expected");
                Assert.AreEqual("Doe", foundUser.LastName, "User LastName different than expected");
            });
        }


        [Test]
        public async Task should_be_able_to_fetch_new_user_v2()
        {
            // Setup application
            var applicationFactory = new SampleApplicationFactory();
            await applicationFactory.Install();
            var apiClient = applicationFactory.CreateClient();


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
        
        [Test]
        public async Task should_be_able_to_fetch_new_user_v3()
        {
            // Setup application
            var applicationFactory = new SampleApplicationFactory();
            await applicationFactory.Install();
            var apiClient = applicationFactory.CreateClient();
            
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
    }
}
