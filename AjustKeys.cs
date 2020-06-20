using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AjustWordKeys
{
    public class AjustKeys
    {
        public static string KeyWordFile { get; } = "keywords.json";

        public static void Ajust(string sourceFolderPath, string destinationFolderPath)
        {
            var replaceWords = LoadKeyWords(sourceFolderPath);
            if (replaceWords == null || replaceWords.Words == null)
                throw new ApplicationException($"Erro na leitura do arquivo {KeyWordFile}");

            var files = Directory.GetFiles(sourceFolderPath, "*.*", SearchOption.AllDirectories);

            var arq = "";
            var arqDestino = "";
            var pastaDestino = "";

            foreach (var f in files)
            {
                if (!IgnoredFile(f))
                {
                    //ajustando o caminho do arquivo
                    arqDestino = f.Replace(sourceFolderPath, destinationFolderPath);
                    Console.WriteLine($"Ajustando o caminho do arquivo: {arqDestino}");

                    foreach (var word in replaceWords.Words)
                    {
                        //ainda ajustanto o caminho
                        arqDestino = arqDestino.Replace(word.Key, word.Value);
                        Console.WriteLine($"Ajustando o caminho do arquivo: key:{word.Key} value:{word.Value}");
                    }

                    //lendo conteudo do arquivo
                    arq = File.ReadAllText(f, Encoding.UTF8);
                    Console.WriteLine($"Lendo conteudo do arquivo: key:{f}");

                    foreach (var word in replaceWords.Words)
                    {
                        //Ajustando conteudo do arquivo
                        arq = arq.Replace(word.Key, word.Value);
                        Console.WriteLine($"Ajustando conteúdo do arquivo: key:{word.Key} value:{word.Value}");
                    }

                    pastaDestino = arqDestino.Substring(0, arqDestino.LastIndexOf("\\"));
                    if (!Directory.Exists(pastaDestino))
                        Directory.CreateDirectory(pastaDestino);

                    File.WriteAllText(arqDestino, arq, Encoding.UTF8);
                    Console.WriteLine($"Gravando conteudo do arquivo: key:{arqDestino}");
                }
            }

        }
        private static bool IgnoredFile(string f)
        {
            return f.ToLower().StartsWith("ajustkeys")
                    || f.ToLower().Contains("\\bin")
                    || f.ToLower().Contains("\\obj")
                    || f.ToLower().Contains("\\.git\\")
                    || f.ToLower().Contains(KeyWordFile)
                    || f.ToLower().Contains(".exe")
                    || f.ToLower().Contains(".dll");
        }

        public static KeyWords LoadKeyWords(string folderPath)
        {
            using (StreamReader r = new StreamReader($"{folderPath}\\{KeyWordFile}"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<KeyWords>(json);
            }
        }
    }

}
