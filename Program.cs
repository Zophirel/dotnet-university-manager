using University.DataModel;

namespace University
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try{
                Menu.ShowMainMenu();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return;
        }
    }
}
