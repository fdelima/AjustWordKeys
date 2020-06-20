using System;
using System.IO;
using System.Security.Cryptography;

namespace AjustWordKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***********************************************************************");
            Console.WriteLine("Inicializando ajuste das palavras configuradas no arquivo keywords.json");
            Console.WriteLine("***********************************************************************");

            Console.WriteLine("Criando diretório");
            var dest = Path.Combine(Environment.CurrentDirectory, "AjustedWordKeys");
            Console.WriteLine(dest);

            try
            {
                if (!Directory.Exists(dest))
                    Directory.CreateDirectory(dest);

                AjustKeys.Ajust(Environment.CurrentDirectory, dest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("******************************************************");
            Console.WriteLine("Finalizando!");
            Console.WriteLine("******************************************************");

            Console.Beep();
            Console.ReadKey();

        }
    }
}
