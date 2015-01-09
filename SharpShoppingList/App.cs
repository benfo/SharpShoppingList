using SharpShoppingList.ViewModel;

namespace SharpShoppingList
{
    public static class App
    {
        private static ViewModelLocator locator;

        public static ViewModelLocator Locator
        {
            get { return locator ?? (locator = new ViewModelLocator()); }
        }
    }
}