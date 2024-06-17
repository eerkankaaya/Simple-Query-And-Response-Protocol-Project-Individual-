using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Properties;
public class HeaderOfSQRP
{

    private ulong dataHeader1;


    public HeaderOfSQRP()
    {
        dataHeader1 = 0;
    }


    public TypeOfMessages typeOfMessages
    {
        get { return (TypeOfMessages)((dataHeader1 >> 63) & 1); }
        set { dataHeader1 = (dataHeader1 & ~(1UL << 63)) | ((ulong)value << 63); }
    }


    public TypeOfQueries QueryType
    {
        get { return (TypeOfQueries)((dataHeader1 >> 61) & 3); }
        set { dataHeader1 = (dataHeader1 & ~(3UL << 61)) | ((ulong)value << 61); }
    }


    public ulong MessageId
    {
        get { return (dataHeader1 >> 53) & 0xFF; }
        set { dataHeader1 = (dataHeader1 & ~(0xFFUL << 53)) | ((value & 0xFFUL) << 53); }
    }


    public DateTime Timestamp
    {
        get
        {
            ulong timeStampBits = (dataHeader1 >> 27) & 0x7FFFFFFFFUL;
            int year = (int)((timeStampBits >> 35) & 0x7F) + 2020;
            int month = (int)((timeStampBits >> 31) & 0xF);
            int day = (int)((timeStampBits >> 26) & 0x1F);
            int hour = (int)((timeStampBits >> 21) & 0x1F);
            int minute = (int)((timeStampBits >> 15) & 0x3F);
            int second = (int)((timeStampBits >> 9) & 0x3F);
            return new DateTime(year, month, day, hour, minute, second);
        }
        set
        {
            int year = value.Year - 2020;
            int month = value.Month;
            int day = value.Day;
            int hour = value.Hour;
            int minute = value.Minute;
            int second = value.Second;

            ulong timeStampBits = ((ulong)year << 35) | ((ulong)month << 31) | ((ulong)day << 26) | ((ulong)hour << 21) | ((ulong)minute << 15) | ((ulong)second << 9);
            dataHeader1 = (dataHeader1 & ~(0x7FFFFFFFFUL << 27)) | (timeStampBits << 27);
        }
    }


    public StatusCode StatusCode
    {
        get { return (StatusCode)((dataHeader1 >> 24) & 0x7); }
        set { dataHeader1 = (dataHeader1 & ~(0x7UL << 24)) | ((ulong)value << 24); }
    }


    public ulong Reserved1
    {
        get { return (dataHeader1 >> 18) & 0x3F; }
        set { dataHeader1 = (dataHeader1 & ~(0x3FUL << 18)) | ((value & 0x3FUL) << 18); }
    }


    public ulong BodyLength
    {
        get { return (dataHeader1 >> 8) & 0xFF; }
        set { dataHeader1 = (dataHeader1 & ~(0xFFUL << 8)) | ((value & 0xFFUL) << 8); }
    }


    public ulong Reserved2
    {
        get { return dataHeader1 & 0xFF; }
        set { dataHeader1 = (dataHeader1 & ~0xFFUL) | (value & 0xFFUL); }
    }


    public ulong HeaderData
    {
        get { return dataHeader1; }
        set { dataHeader1 = value; }
    }
}

public enum TypeOfMessages
{
    Query = 0,
    Response = 1
}

public enum TypeOfQueries
{
    VerifyDirectoryExistence = 0,
    CheckFileExistence = 1,
    DetermineFileModification = 2,
    IdentifyModifiedFiles = 3
}

public enum StatusCode
{
    EXIST = 0b000,
    NOT_EXIST = 0b001,
    DIRECTORY_NEEDED = 0b110,
    CHANGED = 0b010,
    NOT_CHANGED = 0b011,
    SUCCESS = 0b111
}