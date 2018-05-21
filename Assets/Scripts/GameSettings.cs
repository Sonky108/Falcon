namespace Falcon
{
    [System.Serializable]
    public class GameSettings
    {
        public float gravity;
        public float fuel;
        public float thrust;
        public float momentum;
        public float mass;

        public override string ToString()
        {
            return $"GameSettings: {gravity}, {fuel}";
        }
    }
}
