/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

/*
    Character & quebra a linha e causa disrupção no envio das keys
    ResourceManager GetInstance must only execute after resources are complete loading
    ReadCSV must accept either file extension or filename (with extension)
    Criar CSVDTO e separar a classe MenuFields do MenuDTO e add campo Value
    Create a CSVMenu
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
