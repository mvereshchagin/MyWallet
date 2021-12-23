// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using Data;
using Utils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Utils.Serialization;

class Program
{
    #region readonly

    private static readonly string dataDir = @"..\..\Data";
    private static readonly string filterFileName = "filter";
    private static readonly string sorterFileName = "sorter";
    private static readonly string walletFileName = "wallet";

    #endregion

    #region fields

    static ExchangeRateRetriever retriever =
        new SimpleExchangeRateRetiever();

    static Currency rur = new Currency()
        {Name = "RUR", FullName = "Russian ruble"};

    static Currency usd = new Currency()
        {Name = "USD", FullName = "USA dollar"};

    static Currency eur = new Currency()
        {Name = "EUR", FullName = "Euro"};

    #endregion

    #region private methods
    
    private static void RetrieveExchangeRates()
    {
        Currency[] currencies = new Currency[] {eur, rur, usd};
        foreach (var currency in currencies)
            currency.ExchangeRate = retriever.Retrieve(currency.Name);
    }

    private static IList<MoneyItem> ApplyFilterAndSorter(Wallet wallet,
        MoneyItemsFilter? filter, MoneyItemsSorter? sorter)
    {
        var items = wallet.Filter();
        if (filter is not null)
            items = wallet.Filter(startDate: filter.DateStart, endDate: filter.DateEnd,
                currency: filter.Currency, minAmount:
                filter.MinAmount, maxAmount: filter.MaxAmount);
        object? comparer = null;
        if (sorter is not null)
            switch (sorter.Name)
            {
                case "Amount":
                    comparer = null; // new AmountComparer(sorter.Direction);
                    break;
                case "Cuurency":
                    comparer = null; // new CurrencyComparer(sorter.Direction);
                    break;
            }

        // items = ((List<MoneyItem>)items).Sort();
        return items;
    }

    private static (MoneyItemsFilter?, MoneyItemsSorter?)
        ChangSorterFilterAndPrintData(Wallet wallet,
            MoneyItemsFilter? filter, MoneyItemsSorter? sorter)
    {


        var newFilter = ChangeFilter();
        var newSorter = ChangeSorter();
        sorter = newSorter ?? sorter;
        filter = newFilter ?? filter;

        var items = ApplyFilterAndSorter(wallet, filter, sorter);
        PrintData(items);

        return (filter, sorter);
    }

    private static void PrintData(IList<MoneyItem> items)
    {

    }

    private static MoneyItemsFilter? ChangeFilter()
    {
        MoneyItemsFilter? filter = null;
        Console.WriteLine("Do you want to change filter? (y - yes, n - no(default))");
        var answer = Console.ReadLine();
        if (!String.IsNullOrEmpty(answer) &&
            answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
        {

            var dateStart = ReadDateStart();
            var dateEnd = ReadDateEnd(dateStart);

            filter = new MoneyItemsFilter() {DateStart = dateStart, DateEnd = dateEnd};
        }

        return filter;
    }

    private static MoneyItemsSorter? ChangeSorter()
    {
        MoneyItemsSorter sorter = null;
        Console.WriteLine("Do you want to change sorting? (y - yes, n - no(default))");
        var answer = Console.ReadLine();
        if (!String.IsNullOrEmpty(answer) &&
            answer.Equals("y", StringComparison.InvariantCultureIgnoreCase))
        {

            sorter = new MoneyItemsSorter();
        }

        return sorter;
    }

    private static DateTime? ReadDateStart()
    {
        DateTime? dateStart = null;
        while (true)
        {
            Console.WriteLine("Enter start date (dd.MM.yyyy) or press enter to skip");
            var answer = Console.ReadLine();
            if (!String.IsNullOrEmpty(answer))
            {
                try
                {
                    dateStart = Convert.ToDateTime(answer);
                    break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Not supported format of datetime");
                }

            }
        }

        return dateStart;
    }

    private static DateTime? ReadDateEnd(DateTime? dateStart)
    {
        DateTime? dateEnd = null;
        while (true)
        {
            Console.WriteLine("Enter end date (dd.MM.yyyy) or press enter to skip");
            string answer = Console.ReadLine();
            if (!String.IsNullOrEmpty(answer))
            {
                try
                {
                    dateEnd = Convert.ToDateTime(answer);
                    if (dateStart.HasValue && dateEnd < dateStart)
                        Console.WriteLine("End date should less than start date");
                    else
                        break;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Not supported format of datetime");
                }

            }

        }

        return dateEnd;
    }

    static Wallet LoadWallet()
    {
        Wallet wallet = new Wallet();
        wallet.Add(new MoneyItem()
        {
            Currency = eur, Amount = 2000,
            TransferDate = new DateTime(2021, 12, 5),
            TransferTarget = TransferTarget.Bribe
        });
        wallet.Add(new MoneyItem()
        {
            Currency = usd, Amount = 500,
            TransferDate = new DateTime(2020, 11, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = rur, Amount = 10000,
            TransferDate = new DateTime(2021, 12, 20)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = usd, Amount = 500,
            TransferDate = new DateTime(2019, 11, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = usd, Amount = 500,
            TransferDate = new DateTime(2019, 11, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = eur, Amount = 5000,
            TransferDate = new DateTime(2021, 12, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = eur, Amount = 300,
            TransferDate = new DateTime(2018, 4, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = rur, Amount = 12000,
            TransferDate = new DateTime(2019, 11, 5)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = rur, Amount = 24000,
            TransferDate = new DateTime(2021, 12, 3)
        });
        wallet.Add(new MoneyItem()
        {
            Currency = rur, Amount = 5345,
            TransferDate = new DateTime(2021, 12, 10)
        });
        return wallet;
    }

    #endregion
    
    static void Main(string[] args)
    {
        DataSaver dataSaver = new BinaryDataSaver(dataDir: dataDir);
        
        var filter = dataSaver.LoadItem<MoneyItemsFilter>(filterFileName);
        var sorter = dataSaver.LoadItem<MoneyItemsSorter>(sorterFileName);
        var wallet = dataSaver.LoadItem<Wallet>(walletFileName);

        RetrieveExchangeRates();

        if (wallet is null)
            wallet = LoadWallet();
        var items = ApplyFilterAndSorter(wallet, filter, sorter);
        PrintData(items);

        while (true)
        {
            var res = ChangSorterFilterAndPrintData(wallet, filter, sorter);
            filter = res.Item1;
            sorter = res.Item2;
            Console.WriteLine("If you want to continue, press - y;");
            Console.WriteLine("If you want to cancel, press - enter;");
            var answer = Console.ReadLine();
            if (!String.IsNullOrEmpty(answer) && answer.Equals("y",
                StringComparison.InvariantCultureIgnoreCase))
            {
                res = ChangSorterFilterAndPrintData(wallet, filter, sorter);
                filter = res.Item1;
                sorter = res.Item2;
            }
            else
            {
                dataSaver.SaveItem(filterFileName, filter);
                dataSaver.SaveItem(sorterFileName, sorter);
                dataSaver.SaveItem(walletFileName, wallet);
                return;
            }

        }
    }

}