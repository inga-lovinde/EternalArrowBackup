namespace EternalArrowBackup.Hasher.Sha256
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EternalArrowBackup.Hasher.Contracts;

    public class Sha256ContentHasher : IContentHasher
    {
        public Task<string> ComputeHash(Stream content)
        {
            return Task.Run(() =>
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var hash = sha256.ComputeHash(content);
                    return string.Concat(hash.Select(b => b.ToString("x2")));
                }
            });
        }
    }
}
