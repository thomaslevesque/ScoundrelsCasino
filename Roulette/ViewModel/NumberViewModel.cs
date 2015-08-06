namespace Roulette.ViewModel
{
    public class NumberViewModel
    {
        public NumberViewModel(string value)
        {
            Value = value;

            int i = int.Parse(value);
            if (i == 0)
            {
                Color = "Green";
            }
            else if (i >= 1 && i <= 10 || i >= 19 && i <= 28)
            {
                Color = i%2 == 0 ? "Black" : "Red";
            }
            else
            {
                Color = i%2 == 0 ? "Red" : "Black";
            }
        }

        public string Value { get; }
        public string Color { get; }
    }
}