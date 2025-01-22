/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

/*
    The infinite iteration has to call the getinstructions everytime in the loop because the list gets a fixed size on HESManager
 */

namespace HES
{
    class Program
    {
        static void Main(string[] args)
        {
            new HESManager().Start();
        }
    }
}
