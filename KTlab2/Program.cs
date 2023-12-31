using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;//variant 1

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
    public string[] table;
    private List<List<string>> chains;
    public int size;
    public HashTable(int size)
    {
        this.size = size;
        table = new string[size];
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
    public void Insert(string identifier)
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
    public (bool, int) Containschains(string key)
    {
        int index = HashFunction(key);
        return (table[index].Contains(key), index);
    }

    public void Insertchains(string key)
    {
        int index = HashFunction(key);
        chains[index].Add(key);
    }
    public void Add(string key)
    {
        int index = HashFunction(key);
        chains[index].Add(key);
    }
}
class Program
{
    static List<Lex> LexicalAnalysis(string input, HashTable hashTable, List<string> identifierList)
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
                hashTable.Insert(lexeme);
                identifierList.Add(lexeme);
            }
        }
        return tokens;
    }


    static void Main(string[] args)
    {
        string inputText = File.ReadAllText("C:\\Users\\Admin\\input.txt");
        HashTable HashTablechains = new HashTable(200);
        HashTable hashTable = new HashTable(200);
        List<string> identifierList = new List<string>();
        List<Lex> tokens = LexicalAnalysis(inputText, hashTable, identifierList);
        





        // HashTablechains listhashTable = new HashTablechains(10);
        //
        // listhashTable.Add("apple");
        // listhashTable.Add("banana");
        // listhashTable.Add("bananna");
        // listhashTable.Add("bbbbbanana");
        //
        // listhashTable.Add("abc");
        // listhashTable.Add("bbbananana");
        // listhashTable.Add("aabc");
        // listhashTable.Add("abbbc");



        Console.WriteLine("Hash table contents:");
        

        Console.WriteLine("Check if 'banana' is in the hash table: " + hashTable.Contains("abc1"));
        Console.WriteLine("Check if 'orange' is in the hash table: " + hashTable.Containschains("orange"));
        Console.WriteLine();
        
        foreach (Lex token in tokens)
        {
            if (token.Type == LexType.Identifier)
            {
                (bool containsIdentifier, int iterationsHash) = hashTable.Contains(token.LexName);

                Console.WriteLine($"Идентификатор: {token.LexName}\n --итерации в таблице: {iterationsHash}");

            }
        }
    }
}