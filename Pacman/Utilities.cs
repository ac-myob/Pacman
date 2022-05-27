namespace Pacman;

public static class Utilities
{
   public static int Mod(int num, int m) 
   {
        var r = num % m;
        return r < 0 ? r + m : r;
   }
}
