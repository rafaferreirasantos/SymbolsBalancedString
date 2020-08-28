using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;

namespace Test
{
  class Result
  {
    public bool isBalanced { get; set; }
    public int lastIndexTested { get; set; }
  }
  class Program
  {
    static int control = 0;
    static List<string[]> symbols = new List<string[]>();
    static string[] allSymbols()
    {
      return symbols.SelectMany(s => new string[] { s[0], s[1] }).ToArray();
    }
    static void Main(string[] args)
    {
      symbols.Add(new string[] { "{", "}" });
      symbols.Add(new string[] { "(", ")" });
      symbols.Add(new string[] { "[", "]" });

      Console.WriteLine(isBalanced("[{(00)}]").isBalanced);
      Console.ReadLine();
    }
    static string[] findSymbolPair(string value)
    {
      return symbols.Where(s => s[0] == value || s[1] == value).FirstOrDefault();
    }
    static Result isBalanced(string value, string symbol = "", int initialPos = 0)
    {
      for (int i = initialPos; i < value.Length; i++)
      {
        var current = value[i].ToString();
        if (allSymbols().Contains(current))
        {
          //É um símbolo
          var symbolPair = findSymbolPair(current);
          // Abertura
          if (symbolPair[0] == current)
          {
            control++;
            var result = isBalanced(value, current, i + 1);
            if (!result.isBalanced)
            {
              // Não está balanceado, pode parar por aqui
              return new Result() { isBalanced = false, lastIndexTested = result.lastIndexTested };
            } else
            {
              // O conteúdo está balanceado, continue a partir do final do balanceamento.
              i = result.lastIndexTested;
              continue;
            }
          }
          // Fechamento
          if (symbolPair[1] == current)
          {
            if (symbolPair[0] != symbol)
            {
              // Símbolo de fechamento diferente da última abertura
              return new Result() { isBalanced = false, lastIndexTested = i };
            } else
            {
              // Fechou o último síbolo aberto corretamente.
              control--;
              if (control > 0)
              {
                return new Result() { isBalanced = true, lastIndexTested = i };
              } else
              {
                continue;
              }
            }

          }
        }
      }
      return new Result() { isBalanced = control == 0, lastIndexTested = value.Length - 1 };
    }

  }

}
