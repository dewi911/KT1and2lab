using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
//vartiant 25
class HashTable
{
    private const int TableSize = 200;  
    private const int RehashStep = 1;   

    private List<string>[] chainedTable;  
    private string[] rehashTable;  

    public HashTable()
    {
        chainedTable = new List<string>[TableSize];
        rehashTable = new string[TableSize];

        for (int i = 0; i < TableSize; i++)
        {
            chainedTable[i] = new List<string>();
        }
    }
    public void HashChained(string[] identifiers)
    {
        foreach (var identifier in identifiers)
        {
            int hash = CalculateHash(identifier);
            chainedTable[hash].Add(identifier);
        }
    }
    public void HashRehash(string[] identifiers)
    {
        foreach (var identifier in identifiers)
        {
            int hash = CalculateHash(identifier);
            while (rehashTable[hash] != null)
            {
                hash = (hash + RehashStep) % TableSize;  
            }
            rehashTable[hash] = identifier;
        }
    }
    public (int,int) SearchChained(string identifier)
    {
        int hash = CalculateHash(identifier);
        int iterations = 0;
        if (chainedTable[hash] != null)
        {
            for (int i = 0; i< TableSize; i++)
            {
                if (chainedTable[hash] != null)
                {


                    foreach (string item in chainedTable[i])
                    {
                        iterations++;
                        if (chainedTable[hash] == chainedTable[i])
                            return (iterations, hash);

                    }
                    
                }
            }
        }

        return (iterations, hash);
    }
    public (int,int) SearchRehash(string identifier)
    {
        int hash = CalculateHash(identifier);
        int iterations = 0;  
        if (rehashTable[hash] != null)
        {
            foreach (string item in rehashTable)
            {
                iterations++;
                if (item == identifier.ToString())
                {
                    return (iterations, hash);
                }
                
            }
        }
       

        return (iterations, hash);
    }
    private int CalculateHash(string identifier)
    {
        int hash = 0;
        foreach (char symbol in identifier)
        {
            hash = (hash * 27 + symbol) % TableSize;
        }     
        return hash;
    }
    public void PrintChainedIdentifiers()
    {
        Console.WriteLine("Идентификаторы для метода цепочек:");
        for (int i = 0; i < TableSize; i++)
        {
            Console.Write($"_");
            foreach (var identifier in chainedTable[i])
            {
                Console.Write($".{identifier}.   ");
            }      
        }
    }
    public void PrintRehashIdentifiers()
    {
        Console.WriteLine("Идентификаторы для обычного рехеширования:");
        for (int i = 0; i < TableSize; i++)
        {
            
                Console.Write($".{rehashTable[i]}.   ");
            
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string[] identifiers;
        try
        {
            identifiers = File.ReadAllLines("C:\\Users\\Admin\\input.txt");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("not found");
            return;
        }
        HashTable hashTable = new HashTable();
        HashTable hashTable1 = new HashTable();
        hashTable1.HashChained(identifiers);
        hashTable.HashRehash(identifiers);
        string searchIdentifier = "car";
        (int chainedIterations,int hashch) = hashTable1.SearchChained(searchIdentifier);
        (int rehashIterations, int hashh) = hashTable.SearchRehash(searchIdentifier);
        Console.WriteLine($"Итерации для метода цепочек при поиске идентификатора \"{searchIdentifier}\": {chainedIterations}");
        Console.WriteLine($"Итерации для обычного рехеширования при поиске идентификатора \"{searchIdentifier}\": {rehashIterations}");
        hashTable1.PrintChainedIdentifiers();
        hashTable.PrintRehashIdentifiers();
        while (true)
        {
            Console.WriteLine("ввежиье идетификатор");
            string inputtt = Console.ReadLine();
            (int newiteratchein, int hashch1) = hashTable1.SearchChained(inputtt);
            (int newiteratre, int hashh1) = hashTable.SearchRehash(inputtt);
           
            Console.WriteLine($" для поиска с помощью цепочек было произведено {newiteratchein}   его хэш:{hashch1}\n" +
                $"для поиска с помощью простого хэширования было произведено {newiteratre}  его хэш:{hashh1}");

        }

        Console.ReadKey();
    }
}