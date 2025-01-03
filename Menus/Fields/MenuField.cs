namespace HES.Menus.Fields
{
    public enum Category
    {
        Login = 1,
        Additional = 2
    }

    class MenuField
    {
        public string name { get; set; }
        public string value { get; set; }
        public string type { get; set; }
        public Category category { get; set; }
    }
}
