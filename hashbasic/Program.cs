using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;

enum LexType
{
    Number, Operator, DoWhileOperator, Const, ComparisonMarks, Bracket, Identifier, UnknownLex
}
class Lex
{
    public string LexName { get; }
    public LexType Type { get; }

    public Lex(string lexeme, LexType type)
    {
        LexName = lexeme;
        Type = type;
    }
}
class HashTable
{
    private string[] table;
    private int size;
    private List<string>[] chains;
    public HashTable(int size)
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
    public void Insertbasiccc(string identifier)
    {
        int hash = HashFunction(identifier);
        if (table[hash] != null && table[hash] == identifier)
        {
            return;
        }
        else
        {
            table[hash] = identifier;
        }
    }
    public void Insertbasic(string key)
    {
        int index = HashFunction(key);
        if (table[index] == null)
        {
            table[index] = key;
        }
        else
        {
            // Collision: Use chaining
            chains[index].Add(key);
        }
    }
    public void Inserchains(int index, string key)
    {
        chains[index].Add(key);
    }
    public bool Containschains(int index, string key)
    {
        int count = 0;
        count++;
        return chains[index].Contains(key);
        count++;
    }
    public (bool, int) Contains(string identifier)
    {
        int hash = HashFunction(identifier);
        int iterations = 0;
        while (table[hash] != null)
        {
            iterations++;
            if (table[hash] == identifier)
            {
                return (true, iterations);
            }
            hash = (hash + 1) % size;
        }
        return (false, iterations);
    }

     
    
}
class Program
{

    static List<Lex> LexicalAnalysis(string input, HashTable hashTable, List<string> identifierList, int chainornot)
    {
        List<Lex> tokens = new List<Lex>();
        string patterns = @"(do)|(while)|(:=|[-+*/();])|(<=)|(>=)|(=)|("".*?"")|([a-zA-Z_][a-zA-Z0-9_]*)|(\d+)|([^ \t\r\n]+)";
        Regex regex = new Regex(patterns);
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            int count = 0;
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
                    hashTable.Insertbasic(lexeme);
                    identifierList.Add(lexeme);
                }
                else if (chainornot == 2)
                {
                    hashTable.Inserchains(count, lexeme);
                    identifierList.Add(lexeme);
                }
            }
        }
        return tokens;
    }
    static void Main()
    {
        string inputText = File.ReadAllText("C:\\Users\\Admin\\input.txt");
        HashTable hashTable = new HashTable(200);
        HashTable hashTablechains = new HashTable(200);
        List<string> identifierList = new List<string>();
        List<Lex> tokens = LexicalAnalysis(inputText, hashTable, identifierList, 1);

        List<string> identifierList2 = new List<string>();
        List<Lex> secondtokens = LexicalAnalysis(inputText, hashTablechains, identifierList2, 2);
        foreach (Lex token in tokens)
        {
            
                if (token.Type == LexType.Identifier)
                {
                    (bool containsIdentifier, int iterationsHash) = hashTable.Contains(token.LexName);
                    (bool containsIdentifier1, int iterationsHash1) = hashTablechains.Contains(token.LexName);
                    Console.WriteLine($"Идентификатор: {token.LexName}\n --итерации в таблице:в обычном  {iterationsHash} и ");
                }
            
        }
        foreach (Lex token in tokens)
        {
            

                if (token.Type == LexType.Identifier)
                {
                    
                    (bool containsIdentifier1, int iterationsHash1) = hashTablechains.Contains(token.LexName);
                    Console.WriteLine($"Идентификатор: {token.LexName}\n --итерации в таблице:в обычном  {iterationsHash1} и ");
                }
            
        }

    }
}