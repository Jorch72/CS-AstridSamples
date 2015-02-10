using Astrid.Windows;
using Isometric;
namespace Isometric.Windows
{
    class Program
    {
        static void Main()
        {
            var config = new WindowsApplicationConfig
            {
                Title = "Isometric Astrid Demo",
                Width = 800,
                Height = 540,
                ContentPath = "Content"
            };

            using (var application = new WindowsApplication(config))
            {
                var game = new Game(application);
                application.Run(game);
            }
        }
    }
}
