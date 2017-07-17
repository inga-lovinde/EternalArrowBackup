namespace EternalArrowBackup.Hasher.SHA1
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using EternalArrowBackup.Hasher.Contracts;

    public class SHA1ContentHasher : IContentHasher
    {
        public Task<string> ComputeHash(Stream content)
        {
            return Task.Run(() =>
            {
                using (var sha1 = System.Security.Cryptography.SHA1.Create())
                {
                    var hash = sha1.ComputeHash(content);
                    return string.Concat(hash.Select(b => b.ToString("x2")));
                }
            });
        }
    }
}
