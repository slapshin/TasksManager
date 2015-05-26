using System;

namespace Model
{
    [Flags]
    public enum UserRole
    {
        Admin = 1,
        Master = 2,
        Customer = 4,
        Executor = 8,
        Router = 16,
        Tester = 32
    }
}