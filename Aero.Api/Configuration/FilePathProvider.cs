//using Aero.Application.Interfaces;

//namespace Aero.Api.Configuration
//{
//    public sealed class FilePathProvider : IFilePathProvider
//    {
//        public string Users { get; }
//        public string Maps { get; }

//        public FilePathProvider(IWebHostEnvironment env)
//        {
//            var root = env.ContentRootPath;

//            Users = Path.Combine(root, "images", "users");
//            Maps = Path.Combine(root, "images", "maps");

//            Directory.CreateDirectory(Users);
//            Directory.CreateDirectory(Maps);
//        }
//    }
//}


using Aero.Application.Interfaces;

namespace Aero.Api.Configuration
{
    public sealed class FilePathProvider : IFilePathProvider
    {
        public string Users { get; }
        public string Maps { get; }

        public FilePathProvider()
        {
            // Runtime directory (where the app is running)
            var runtimeRoot = AppContext.BaseDirectory;

            Users = Path.Combine(runtimeRoot, "images", "users");
            Maps = Path.Combine(runtimeRoot, "images", "maps");

            Directory.CreateDirectory(Users);
            Directory.CreateDirectory(Maps);
        }
    }
}