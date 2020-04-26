namespace Tonight
{
    public class GameTime
    {
        private float deltaTime = 0f;
        private float timeScale = 1f;

        public float TimeScale
        {
            get => timeScale;
            set => timeScale = value;
        }

        public float DeltaTime
        {
            get => deltaTime * timeScale;
            set => deltaTime = value;
        }

        public float DeltaTimeUnscaled
        {
            get => deltaTime;
        }

        public float TotalTimeElapsed { get; private set; }

        public void Update(float deltaTime, float totalTimeElapsed)
        {
            DeltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
    }
}