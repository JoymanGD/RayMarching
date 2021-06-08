using System;

namespace RayMarchingDirectX
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new RayMarchingGame())
                game.Run();
        }
    }
}
