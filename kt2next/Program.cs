using System;
using System.Collections.Generic;
using System.IO;

class HashTable
{
    private const int TableSize = 200;  // Размер хэш-таблицы
    private const int RehashStep = 1;   // Шаг для обычного рехеширования

    private List<string>[] chainedTable;  // Хэш-таблица для метода цепочек
    private string[] rehashTable;  // Хэш-таблица для обычного рехеширования

    public HashTable()
    {
        chainedTable = new List<string>[TableSize];
        rehashTable = new string[TableSize];

        for (int i = 0; i < TableSize; i++)
        {
            chainedTable[i] = new List<string>();
        }
    }

    // Хэширование с помощью метода цепочек
    public void HashChained(string[] identifiers)
    {
        foreach (var identifier in identifiers)
        {
            int hash = CalculateHash(identifier);
            chainedTable[hash].Add(identifier);
        }
    }

    // Хэширование с помощью обычного рехеширования
    public void HashRehash(string[] identifiers)
    {
        foreach (var identifier in identifiers)
        {
            int hash = CalculateHash(identifier);
            while (rehashTable[hash] != null)
            {
                hash = (hash + RehashStep) % TableSize;  // Рехеширование с шагом
            }
            rehashTable[hash] = identifier;
        }
    }

    // Поиск итераций для метода цепочек
    public int SearchChained(string identifier)
    {
        int hash = CalculateHash(identifier);
        int iterations = 0;

        if (chainedTable[hash] != null)
        {
            foreach (var id in chainedTable[hash])
            {
                iterations++;
                if (id == identifier)
                    return iterations;
            }
        }

        return iterations;
    }

    // Поиск итераций для обычного рехеширования
    public int SearchRehash(string identifier)
    {
        int hash = CalculateHash(identifier);
        int iterations = 0;

        while (rehashTable[hash] != null)
        {
            iterations++;
            if (rehashTable[hash] == identifier)
                return iterations;

            hash = (hash + RehashStep) % TableSize;  // Рехеширование с шагом
        }

        return iterations;
    }

    private int CalculateHash(string identifier)
    {
        // Простейшая функция хэширования: преобразуем строку в число и берем остаток от деления

        int hash = identifier.GetHashCode() % TableSize;
        return (hash < 0) ? -hash : hash;  // Обработка отрицательных значений
    }


    public void PrintChainedIdentifiers()
    {
        Console.WriteLine("Идентификаторы для метода цепочек:");
        for (int i = 0; i < TableSize; i++)
        {
            if (chainedTable[i].Count > 0)
            {
                foreach (var identifier in chainedTable[i])
                {
                    Console.Write($"{ identifier}   ");
                }
            }
        }
    }

    // Вывод всех идентификаторов для обычного рехеширования
    public void PrintRehashIdentifiers()
    {
        Console.WriteLine("Идентификаторы для обычного рехеширования:");
        for (int i = 0; i < TableSize; i++)
        {
            if (rehashTable[i] != null)
            {
                Console.Write($"{rehashTable[i]}   ");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string[] identifiers;

        // Чтение идентификаторов из файла (каждый идентификатор на новой строке)
        try
        {
            identifiers = File.ReadAllLines("C:\\Users\\Admin\\input.txt");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл с идентификаторами не найден.");
            return;
        }

        HashTable hashTable = new HashTable();

        // Хэширование с использованием метода цепочек
        hashTable.HashChained(identifiers);

        // Хэширование с использованием обычного рехеширования
        hashTable.HashRehash(identifiers);

        // Поиск итераций для примера конкретного идентификатора
        string searchIdentifier = "for";
        int chainedIterations = hashTable.SearchChained(searchIdentifier);
        int rehashIterations = hashTable.SearchRehash(searchIdentifier);


        Console.WriteLine($"Итерации для метода цепочек при поиске идентификатора \"{searchIdentifier}\": {chainedIterations}");
        Console.WriteLine($"Итерации для обычного рехеширования при поиске идентификатора \"{searchIdentifier}\": {rehashIterations}");
        // Вывод всех идентификаторов для метода цепочек
        hashTable.PrintChainedIdentifiers();

        // Вывод всех идентификаторов для обычного рехеширования
        hashTable.PrintRehashIdentifiers();
        while (true)
        {
            Console.WriteLine("ввежиье идетификатор");
            string inputtt = Console.ReadLine();
            int newiteratchein = hashTable.SearchRehash(inputtt);
            int newiteratre = hashTable.SearchRehash(inputtt);
            Console.WriteLine($" для поиска с помощью чепочек было произведено {newiteratchein}\n" +
                $"для поиска с помощью простого хэширования было произведено {newiteratre}");

        }

        Console.ReadKey();
    }
}