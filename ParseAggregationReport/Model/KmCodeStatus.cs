using System.ComponentModel;

namespace ParseReport.Model {
    public enum KmCodeStatus {
        [Description("Новый")]
        New,

        [Description("В печати")]
        Printing,

        [Description("Напечатан")]
        Printed
    }
}