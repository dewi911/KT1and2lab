using System;
using System.Collections.Generic;

public class Identifier
{
    public string Value { get; set; }

    public Identifier(string value)
    {
        Value = value;
    }
}

public class HashTable
{
    private const int TableSize = 200;
    private List<Identifier>[] table;

    public HashTable()
    {
        table = new List<Identifier>[TableSize];
        for (int i = 0; i < TableSize; i++)
        {
            table[i] = new List<Identifier>();
        }
    }

    public void Insert(Identifier identifier)
    {
        int index = HashFunction(identifier.Value);
        table[index].Add(identifier);
    }

    public bool Search(Identifier identifier, out int comparisons)
    {
        int index = HashFunction(identifier.Value);
        comparisons = 0;

        foreach (var id in table[index])
        {
            comparisons++;
            if (id.Value == identifier.Value)
                return true;
        }

        return false;
    }

    private int HashFunction(string value)
    {
        
        int hash = 0;
        foreach (char c in value)
        {
            hash = (hash + (int)c) % TableSize;
        }
        return hash;
    }
}

class Program
{
    static void Main(string[] args)
    {
        HashTable hashTable = new HashTable();

        string[] identifiers = System.IO.File.ReadAllLines("C:\\Users\\Admin\\input.txt");
        foreach (string identifier in identifiers)
        {
            hashTable.Insert(new Identifier(identifier));
        }
        Console.WriteLine("vvedite indetificator");
        string indtf = Console.ReadLine();
        Identifier target = new Identifier(indtf);
        int comparisons;
        bool found = hashTable.Search(target, out comparisons);

      
        if (found)
            Console.WriteLine($"indetificator '{indtf}' found  {comparisons}");
        else
            Console.WriteLine($"indetificator '{indtf}' not found  {comparisons}");
    }
}