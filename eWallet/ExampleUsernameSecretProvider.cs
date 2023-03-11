using FlakeyBit.DigestAuthentication.Implementation;


internal class UsernameSecretProvider : IUsernameHashedSecretProvider
{
    public Task<string> GetA1Md5HashForUsernameAsync(string username, string realm)
    {
        if (username == "eddie" && realm == "some-realm")
        {
            // The hash value below would have been pre-computed & stored in the database.
            var hash = DigestAuthentication.ComputeA1Md5Hash("eddie", "some-realm", "starwars123");

            return Task.FromResult(hash);
        }

        // User not found
        return Task.FromResult<string>(null);
    }
}
