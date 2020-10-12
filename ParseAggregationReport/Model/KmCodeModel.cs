using System;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ParseReport.Model
{
    public class KmCodeModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InternalId {get; set;}

        public int NumberId {get; set;}

        public string Code {get; set;}

        //TODO: remove in future
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId {get; set;}

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiptId {get; set;}

        public KmCodeStatus Status {get; set;}

        public KmCodeType Type {get; set;}

        public bool Defect {get; set;}

        public bool ExternalAdded {get; set;}

        public override string ToString() => Code;
    }

    public class ProductModel  {
        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string InternalId { get; set; }

        public int NumberId { get; set; }

        public string Code { get; set; }

        public DateTime Created { get; set; }

        public string Block { get; set; }

        public long? PlcId { get; set; }

        public CodeErrorReason ErrorStatus { get; set; }

        [BsonIgnore]
        public bool Error => ErrorStatus != CodeErrorReason.Ok;

        public int Order { get; set; }
        public string ClientId { get; set; }
    }

    public class BlockModel {

        [BsonId]
        [BsonRepresentation( BsonType.ObjectId )]
        public string InternalId { get; set; }

        public int NumberId { get; set; }

        public string Code { get; set; }

        public string [ ] Children { get; set; }

        public DateTime Created { get; set; }

        public string PackId { get; set; }
        public long? PlcId { get; set; }

        public bool IsValid {get; set;}
        public bool InBuildingPack { get; set; }
        public string ClientId { get; set; }

        [BsonIgnore]
        public virtual bool IsFree { get; set; }
    }

    public enum CodeErrorReason {
        [Description("Пусто")]
        Empty,

        [Description("OK")]
        Ok,

        [Description("Отсутствует код")]
        NoReadError,

        [Description("Некорректная длина")]
        LengthError,

        [Description("Некорректные символы")]
        DomainCharsError,

        [Description("Некорректные контрольные символы")]
        InvalidPreambuleChars,

        [Description("Некорректный GTIN")]
        InvalidGtin,


        [Description("Не найден в КМ")]
        NotFoundInKm,

        [Description("Не был напечатан")]
        NotPrinted,

        [Description("Истек срок годности заказа с кодом")]
        OrderOverdueExpired,

        [Description("Уже агрегирован")]
        WasAggregate,

        [Description("Агрегируется")]
        Aggregating,

        [Description("Принадлежит другому рецепту")]
        AnotherReceipt,

        [Description("Партия не запущена")]
        BatchNotStarted,

        [Description("КМ забракован")]
        CodeDefect,

        [Description("Не найден в коробах")]
        NotFoundInBlocks,
    }
}