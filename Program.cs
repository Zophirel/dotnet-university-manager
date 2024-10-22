using University.BLogic;
namespace University
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try{
                Menu.ShowMenu();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return;
        }
    
    }
    
}