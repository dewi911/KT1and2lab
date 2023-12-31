using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;

enum LexType
{
    Number, Operator, DoWhileOperator, Const, ComparisonMarks, Bracket, Identifier, UnknownLex
}

class Lex
{
    public string LexName { get; set; }
    public LexType Type { get; }

    public Lex(string lexeme, LexType type)
    {
        LexName = lexeme;
        Type = type;
    }
}

public class HashTableOpenAddressingWithChaining
{
    private string[] table;
    private int size;
    private List<string>[] chains;

    public HashTableOpenAddressingWithChaining(int size)
    {
        this.size = size;
        table = new string[size];
        chains = new List<string>[size];

        for (int i = 0; i < size; i++)
        {
            chains[i] = new List<string>();
        }
    }

    private int HashFunction(string key)
    {
        int hash = 0;
        foreach (char symbol in key)
        {
            hash = (hash * 27 + symbol) % size;
        }
        return hash;
    }

    public void InsertOpenAddressing(string key)
    {
        int hash = HashFunction(key);
        if (table[hash] != null && table[hash] == key)
        {
            return;
        }
        else
        {
            table[hash] = key;
        }
    }

    public void InsertChaining(int index, string key)
    {
        chains[index].Add(key);
    }

    public (bool, int) SearchOpenAddressing(string key)
    {
        int hash = HashFunction(key);
        int iterations = 0;
        while (table[hash] != null)
        {
            iterations++;
            if (table[hash] == key)
            {
                return (true, iterations);
            }
            hash = (hash + 1) % size;
        }
        return (false, iterations);
    }

   public (bool, int) SearchChaining(string key)
   {
       int iterations = 0;
       
       foreach (var chain in chains)
       {
           
           if (chain.Contains(key)) 
           {
               
               return (true, iterations);
                break;
           }
           else
           {
               iterations++;
           }
               
       }
       return (false, iterations);
   }

    


    // ... Other methods for data access, deletion, etc.
}

class Program
{
    static List<Lex> LexicalAnalysis(string input, HashTableOpenAddressingWithChaining hashTable, List<string> identifierList, int chainornot)
    {
        List<Lex> tokens = new List<Lex>();
        string patterns = @"(do)|(while)|(:=|[-+*/();])|(<=)|(>=)|(=)|("".*?"")|([a-zA-Z_][a-zA-Z0-9_]*)|(\d+)|([^ \t\r\n]+)";
        Regex regex = new Regex(patterns);
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            string lexeme = match.Value;
            LexType type = LexType.UnknownLex;

            if (Regex.IsMatch(lexeme, @"^\d+$"))
            {
                type = LexType.Number;
            }
            else if (Regex.IsMatch(lexeme, @"^:=|[+\-*/;]$"))
            {
                type = LexType.Operator;
            }
            else if (Regex.IsMatch(lexeme, @"^do|while$"))
            {
                type = LexType.DoWhileOperator;
            }
            else if (Regex.IsMatch(lexeme, @"^"".*?""$"))
            {
                type = LexType.Const;
            }
            else if (Regex.IsMatch(lexeme, @"^=|<=|>=$"))
            {
                type = LexType.ComparisonMarks;
            }
            else if (Regex.IsMatch(lexeme, @"^[()]$"))
            {
                type = LexType.Bracket;
            }
            else if (Regex.IsMatch(lexeme, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
            {
                type = LexType.Identifier;
                if (!tokens.Any(t => t.LexName == lexeme))
                {
                    tokens.Add(new Lex(lexeme, type));
                }
                if (chainornot == 1)
                {
                    hashTable.InsertOpenAddressing(lexeme);
                    identifierList.Add(lexeme);
                }
                else if (chainornot == 2)
                {
                    hashTable.InsertChaining(0, lexeme);
                    identifierList.Add(lexeme);
                }
            }
        }
        return tokens;
    }

    static void Main()
    {
        string inputText = File.ReadAllText("C:\\Users\\Admin\\input.txt");
        HashTableOpenAddressingWithChaining hashTable = new HashTableOpenAddressingWithChaining(200);
        HashTableOpenAddressingWithChaining hashTablechains = new HashTableOpenAddressingWithChaining(200);
        List<string> identifierList = new List<string>();
        List<Lex> tokens = LexicalAnalysis(inputText, hashTable, identifierList, 1);

        List<string> identifierList2 = new List<string>();
        List<Lex> secondtokens = LexicalAnalysis(inputText, hashTablechains, identifierList2, 2);

        foreach (Lex token in tokens)
        {
            
                if (token.Type == LexType.Identifier)
                {
                    (bool containsIdentifier, int iterationsHash) = hashTable.SearchOpenAddressing(token.LexName);
                    (bool containsIdentifier1, int iterationsHash1) = hashTable.SearchChaining(token.LexName);
                    Console.WriteLine($"Идентификатор: {token.LexName}\n --итерации в таблице: в обычном {iterationsHash} и {token.LexName} с помощью цепочек {iterationsHash1}");
                }
            
        }
        foreach (Lex tokenn in secondtokens)
        {

            if (tokenn.Type == LexType.Identifier)
            {
                (bool containsIdentifier1, int iterationhash) = hashTablechains.SearchChaining(tokenn.LexName);
                (bool containsIdentifier, int iterationsHash) = hashTable.SearchOpenAddressing(tokenn.LexName);
                Console.WriteLine($"Идентификатор: {tokenn.LexName}\n --итерации в таблице: в обычном {iterationsHash}  и {tokenn.LexName} с помощью цепочек {iterationhash}");
            }

        }
        
    }
}
