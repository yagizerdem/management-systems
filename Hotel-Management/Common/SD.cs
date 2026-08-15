using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string ReceptionChief = "ReceptionChief";
        public const string Receptionist = "Receptionist";
        public const string IT = "IT";
        public const string Customer = "Customer";

        public static readonly string[] All =
        {
        Admin,
        Manager,
        ReceptionChief,
        Receptionist,
        IT,
        Customer
    };
    }
}
