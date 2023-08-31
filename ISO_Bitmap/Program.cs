using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO_Bitmap
{
public class MessageBuilder
{
    public static string BuildTransactionMessage(string mti, List<DataElement> dataElements, string headerValue, LenType lengthType)
    {
        StringBuilder bitmap = new StringBuilder("0000000000000000000000000000000000000000000000000000000");
        foreach (DataElement de in dataElements)
        {
            bitmap[de.PositionInTheMsg] = '1';
        }

        StringBuilder tcpMsg = new StringBuilder();

        tcpMsg.Append(headerValue);




        if (lengthType == LenType.Fixed)
        {
            int messageLength = headerValue.Length + mti.Length + 4 + bitmap.Length / 8 + dataElements.Sum(de => de.Value.Length);
            tcpMsg.Append(messageLength.ToString());
        }

        tcpMsg.Append(bitmap.ToString());

        foreach (DataElement de in dataElements)
        {
            tcpMsg.Append(de.Value);
        }

        return tcpMsg.ToString();
    }
}


class Program
{
    static void Main(string[] args)
    {
        List<DataElement> balanceData = new List<DataElement>
        {
            new DataElement { Id = "DE-003", PositionInTheMsg = 3, Name = "Processing Code", Value = "560000" },
            new DataElement { Id = "DE-004", PositionInTheMsg = 4, Name = "Transaction Amount", Value = "000110100000" },

        };

        List<DataElement> cashWithdrawalData = new List<DataElement>
        {
            new DataElement { Id = "DE-003", PositionInTheMsg = 3, Name = "Processing Code", Value = "910000" },
            new DataElement { Id = "DE-004", PositionInTheMsg = 4, Name = "Transaction Amount", Value = "000001100000" },

        };

        List<DataElement> signOnData = new List<DataElement>
        {
            new DataElement { Id = "DE-003", PositionInTheMsg = 4, Name = "Processing Code", Value = "660000" },
            new DataElement { Id = "DE-011", PositionInTheMsg = 11, Name = "System Trace Number", Value = "890765" },

        };


        List<DataElement> enhancedNetworkData = new List<DataElement>
        {
            new DataElement { Id = "DE-003", PositionInTheMsg = 3, Name = "Processing Code", Value = "561000" },
            new DataElement { Id = "DE-011", PositionInTheMsg = 11, Name = "System Trace Number", Value = "12345" },
            new DataElement { Id = "DE-025", PositionInTheMsg = 25, Name = "POS Condition Code", Value = "80" },

        };

        string headerValue = "ISO8583";
        LenType lengthType = LenType.Fixed;

        string balanceMTI = "0200";
        string cashWithdrawalMTI = "0100";
        string signOnMTI = "0801";
        string enhancedNetworkMTI = "0801";


        string balance = MessageBuilder.BuildTransactionMessage(balanceMTI, balanceData, headerValue, lengthType);
        string cashWithdrawal = MessageBuilder.BuildTransactionMessage(cashWithdrawalMTI, cashWithdrawalData, headerValue, lengthType);
        string signOn = MessageBuilder.BuildTransactionMessage(signOnMTI, signOnData, headerValue, lengthType);
        string enhancedNetwork = MessageBuilder.BuildTransactionMessage(enhancedNetworkMTI, enhancedNetworkData, headerValue, lengthType);

        Console.WriteLine("Sign On Message:");
        Console.WriteLine(signOn);
        Console.WriteLine(" ");

        Console.WriteLine("Enhanced Network Message:");
        Console.WriteLine(enhancedNetwork);
        Console.WriteLine(" ");

        Console.WriteLine("Cash Withdrawal Message:");
        Console.WriteLine(cashWithdrawal);
        Console.WriteLine(" ");

        Console.WriteLine("Balance Inquiry Message:");
        Console.WriteLine(balance);

        

        Console.ReadLine();
    }
}
    public class DataElement
    {
        public string Id { get; set; }
        public int PositionInTheMsg { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public enum LenType
    {
        Fixed,
        Variable
    }
}