namespace MooreHotelAndSuites.Domain.Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Receptionist = "Receptionist";
        public const string HouseKeeping = "HouseKeeping";

        public static readonly string[] StaffRoles =
        {
            Admin,
            Manager,
            Receptionist,
            HouseKeeping
        };

        public static readonly string[] AllRoles =
        {
            Admin,
            Manager,
            Receptionist,
            HouseKeeping,
            "Guest"
        };
    }
}
