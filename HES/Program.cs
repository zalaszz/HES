/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

/*
    Character & quebra a linha e causa disrupção no envio das keys
    Usar apenas 1 HESFile e passar para construtores para evitar criar vários
    Criar interfaces para tornar numa biblioteca
*/

namespace HES
{
    class Program
    {
        static void Main(string[] args)
        {
            //new HESManager("srptlxfishp101").Start();
            new HESManager("Notepad").Start();
        }
    }
}
